using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MicroSimSettings;

namespace MicroSimulation
{
    public partial class Compiler
    {
        private void createRelationShipExtensions()
        {
            bool allowConflict = ModelSettings.Instance.AllowConflictingRelationships;
            //bool parallel = allowConflict;
            bool parallel = false;
            mainCode += "\tpublic static class RelationshipExtensions" + Environment.NewLine + "\t{" + Environment.NewLine;
            mainCode += "\t\tpublic static void CreateNewRelationships";
            mainCode += "(object inputPopulation, List<Relationship> newRelationships, ConcurrentBag<object> newHouseholds, ParallelOptions pOptions, Random GlobalRng)";
            mainCode += Environment.NewLine + "\t\t{" + Environment.NewLine;
            if (!parallel)
            {
                mainCode += "\t\t\tList<int> targetIds = new List<int>();" + Environment.NewLine;
                mainCode += "\t\t\tList<int> targetCounts = new List<int>();" + Environment.NewLine;
            }
            mainCode += "\t\t\tList<Person> Population = (List<Person>)inputPopulation;" + Environment.NewLine;          
            mainCode += "\t\t\tforeach (RelationshipType rst in ModelSettings.Instance.RelationshipTypes)" + Environment.NewLine;
            mainCode += "\t\t\t{" + Environment.NewLine;            
            mainCode += "\t\t\t\tstring relationshipType = rst.Name;" + Environment.NewLine;
            mainCode += "\t\t\t\tvar currentPeople = newRelationships.Where(x => x.Type == relationshipType).Select(x => (Person)x.Person).ToList<Person>();" + Environment.NewLine;            
            foreach (RelationshipType rst in ModelSettings.Instance.RelationshipTypes)
            {
                int n = 4;
                mainCode += Environment.NewLine;
                mainCode += indent(n) + "if(relationshipType == \"" + rst.Name + "\")" + Environment.NewLine;
                mainCode += indent(n) + "{" + Environment.NewLine; n++;
                mainCode += indent(n) + "var sourceGroups" + createGroupBy(rst, false);
                mainCode += indent(n) + "var targetGroups" + createGroupBy(rst, true);                
                if (parallel)
                {
                    mainCode += indent(n) + "int[] randomSeeds = new int[sourceGroups.Count];" + Environment.NewLine;
                    mainCode += indent(n) + "for (int rngId = 0; rngId < randomSeeds.Length; rngId++)" + Environment.NewLine;
                    mainCode += indent(n) + "{" + Environment.NewLine; n++;
                    mainCode += indent(n) + "randomSeeds[rngId] = GlobalRng.Next();" + Environment.NewLine;
                    n--;  mainCode += indent(n) + "}" + Environment.NewLine;
                    mainCode += indent(n) + "Parallel.For(0, sourceGroups.Count, pOptions, (i) => " + Environment.NewLine;
                    mainCode += indent(n) + "{" + Environment.NewLine; n++;
                    mainCode += indent(n) + "int j = i;" + Environment.NewLine;
                    mainCode += indent(n) + "var sourceGroup = sourceGroups[j];" + Environment.NewLine;
                    mainCode += indent(n) + "Random rng = new Random(randomSeeds[j]);" + Environment.NewLine;
                    mainCode += indent(n) + "List<int> targetIds = new List<int>();" + Environment.NewLine;
                    mainCode += indent(n) + "List<int> targetCounts = new List<int>();" + Environment.NewLine;
                }
                else
                {
                    mainCode += indent(n) + "foreach (var sourceGroup in sourceGroups)" + Environment.NewLine;
                    mainCode += indent(n) + "{" + Environment.NewLine; n++;
                }
                mainCode += indent(n) + "foreach (var sourcePerson in sourceGroup.Members)" + Environment.NewLine;
                mainCode += indent(n) + "{" + Environment.NewLine; n++;                
                mainCode += indent(n) + "int keyDistance = -1;" + Environment.NewLine;
                mainCode += indent(n) + "double keyDistanceProbability = " + (parallel?"rng":"GlobalRng") + ".NextDouble();" + Environment.NewLine;
                for (int keyId = 0; keyId < rst.GetProbabilities().Length; keyId++)
                {
                    mainCode += indent(n);
                    if (keyId != 0) mainCode += "else ";
                    double[] currentP = rst.GetProbabilities();
                    mainCode += "if(keyDistanceProbability <= " + currentP[keyId].ToString().Replace(',', '.') + ") keyDistance = "
                        + keyId.ToString() + ";" + Environment.NewLine;
                }                
                mainCode += Environment.NewLine;                
                mainCode += indent(n) + "targetIds.Clear();" + Environment.NewLine;
                mainCode += indent(n) + "targetCounts.Clear();" + Environment.NewLine;
                mainCode += indent(n) + "for (int tgId = 0; tgId < targetGroups.Count; tgId++)" + Environment.NewLine;
                mainCode += indent(n) + "{" + Environment.NewLine; n++;
                mainCode += indent(n) + "var targetGroup = targetGroups[tgId];" + Environment.NewLine;                
                mainCode += indent(n) + "int currentKeyDistance = 0;" + Environment.NewLine;
                foreach (RelationshipGrouping rtGroup in rst.GroupingVariables.Where(x => !x.IsExclude))
                {
                    mainCode += indent(n) + "currentKeyDistance += (int)Math.Abs(";
                    mainCode += "targetGroup.Key." + rtGroup.Field.Name + " - ";
                    mainCode += "sourceGroup.Key." + rtGroup.Field.Name;
                    mainCode += ");" + Environment.NewLine;
                }
                mainCode += Environment.NewLine;
                mainCode += indent(n) + "if(currentKeyDistance != keyDistance) continue;" + Environment.NewLine;
                mainCode += Environment.NewLine;
                mainCode += indent(n) + "targetIds.Add(tgId);" + Environment.NewLine;
                mainCode += indent(n) + "targetCounts.Add(targetGroup.Members.Count());" + Environment.NewLine;                
                n--; mainCode += indent(n) + "}" + Environment.NewLine;
                mainCode += Environment.NewLine;


                mainCode += indent(n) + "int selectedTargetId = " + (parallel ? "rng" : "GlobalRng") + ".Next(targetCounts.Sum());" + Environment.NewLine;
                mainCode += indent(n) + "Person targetPerson = null;" + Environment.NewLine;
                mainCode += indent(n) + "for (int tgId = 0; tgId < targetIds.Count(); tgId++)" + Environment.NewLine;
                mainCode += indent(n) + "{" + Environment.NewLine; n++;
                mainCode += indent(n) + "if(targetCounts[tgId] <= selectedTargetId)" + Environment.NewLine;
                mainCode += indent(n) + "{" + Environment.NewLine; n++;
                mainCode += indent(n) + "selectedTargetId -= targetCounts[tgId];" + Environment.NewLine;
                n--; mainCode += indent(n) + "}" + Environment.NewLine;                
                mainCode += indent(n) + "else" + Environment.NewLine;
                mainCode += indent(n) + "{" + Environment.NewLine; n++;
                mainCode += indent(n) + "var targetGroup = targetGroups[targetIds[tgId]];" + Environment.NewLine;
                mainCode += indent(n) + "targetPerson = targetGroup.Members[selectedTargetId];" + Environment.NewLine;
                // if(!allowConflict) mainCode += indent(n) + "targetGroup.Members.RemoveAt(selectedTargetId);" + Environment.NewLine;
                mainCode += indent(n) + "break;" + Environment.NewLine;
                n--; mainCode += indent(n) + "}" + Environment.NewLine;                               
                n--; mainCode += indent(n) + "}" + Environment.NewLine;
                mainCode += indent(n) + "if(targetPerson == null || targetPerson == sourcePerson) continue;" + Environment.NewLine;
                mainCode += indent(n) + "Household h = new Household();" + Environment.NewLine;
                mainCode += indent(n) + "newHouseholds.Add(h);" + Environment.NewLine;                
                mainCode += indent(n) + "ExtensionMethods.JoinNewHousehold(sourcePerson, h, \"" + rst.Name + "\");" + Environment.NewLine;
                mainCode += indent(n) + "ExtensionMethods.JoinNewHousehold(targetPerson, h, \"" + rst.Name + "\");" + Environment.NewLine;                
                n--; mainCode += indent(n) + "}" + Environment.NewLine; // foreach sourceGroup.Members
                n--; mainCode += indent(n) + "}" + (parallel ? ");" : "") + Environment.NewLine; // foreach sourceGroups                
                n--; mainCode += indent(n) + "}" + Environment.NewLine; // if relationshipType
            }            
            mainCode += "\t\t\t}" + Environment.NewLine; // foreach RelationshipTypes
            mainCode += "\t\t}" + Environment.NewLine + Environment.NewLine; // CreateNewRelationships
            mainCode += "\t}" + Environment.NewLine; // RelationshipExtensions
        }

