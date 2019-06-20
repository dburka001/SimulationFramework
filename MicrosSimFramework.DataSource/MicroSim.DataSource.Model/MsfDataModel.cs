using MicroSim.DataSource.Birth;
using MicroSim.DataSource.BirthComplete;
using MicroSim.DataSource.BirthEdu;
using MicroSim.DataSource.Core;
using MicroSim.DataSource.DataAccess;
using MicroSim.DataSource.Death;
using MicroSim.DataSource.DeathEdu;
using MicroSim.DataSource.DeterministicEducation;
using MicroSim.DataSource.Entities;
using MicroSim.DataSource.ExcelExport.Commands;
using MicroSim.DataSource.Fertility;
using MicroSim.DataSource.Mortality;
using MicroSim.DataSource.Other;
using MicroSim.DataSource.Output;
using MicroSim.DataSource.Population;
using MicroSim.DataSource.PopulationEdu;
using MicroSim.DataSource.ProcessLog;
using MicroSim.DataSource.SocialGroups;
using MicroSim.DataSource.StartingPopulation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Localizations = MicroSim.DataSource.Model.Properties.Resources;

namespace MicroSim.DataSource.Model
{
    /// <summary>
    /// Model controlling the chain of DataSource processing units
    /// </summary>
    public class MsfDataModel
    {
        private static string ExportFileNameBase = Resources.ExcelFilename;

        /// <summary>
        /// The instance
        /// </summary>
        private static MsfDataModel _instance;

        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <value>
        /// The instance.
        /// </value>
        public static MsfDataModel Instance
            => _instance ?? (_instance = new MsfDataModel());

        /// <summary>
        /// Prevents a default instance of the <see cref="MsfDataModel"/> class from being created.
        /// </summary>
        private MsfDataModel()
        {
            DataSources.Add(new DetEduDataSource(
                "Determinisztikus végzettségek.csv"));
            DataSources.Add(new PopulationDataSource(
                "Population.csv"));
            DataSources.Add(new PopulationEduDataSource(
                "edat_lfs_9901.tsv",
                GetDataSoruceOfType<PopulationDataSource>()));
            DataSources.Add(new DeathDataSource(
                "Death.csv"));
            DataSources.Add(new DeathEduDataSource(
                "demo_maeduc.tsv"));
            DataSources.Add(new BirthDataSource(
                "HUNbirthsRRbo.csv"));
            DataSources.Add(new BirthEduDataSource(
                "demo_faeduc.tsv"));
            DataSources.Add(new BirthCompleteDataSource(new string[] {
                "Szülőképes korú nők - Teljes bontás.csv",
                "Élveszületések - Teljes bontás.csv" },
                GetDataSoruceOfType<PopulationEduDataSource>(),
                GetDataSoruceOfType<BirthEduDataSource>()));
            DataSources.Add(new MortalityDataSource(
                GetDataSoruceOfType<PopulationDataSource>(),
                GetDataSoruceOfType<DeathDataSource>(),
                GetDataSoruceOfType<BirthDataSource>(),
                GetDataSoruceOfType<PopulationEduDataSource>(),
                GetDataSoruceOfType<DeathEduDataSource>(),
                GetDataSoruceOfType<BirthEduDataSource>()));
            DataSources.Add(new FertilityDataSource(
                GetDataSoruceOfType<PopulationDataSource>(),
                GetDataSoruceOfType<BirthDataSource>(),
                GetDataSoruceOfType<BirthCompleteDataSource>()));
            DataSources.Add(new SocialGroupsDataSource(new string[] {
                "Kisebbségek aránya.csv",
                "Bevándorlók.csv" }));
            DataSources.Add(new OtherDataSource(new string[] {
                "Végzettségfüggő alapértékek.csv",
                "Jövedelem növekedés.csv",
                "Szolgálati státusz.csv",
                "Szolgálati státusz ideje.csv",
                "Nyugdíj szorzók.csv",
                "Társadalmi csoport végzettség típusa.csv",
                "Nyugdíj változók.csv",
                "Gazdasági növekedés.csv" }));
            DataSources.Add(new StartingPopulationDataSource(
                GetDataSoruceOfType<PopulationDataSource>(),
                GetDataSoruceOfType<PopulationEduDataSource>(),
                GetDataSoruceOfType<BirthCompleteDataSource>(),
                GetDataSoruceOfType<SocialGroupsDataSource>()));            
            DataSources.Add(new OutputDataSource(
                GetDataSoruceOfType<StartingPopulationDataSource>(),
                GetDataSoruceOfType<MortalityDataSource>(),
                GetDataSoruceOfType<FertilityDataSource>(),
                GetDataSoruceOfType<DetEduDataSource>(),
                GetDataSoruceOfType<OtherDataSource>()));            

            Nomenclatures.Add(typeof(Gender));
            Nomenclatures.Add(typeof(Education));            
            Nomenclatures.Add(typeof(WorkStatus));
            Nomenclatures.Add(typeof(SocialGroup));
            Nomenclatures.Add(typeof(DetEducationType));
            Nomenclatures.Add(typeof(PensionType));
        }

        /// <summary>
        /// Gets the type of the data soruce of.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public MsfDataSource GetDataSoruceOfType<T>()
            => DataSources.Where(p => p.GetType() == typeof(T)).FirstOrDefault();

        /// <summary>
        /// Gets the data sources.
        /// </summary>
        /// <value>
        /// The data sources.
        /// </value>
        public List<MsfDataSource> DataSources { get; } = new List<MsfDataSource>();

        /// <summary>
        /// Gets the enumerations.
        /// </summary>
        /// <value>
        /// The enumerations.
        /// </value>
        public List<Type> Nomenclatures { get; } = new List<Type>();

        public async Task ExportAsync(
            IExportCommand exportCommand
            )
        {
            try
            {                
                Log.IncreaseIndent();
                ExportCSV<
                    DspOutputPopulationValidation, 
                    OutputPopulationValidationEntity>();
                ExportCSV<
                    DspOutputPopulation, 
                    OutputPopulationEntity>();
                ExportCSV<
                    DspOutputMortality,
                    OutputMortalityEntity>();
                ExportCSV<
                    DspOutputFertilityTotal,
                    OutputFertilityTotalEntity>();

                var fileName = MsfEnvironment.Current.GetStaticExportFileName(ExportFileNameBase, ".xlsx");

                Log.WriteLine(string.Format(Localizations.LogExportStarted, Path.GetFileName(fileName)));

                using (var target = new FileStream(fileName, FileMode.Create, FileAccess.ReadWrite))
                {
                    await exportCommand.ExportAsync(
                        target,
                        DataSources.OfType<OutputDataSource>().First(),
                        Nomenclatures.ToArray()
                        );
                }

                Log.DecreaseIndent();
                Log.WriteLine(Localizations.LogExportSuccess, LogLevel.Important);
            }
            catch (Exception ex)
            {
                Log.Exception(Localizations.LogExportFail, ex);
            }
        }

        private void ExportCSV<TDataSource, TEntity>()
        {           
            var datasource = GetDataSoruceOfType<OutputDataSource>();
            var part = datasource.GetPartOfType<TDataSource>();
            var data = (IEnumerable<TEntity>)part.GetData();
            var fileName = Path.ChangeExtension(part.Title, DataFileType.CSV.GetExtension());

            Log.WriteLine(string.Format(Localizations.LogExportStarted, fileName));

            DataHandler.Instance.SaveToFile(
                Path.Combine(Resources.ExportDirectory, fileName),
                data);
        }
    }
}
