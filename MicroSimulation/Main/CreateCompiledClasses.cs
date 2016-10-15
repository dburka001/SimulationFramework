using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MicroSimSettings;
using System.Reflection;
using System.Collections;
using System.Collections.Concurrent;

namespace MicroSimulation
{
    public partial class Simulation
    {
        Type Person;
        Type Household;
        Func<int, object, int, int, SimStepOutput> SimStep;
        Action<object, List<object>, List<Result>, int> GetResults;
        Action<object, object, string> JoinNewHousehold;
        Func<object, object> GetHouseholdIsEmpty;
        Func<object, object> GetPersonIsAlive;
        Action<object, List<Relationship>, ConcurrentBag<object>, ParallelOptions, Random> CreateNewRelationships;       

        private void createCompiledClasses()
        {
            Type ExtensionMethods;
            Type RelationshipExtensions;

            Compiler compiler = new Compiler();
            foreach (ClassField pField in Settings.PersonFields)
            {
                compiler.AddField(DynamicClassType.Person, pField);
            }
            if(ModelSettings.Instance.UseHouseholds)
            foreach (ClassField hField in Settings.HouseholdFields)
            {
                compiler.AddField(DynamicClassType.Household, hField);
            }

            try
            {
                CompilerResults result = compiler.Compile();
                Person = result.CompiledAssembly.GetType(compiler.Name + ".Person");
                if (ModelSettings.Instance.UseHouseholds) Household = result.CompiledAssembly.GetType(compiler.Name + ".Household");
                ExtensionMethods = result.CompiledAssembly.GetType(compiler.Name + ".ExtensionMethods");
                RelationshipExtensions = result.CompiledAssembly.GetType(compiler.Name + ".RelationshipExtensions");
            }
            catch (Exception)
            {
                MessageBox.Show(
                    "Failed to compile" + Environment.NewLine +
                    "For details see " + Application.StartupPath + @"\Compiler.log");
                return;
            }

            GetPersonIsAlive = createGetDelegate(Person.GetProperty("IsAlive"));

            MethodInfo method = null;
            method = ExtensionMethods.GetMethod("SimStep");
            SimStep = (Func<int, object, int, int, SimStepOutput>)Delegate.CreateDelegate(typeof(Func<int, object, int, int, SimStepOutput>), method);
            method = ExtensionMethods.GetMethod("GetResults");
            GetResults = (Action<object, List<object>, List<Result>, int>)Delegate.CreateDelegate(typeof(Action<object, List<object>, List<Result>, int>), method);

            if (ModelSettings.Instance.UseHouseholds)
            {
                GetHouseholdIsEmpty = createGetDelegate(Household.GetProperty("IsEmpty"));                
                method = ExtensionMethods.GetMethod("JoinNewHousehold");
                JoinNewHousehold = (Action<object, object, string>)Delegate.CreateDelegate(typeof(Action<object, object, string>), method);
                method = RelationshipExtensions.GetMethod("CreateNewRelationships");
                CreateNewRelationships = (Action<object, List<Relationship>, ConcurrentBag<object>, ParallelOptions, Random>)Delegate
                    .CreateDelegate(typeof(Action<object, List<Relationship>, ConcurrentBag<object>, ParallelOptions, Random>), method);
            }
        }
    }
}
