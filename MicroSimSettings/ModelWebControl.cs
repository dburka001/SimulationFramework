using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CefSharp;
using CefSharp.WinForms;

namespace MicroSimSettings
{
    public class ModelWebControl : ChromiumWebBrowser
    {
        public bool IsReady { get; set; }
        public static ModelWebControl Instance { get; set; } = new ModelWebControl("");

        public ModelWebControl(string address) : base(address)
        {
            IsReady = false;
            this.Dock = System.Windows.Forms.DockStyle.Fill;            
            this.ConsoleMessage += Web_ConsoleMessage;  
        }

        private void Web_ConsoleMessage(object sender, ConsoleMessageEventArgs e)
        {
            Console.WriteLine("JavasScript: " + e.Message + " On line: " + e.Line.ToString());
        }
    }
}
