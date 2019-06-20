using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MicroSimSettings;
using System.IO;
using CefSharp;
using MicroSimCodeBuilder;
using Newtonsoft.Json;
using System.Globalization;

namespace MicroSimFramework
{
    public partial class MainForm : Form
    {
        private static List<Type> ucTypesDefault = 
            new List<Type>() {
                typeof(ucLoad),
                typeof(ucSettings),
                typeof(ucSimStep),
                typeof(ucNewBorn),
                typeof(ucHouseholdJoinNew),
                typeof(ucResultSettings),
                typeof(ucSimulation),
                typeof(ucResults)
            };
        private List<Type> ucTypes;        
        private MicroSimUserControl currentUC = null;
        private SimulationBinding SimBinding;

        public MainForm()
        {
            InitializeComponent();
            Icon = Properties.Resources.icon;
            CultureInfo.DefaultThreadCurrentCulture = CultureInfo.CreateSpecificCulture("en-US");
            Cef.Initialize(new CefSettings());
            SimBinding = new SimulationBinding();
            ModelWebControl.Instance.RegisterJsObject("pageLoadBinder", new PageLoadBinding());
            ModelWebControl.Instance.RegisterJsObject("loadBinder", new LoadBinding());
            ModelWebControl.Instance.RegisterJsObject("simulationBinder", SimBinding);
            ucTypes = ucTypesDefault.ToList();            
            load(Properties.Settings.Default.DefaultPath);
            setDefaults();
            // loadSettings();                
            createNewUC(0);
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            createNewUC(ucTypes.FindIndex(x => x == currentUC.GetType()) + 1);
        }

        private void btnPrevious_Click(object sender, EventArgs e)
        {
            createNewUC(ucTypes.FindIndex(x => x == currentUC.GetType()) - 1);
        }

        private void createNewUC(int id)
        {
            if (currentUC != null)
            {                
                Type currentType = currentUC.GetType();
                if (!currentUC.FinalizeAdjustments()) return;
                if (currentType == typeof(ucSimStep))
                {
                    ucTypes = ucTypesDefault.ToList();
                    if (!ModelSettings.Instance.XML_SimStep.Contains("<block type=\"born\"")) ucTypes.Remove(typeof(ucNewBorn));
                    if (!ModelSettings.Instance.UseHouseholds) ucTypes.Remove(typeof(ucHouseholdJoinNew));
                }
                if (currentType == typeof(ucSimulation))
                {
                    ((ucSimulation)currentUC).Started -= MainForm_Started;
                    ((ucSimulation)currentUC).Stopped -= MainForm_Stopped;
                    ((ucSimulation)currentUC).Sim.Dispose();
                }
            }

            if (id == 0) btnPrevious.Enabled = false; else btnPrevious.Enabled = true;
            if (id == ucTypes.Count - 1) btnNext.Enabled = false; else btnNext.Enabled = true;

            MicroSimUserControl newUC = getUCfromID(id);            
            panelMain.Controls.Clear();
            newUC.Dock = DockStyle.Fill;
            lblTitle.Text = newUC.Title;
            panelMain.Controls.Add(newUC);            

            currentUC = newUC;

            if (currentUC.GetType() == typeof(ucSimulation))
            {
                SimBinding.Sim = ((ucSimulation)currentUC).Sim;
                ((ucSimulation)currentUC).Started += MainForm_Started;
                ((ucSimulation)currentUC).Stopped += MainForm_Stopped;                
            }            
        }
        
        private void MainForm_Started(object sender, EventArgs e)
        {
            btnPrevious.Enabled = false;
            btnNext.Enabled = false;
        }

        private void MainForm_Stopped(object sender, EventArgs e)
        {
            btnPrevious.Enabled = true;
            btnNext.Enabled = true;
        }

        private MicroSimUserControl getUCfromID(int id)
        {            
            if (id < 0 || id >= ucTypes.Count) return null;            
            return (MicroSimUserControl)Activator.CreateInstance(ucTypes[id]);            
        }