        private string createGroupBy(RelationshipType rst, bool useExclusions)
        {
            string groupby = " = " + (useExclusions ? "Population" : "currentPeople");
            var currentExclusions = rst.GroupingVariables.Where(x => x.IsExclude).ToList<RelationshipGrouping>();
            var currentGrouping = rst.GroupingVariables.Where(x => !x.IsExclude).ToList<RelationshipGrouping>();
            if (useExclusions)
            {
                groupby += ".Where(p => p.IsAlive";
                for (int i = 0; i < currentExclusions.Count(); i++)
                {                    
                    RelationshipGrouping rsg = currentExclusions[i];
                    groupby += " && ";
                    switch (rsg.ExcludeType)
                    {
                        case ExcludeType.Equals:
                            groupby += "p." + rsg.Field.Name + ".ToString() != \"" + rsg.ExcludeValue.ToString() + "\"";
                            break;
                        case ExcludeType.LowerThan:
                            groupby += "(double)p." + rsg.Field.Name + " < " + rsg.ExcludeValue.ToString();
                            break;
                        case ExcludeType.HigherThan:
                            groupby += "(double)p." + rsg.Field.Name + " > " + rsg.ExcludeValue.ToString();
                            break;
                        default:
                            break;
                    }                    
                }
                groupby += ")";
            }
            groupby += ".GroupBy(p => new { ";
            for (int i = 0; i < currentGrouping.Count(); i++)
            {
                RelationshipGrouping rsg = currentGrouping[i];
                if (i != 0) groupby += ", ";
                groupby += rsg.Field.Name + " = p." + rsg.Field.Name;
            }
            groupby += " }, (key, g) => new { Key = key, Members = g.ToList<Person>() })";
            groupby += ".ToList()";
            groupby += ";" + Environment.NewLine;

            return groupby;
        }

        private string indent(int n)
        {
            return new String('\t', n);
        }
    }
}
