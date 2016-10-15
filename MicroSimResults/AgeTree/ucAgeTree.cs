using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using MicroSimSettings;
using System.Reflection;
using CefSharp.WinForms;
using CefSharp;

namespace MicroSimResults
{
    public partial class ucAgeTree : UserControl
    {
        private ModelWebControl browser;
        AgeTreeInput data;

        public ucAgeTree(AgeTreeInput data)
        {
            InitializeComponent();
            this.data = data;
            browser = ModelWebControl.Instance;
            browser.Load(Path.Combine(Environment.CurrentDirectory, @"AgeTree\index.html"));            
            mainPanel.Controls.Add(browser);          
            browser.FrameLoadEnd += Browser_FrameLoadEnd;

            hScrollYear.Minimum = 0;
            hScrollYear.Maximum = data.Years.Length - 1;            
        }

        private void Browser_FrameLoadEnd(object sender, FrameLoadEndEventArgs e)
        {
            setDisplay(data.Years[0], 0);
        }
               

        private void hScrollYear_Scroll(object sender, ScrollEventArgs e)
        {
            setDisplay(data.Years[e.NewValue], e.NewValue);
        }

        private void setDisplay(int year, int id)
        {
            browser.GetMainFrame().ExecuteJavaScriptAsync(
                "setData(" + year.ToString() +
                ", " + data.InputData[id] +
                ", " + data.xMax.ToString() +
                ", " + data.yMax.ToString() + ");");
        }
    }
}