        private void setDefaults()
        {
            List<ExcludeType> excludeTypeList = Enum.GetValues(typeof(ExcludeType)).Cast<ExcludeType>().ToList<ExcludeType>();
            ModelSettings.Instance.ExcludeTypes = new List<ExcludeTypeContainer>();
            foreach (var item in excludeTypeList)
            {
                ModelSettings.Instance.ExcludeTypes.Add(new ExcludeTypeContainer(item.ToString(), item));
            }
            ModelSettings.Instance.DefaultTypes = new List<DefaultType>()
            {
                new DefaultType("String", typeof(string)),
                new DefaultType("Bool", typeof(bool)),
                new DefaultType("Int", typeof(int)),
                new DefaultType("Double", typeof(double)),
                new DefaultType("Float", typeof(float))
            };
            ModelSettings.Instance.DefaultPersonField = new ClassField(ModelSettings.Instance.DefaultTypes[2], "Field");
            ModelSettings.Instance.DefaultConstant = new Constant("Constant", 1);            
            ModelSettings.Instance.DefaultHouseholdIdField = new DefaultIdField();
            ModelSettings.Instance.DefaultHouseholdField = new ClassField(ModelSettings.Instance.DefaultTypes[2], "Field");
            ModelSettings.Instance.DefaultRelationshipType = new RelationshipType("RelationshipType", "");
            ModelSettings.Instance.DefaultRelationshipGroupingVariable = new RelationshipGrouping();            
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "Json files (.json) | *.json";
            if (sfd.ShowDialog() == DialogResult.OK && currentUC.FinalizeAdjustments())
            {
                createNewUC(ucTypes.FindIndex(x => x == currentUC.GetType()));
                string jsonString = JsonConvert.SerializeObject(ModelSettings.Instance);                
                StreamWriter sw = new StreamWriter(sfd.FileName, false, Encoding.UTF8);
                sw.WriteLine(jsonString);
                sw.Close();
                Properties.Settings.Default.DefaultPath = sfd.FileName;
                Properties.Settings.Default.Save();
            }
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Any unsaved changes will be lost\nAre you sure you want to load different settings?", "Load", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                OpenFileDialog ofd = new OpenFileDialog();
                ofd.Filter = "Json files (.json) | *.json";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    load(ofd.FileName);
                }
            }
        }

        private void load(string filename)
        {
            if (File.Exists(filename))
            {
                StreamReader sr = new StreamReader(filename, Encoding.Default);
                string jsontString = sr.ReadToEnd();
                ModelSettings.Instance.ResetToDefault();
                ModelData.Instance.ResetToDefault();
                ModelSettings.Instance = JsonConvert.DeserializeObject<ModelSettings>(jsontString);
                sr.Close();
                Properties.Settings.Default.DefaultPath = filename;
                Properties.Settings.Default.Save();
                if (currentUC != null)
                {
                    currentUC.Controls.Clear();
                    currentUC.Dispose();
                    currentUC = null;
                }
            }
            createNewUC(0);
        }

        private void loadSettings() // TEMP
        {
            ModelSettings model = ModelSettings.Instance;
            // Load data
            model.PopulationDataFileName = @"c:\Users\dburk\EGYETEM\Corvinus\Gazdaságinformatikus\PhD\Disszertáció\MikroSzimuláció\Adatofrrások\szem.csv";
            model.ParametersFileName = @"c:\Users\dburk\EGYETEM\Corvinus\Gazdaságinformatikus\PhD\Disszertáció\MikroSzimuláció\Adatofrrások\nómneklatúrák és paramétertáblák.xlsx";                        
            // Settings
            model.StartYear = 2005;
            model.EndYear = 2007;            
            model.UseHouseholds = true;
            model.AllowConflictingRelationships = true;
            /*
            model.HouseholdIdFields.Add("TERUL");
            model.HouseholdIdFields.Add("SZLOK");
            model.HouseholdIdFields.Add("CIMSSZ");
            model.HouseholdIdFields.Add("HSOR");
            */
            
            //model.RelationshipTypes.Add(new RelationshipType("Life Partner", new List<double>() { 65, 25, 10 }));
            model.Constants.Add(new Constant("MaleBirthProbability", 0.514));
            ModelSettings.Instance.MultiplierField = new DefaultField("SZORZO");
            ModelSettings.Instance.PersonFields.Add(new ClassField(new DefaultType("Int", typeof(int)), "BirthYear", "1900", new DefaultField("SZEV")));
            ModelSettings.Instance.PersonFields.Add(new ClassField(new DefaultType("Int", typeof(int)), "Gender", "1", new DefaultField("NEME")));
            ModelSettings.Instance.PersonFields.Add(new ClassField(new DefaultType("Int", typeof(int)), "CSLAS", "1", new DefaultField("CSLAS")));
            ModelSettings.Instance.PersonFields.Add(new ClassField(new DefaultType("Int", typeof(int)), "Regio", "2", new DefaultField("REGIO")));
            model.RelationshipTypes.Add(new RelationshipType("Marriage", "65;25;10"));
            ModelSettings.Instance.RelationshipTypes[0].GroupingVariables.Add(new RelationshipGrouping(ModelSettings.Instance.PersonFields[1], true, ExcludeType.Equals, 2));
            ModelSettings.Instance.RelationshipTypes[0].GroupingVariables.Add(new RelationshipGrouping(ModelSettings.Instance.PersonFields[0], true, ExcludeType.LowerThan, 1990));
            ModelSettings.Instance.RelationshipTypes[0].GroupingVariables.Add(new RelationshipGrouping(ModelSettings.Instance.PersonFields[2], false, null, null));
            ModelSettings.Instance.RelationshipTypes[0].GroupingVariables.Add(new RelationshipGrouping(ModelSettings.Instance.PersonFields[3], false, null, null));

            // DefaulXML
            StreamReader sr;
            
            sr = new StreamReader("TempXML/SimStep.xml", Encoding.Default);
            while (!sr.EndOfStream)
            {
                ModelSettings.Instance.XML_SimStep += sr.ReadLine();
            }
            sr.Close();

            sr = new StreamReader("TempXML/NewBorn.xml", Encoding.Default);
            while (!sr.EndOfStream)
            {
                ModelSettings.Instance.XML_NewBorn += sr.ReadLine();
            }
            sr.Close();

            sr = new StreamReader("TempXML/ResultSettings.xml", Encoding.Default);
            while (!sr.EndOfStream)
            {
                ModelSettings.Instance.XML_ResultSettings += sr.ReadLine();
            }
            sr.Close();
        }
    }
}
