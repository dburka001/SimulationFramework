using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MicroSimulation;
using MicroSimSettings;
using MicroSimCodeBuilder;
using CefSharp;
using CefSharp.WinForms;

namespace MicroSimFramework
{
    public partial class ucSimulation : MicroSimUserControl
    {
        public Simulation Sim { get; set; }

        public event SimulationStarted Started;
        public event SimulationFinishedCompletely Stopped;

        SimulationBuilder sb;

        public ucSimulation()
        {
            InitializeComponent();
            
            this.Title = "Simulation";
            sb = new SimulationBuilder();            
            this.Disposed += UcSimulation_Disposed;         
            // this.Controls.Remove(browser);
            CheckForIllegalCrossThreadCalls = false; // TODO - Needed for progressbar?                        
            // resetForm();
            Sim = new Simulation();
            Sim.SimulationStarted += Sim_SimulationStarted;
            Sim.YearFinished += Sim_YearFinished;
            Sim.SimulationFinished += Sim_SimulationFinished;
            Sim.SimulationFinishedCompletely += Sim_SimulationFinishedCompletely;
            Sim.Canceled += Sim_Canceled;
        }

        private void Sim_Canceled(object sender, EventArgs e)
        {
            Stopped(Sim, new EventArgs());
        }

        private void UcSimulation_Disposed(object sender, EventArgs e)
        {
            Sim.CancelTokenSource.Cancel();
        }

        private void Sim_SimulationStarted(object sender, EventArgs e)
        {
            Started(sender, new EventArgs());
            ModelWebControl.Instance.GetMainFrame().ExecuteJavaScriptAsync("displayText('---START---');");
        }

        private void Sim_SimulationFinishedCompletely(object sender, EventArgs e)
        {
            Stopped(sender, new EventArgs());
            ModelWebControl.Instance.GetMainFrame().ExecuteJavaScriptAsync("displayText('---END---');");
        }

        private void Sim_SimulationFinished(object sender, SimulationFinishedEventArgs e)
        {                                    
            ModelWebControl.Instance.GetMainFrame().ExecuteJavaScriptAsync("mainProgress(''," + e.Progress + ")");
            ModelWebControl.Instance.GetMainFrame().ExecuteJavaScriptAsync("displayText('Simulation Finished');");            
        }

        private void Sim_YearFinished(object sender, YearFinishedEventArgs e)
        {         
            string inputString = e.Year.ToString();
            ModelWebControl.Instance.GetMainFrame().ExecuteJavaScriptAsync("simProgress("+ inputString + "," + e.Progress + ")");       
        }
    }
}
