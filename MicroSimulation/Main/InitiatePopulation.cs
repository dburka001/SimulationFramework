using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MicroSimSettings;
using System.Reflection;
using System.Linq.Expressions;

namespace MicroSimulation
{
    public partial class Simulation
    {
        IList Population;
        List<object> Households;
        List<ClassField> currentPersonFields;
        List<ClassField> currentHouseholdFields;

        Func<object, object> PersonClone;
        Action<object, object>[] PersonFieldSet;
        Func<object, object> HouseholdClone;
        Action<object, object>[] HouseholdFieldSet;
        PropertyInfo members;
        Func<object, object> HouseholdMembersGet;
        Action<object, object> PersonHouseholdSet;

        private void initiatePopulation()
        {
            Population = (IList)Activator.CreateInstance(typeof(List<>).MakeGenericType(Person));            
            currentPersonFields = (from x in Settings.PersonFields where x.DefaultDataField != null select x).ToList<ClassField>();

            if (Settings.UseHouseholds)
            {
                //Households = (IList)Activator.CreateInstance(typeof(List<>).MakeGenericType(Household));                
                Households = new List<object>();
                currentHouseholdFields = (from x in Settings.HouseholdFields where x.DefaultDataField.ColumnName != " " select x).ToList<ClassField>();
            }

            createDelegates();

            if (Settings.UseHouseholds)
            {
                var groupedDataRows = ModelData.Instance.PopulationData.AsEnumerable()
                    .GroupBy(r => new NTuple<object>(from column in Settings.HouseholdIdFields select r[column.Field.ColumnName]))
                    .Select(r => r);
                foreach (var group in groupedDataRows)
                {                    
                    createPeople(group.ToArray<DataRow>());
                }    
            }
            else createPeople(ModelData.Instance.PopulationData.Select());
        }

        private void createPeople(DataRow[] currentDataRows)
        {
            bool isFirst = false;
            int multiplier = 1; // Multiplier has to be the same for every person in household        

            object currentHousehold = null;
            IList currentMembers = null;            
            if (Settings.UseHouseholds)
            {
                isFirst = true;
                currentHousehold = Activator.CreateInstance(Household);
                currentMembers = (IList)HouseholdMembersGet(currentHousehold);
                Households.Add(currentHousehold);                
            }

            int counter = 0;
            foreach (DataRow row in currentDataRows)
            {
                counter++;
                CancelTokenSource.Token.ThrowIfCancellationRequested();
                if (isFirst)
                {
                    isFirst = false;
                    for (int fieldId = 0; fieldId < currentHouseholdFields.Count; fieldId++)
                    {
                        ClassField field = currentHouseholdFields[fieldId];
                        object currentValue = Convert.ChangeType(row.Field<string>(field.DefaultDataField.ColumnName), field.DefaultType.Type);
                        if (field.DefaultType.Type == typeof(string)) currentValue = "\"" + currentValue + "\"";
                        HouseholdFieldSet[fieldId](currentHousehold, currentValue);
                    }
                }
                object currentPerson = Activator.CreateInstance(Person);                
                for (int fieldId = 0; fieldId < currentPersonFields.Count; fieldId++)
                {
                    ClassField field = currentPersonFields[fieldId];                    
                    object currentValue = Convert.ChangeType(row.Field<string>(field.DefaultDataField.ColumnName), field.DefaultType.Type);
                    if (field.DefaultType.Type == typeof(string)) currentValue = "\"" + currentValue + "\"";
                    PersonFieldSet[fieldId](currentPerson, currentValue);
                }
                if (Settings.MultiplierField != null)
                    multiplier = Convert.ToInt32(row.Field<string>(Settings.MultiplierField.ColumnName));
                if (multiplier == 0)
                    continue;
                Population.Add(currentPerson);
                if (Settings.UseHouseholds)
                {
                    PersonHouseholdSet(currentPerson, currentHousehold);                    
                    currentMembers.Add(currentPerson);
                    continue;
                }                                               
                for (int i = 1; i < multiplier; i++)
                {
                    Population.Add(PersonClone(currentPerson));
                }
                
            }

            if (Settings.UseHouseholds && Settings.MultiplierField != null)
            {
                for (int i = 1; i < multiplier; i++)
                {
                    object newHousehold = HouseholdClone(currentHousehold);
                    Households.Add(newHousehold);                   
                    foreach (var currentPerson in currentMembers)
                    {                        
                        object newPerson = PersonClone(currentPerson);
                        PersonHouseholdSet(newPerson, newHousehold);
                        ((IList)HouseholdMembersGet(newHousehold)).Add(newPerson);                        
                        Population.Add(newPerson);
                    }                    
                }
            }
        }

        private void createDelegates()
        {         
            PersonClone = createCloneDelegate(Person);
            PersonFieldSet = new Action<object, object>[currentPersonFields.Count];
            for (int fieldId = 0; fieldId < currentPersonFields.Count; fieldId++)
            {
                ClassField field = currentPersonFields[fieldId];
                PersonFieldSet[fieldId] = createSetDelegate(Person.GetProperty(field.Name));
            }

            if (Settings.UseHouseholds)
            {
                HouseholdClone = createCloneDelegate(Household);
                HouseholdFieldSet = new Action<object, object>[currentHouseholdFields.Count];
                for (int fieldId = 0; fieldId < currentHouseholdFields.Count; fieldId++)
                {
                    ClassField field = currentHouseholdFields[fieldId];
                    HouseholdFieldSet[fieldId] = createSetDelegate(Household.GetProperty(field.Name));
                }
                members = Household.GetProperty("Members");
                HouseholdMembersGet = createGetDelegate(members);
                PersonHouseholdSet = createSetDelegate(Person.GetProperty("Household"));  
            }
        }

        ParameterExpression value = Expression.Parameter(typeof(object), "value");
        ParameterExpression instance = Expression.Parameter(typeof(object), "instance");        

        private Action<object, object> createSetDelegate(PropertyInfo currentProperty)
        {
            // value as T is slightly faster than (T)value, so if it's not a value type, use that 
            UnaryExpression instanceCast = (!currentProperty.DeclaringType.IsValueType) ? Expression.TypeAs(instance, currentProperty.DeclaringType) : Expression.Convert(instance, currentProperty.DeclaringType);
            UnaryExpression valueCast = (!currentProperty.PropertyType.IsValueType) ? Expression.TypeAs(value, currentProperty.PropertyType) : Expression.Convert(value, currentProperty.PropertyType);
            return Expression.Lambda<Action<object, object>>(Expression.Call(instanceCast, currentProperty.GetSetMethod(), valueCast), new ParameterExpression[] { instance, value }).Compile();
        }

        private Func<object, object> createGetDelegate(PropertyInfo currentProperty)
        {
            UnaryExpression instanceCast = (!currentProperty.DeclaringType.IsValueType) ? Expression.TypeAs(instance, currentProperty.DeclaringType) : Expression.Convert(instance, currentProperty.DeclaringType);
            return Expression.Lambda<Func<object, object>>(Expression.TypeAs(Expression.Call(instanceCast, currentProperty.GetGetMethod()), typeof(object)), instance).Compile();
        }

        private Func<object, object> createCloneDelegate(Type currentType)
        {
            UnaryExpression instanceCast = (!currentType.IsValueType) ? Expression.TypeAs(instance, currentType) : Expression.Convert(instance, currentType);
            return Expression.Lambda<Func<object, object>>(Expression.TypeAs(Expression.Call(instanceCast, currentType.GetMethod("Clone")), typeof(object)), instance).Compile();
        }
    }
}
