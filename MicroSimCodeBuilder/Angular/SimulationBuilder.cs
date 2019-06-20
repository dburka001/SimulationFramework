using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MicroSimulation;
using System.Threading;
using System.Windows.Forms;
using System.IO;
using MicroSimSettings;

namespace MicroSimCodeBuilder
{
    public class SimulationBuilder : AngularBuilder
    {
        public SimulationBuilder() : base("Simulation") { }
    }

    public class SimulationBinding : AngularBinding
    {
        public Simulation Sim { get; set; }

        public void OnStartClicked()
        {
            Thread threadGetFile = new Thread(new ThreadStart(Sim.Run));
            threadGetFile.SetApartmentState(ApartmentState.STA);
            threadGetFile.Start();            
        }

        public void OnCancelClicked()
        {
            Sim.Cancel();
        }

        public void OnSaveClicked()
        {
            Thread threadGetFile = new Thread(new ThreadStart(Save));
            threadGetFile.SetApartmentState(ApartmentState.STA);
            threadGetFile.Start();
        }

        private void Save()
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "Simulation Result (.sr) | *.sr";
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                using (StreamWriter sw = new StreamWriter(sfd.FileName, false, Encoding.Default))                    
                    sw.Write(Sim.ResultsAsString());                
            }
        }        
    }
}
