using MicroSim.DataSource.DataAccess;
using MicroSim.DataSource.Entities;
using MicroSim.DataSource.ProcessLog;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MicroSim.DataSource.Core
{
    /// <summary>
    /// Handler class for an iteration of the processing of a DataSource
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <seealso cref="MicroSim.DataSource.Core.MsfDataSourcePart" />
    public abstract class MsfDataSourcePart<T> : MsfDataSourcePart
    {
        /// <summary>
        /// The input path
        /// </summary>
        protected string _inputPath;

        /// <summary>
        /// Gets or sets the data.
        /// </summary>
        /// <value>
        /// The data.
        /// </value>
        public IEnumerable<T> Data { get; set; } = new List<T>();

        /// <summary>
        /// Gets the type of the input part.
        /// </summary>
        /// <value>
        /// The type of the input part.
        /// </value>
        public override Type InputPartType => typeof(T);

        /// <summary>
        /// Initializes a new instance of the <see cref="MsfDataSourcePart" /> class.
        /// </summary>
        /// <param name="inputs">The inputs.</param>
        public MsfDataSourcePart(params MsfDataSourcePart[] inputs)
        {
            InputParts.AddRange(inputs);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MsfDataSourcePart{T}"/> class.
        /// </summary>
        /// <param name="inputPath">The input path.</param>
        public MsfDataSourcePart(string inputPath)
        {
            _inputPath = inputPath;
        }

        /// <summary>
        /// Loads the view generic.
        /// </summary>
        /// <typeparam name="TBaseUC">The type of the base uc.</typeparam>
        private void LoadViewGeneric<TBaseUC>()
        {
            if (View is IBaseUC<TBaseUC>)
                ((IBaseUC<TBaseUC>)View).LoadData(Data.Cast<TBaseUC>());
        }

        /// <summary>
        /// Generates the data.
        /// </summary>
        protected virtual void GenerateData()
        {
            Data = DataHandler.Instance.LoadFromFile<T>(_inputPath);
        }

        /// <summary>
        /// Loads the data.
        /// </summary>
        protected virtual void LoadData()
        {
            Data = DataHandler.Instance.LoadFromFile<T>(SourceFilePath());
            InvokeExtensions.InvokeAction(View, () => LoadView());
            Log.WriteLine(String.Format(Resources.LoadMessage, Title));
        }

        /// <summary>
        /// Gets the view and loads the current Data.
        /// </summary>
        /// <returns></returns>
        protected virtual void LoadView()
        {
            LoadViewGeneric<object>();
            LoadViewGeneric<IPopulationEntity>();
            LoadViewGeneric<IPopulationEduEntity>();
            LoadViewGeneric<IPopulationBirthOrderEntity>();
            LoadViewGeneric<IPopulationCompleteEntity>();
            LoadViewGeneric<IIncomeIncreaseDisplayEntity>();
        }

        /// <summary>
        /// Saves the data.
        /// </summary>
        protected virtual void SaveData()
        {
            DataHandler.Instance.SaveToFile<T>(SourceFilePath(), Data);
        }

        /// <summary>
        /// Gets the type of the input by.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        public IEnumerable<Tdata> GetInputDataOfType<Tdata>()
            => ((MsfDataSourcePart<Tdata>)InputParts.Where(p => p.GetType().IsSubclassOf(typeof(MsfDataSourcePart<Tdata>)))
            .FirstOrDefault()).Data;

        /// <summary>
        /// Loads this instance.
        /// </summary>
        public override void Load(bool forceGeneration = false)
        {
            if (!DataHandler.Instance.FileExists(SourceFilePath()) || forceGeneration)
            {
                GenerateData();
                Log.WriteLine(String.Format(Resources.GenerateMessage, Title));
                SaveData();
                LoadData();
            }
            else
            {
                try
                {
                    LoadData();
                }
                catch (Exception ex)
                {
                    Log.WriteLine(String.Format(Resources.LoadError, Title), LogLevel.Error);
                    Log.WriteLineWithIndent(ex.Message);
                    Log.WriteLine(Resources.GenerateForceAttempt);
                    Load(true);
                }
            }
        }

        /// <summary>
        /// Gets the data.
        /// </summary>
        /// <returns>
        /// The data content as an array.
        /// </returns>
        public override IEnumerable<object> GetData()
            => Data.Cast<object>();
    }
}