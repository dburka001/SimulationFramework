using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace MicroSimSettings
{
    public class ModelSettings
    {
        [ScriptIgnore]
        public static ModelSettings Instance { get; set; } = new ModelSettings();

        // Load
        public string PopulationDataFileName { get; set; }
        public string ParametersFileName { get; set; }

        // Simulation
        public int StartYear { get; set; }
        public int EndYear { get; set; }
        public DefaultField MultiplierField { get; set; }        
        public bool UseHouseholds { get; set; }

        // Defaults
        public List<DefaultField> DefaultFields { get; set; }
        public List<DefaultType> DefaultTypes { get; set; }
        public ClassField DefaultPersonField { get; set; }
        public Constant DefaultConstant { get; set; }
        public DefaultIdField DefaultHouseholdIdField { get; set; }
        public ClassField DefaultHouseholdField { get; set; }
        public RelationshipType DefaultRelationshipType { get; set; }
        public RelationshipGrouping DefaultRelationshipGroupingVariable { get; set; }

        // Containers
        public List<ClassField> PersonFields { get; set; }        
        public List<Constant> Constants { get; set; }        

        // Households + RelationShips 
        public bool AllowConflictingRelationships { get; set; }
        public List<DefaultIdField> HouseholdIdFields { get; set; }
        public List<ClassField> HouseholdFields { get; set; }
        public List<RelationshipType> RelationshipTypes { get; set; }   
        public List<ExcludeTypeContainer> ExcludeTypes { get; set; }             

        // Blockly        
        public string XML_SimStep { get; set; }
        public string Code_SimStep { get; set; }
        public string XML_NewBorn { get; set; }
        public string Code_NewBorn { get; set; }
        public string XML_HouseholdJoinNew { get; set; }
        public string Code_HouseholdJoinNew { get; set; }
        public string XML_ResultSettings { get; set; }
        public string Code_ResultSettings { get; set; }

        public ModelSettings()
        {            
            ResetToDefault();
        }

        public void ResetToDefault()
        {
            this.StartYear = 2005;
            this.EndYear = 2007;            
            this.UseHouseholds = true;

            this.DefaultTypes = new List<DefaultType>();
            this.PersonFields = new List<ClassField>();            
            this.Constants = new List<Constant>();

            this.HouseholdFields = new List<ClassField>();
            this.HouseholdIdFields = new List<DefaultIdField>();
            this.RelationshipTypes = new List<RelationshipType>();
            this.ExcludeTypes = new List<ExcludeTypeContainer>();

            this.XML_SimStep = "";
            this.Code_SimStep = "";
            this.XML_NewBorn = "";
            this.Code_NewBorn = "";
            this.XML_HouseholdJoinNew = "";
            this.Code_HouseholdJoinNew = "";
            this.XML_ResultSettings = "";
            this.Code_ResultSettings = "";
        }      
    }
}
