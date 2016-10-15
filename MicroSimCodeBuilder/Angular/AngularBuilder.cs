using CefSharp.WinForms;
using CefSharp;
using MicroSimSettings;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace MicroSimCodeBuilder
{
    public class AngularBuilder
    {
        protected ModelWebControl browser;

        public AngularBuilder(string type)
        {            
            browser = ModelWebControl.Instance;
            browser.IsReady = false;
            browser.Load(Path.Combine(Environment.CurrentDirectory, @"Angular\" + type + ".html"));            
            if(type != "Simulation") browser.FrameLoadEnd += Browser_FrameLoadEnd;
        }

        public static void GetSettings()
        {            
            string resultString = (ModelWebControl.Instance.GetMainFrame().EvaluateScriptAsync("save();", null)).Result.Result.ToString();
            string jsonString = Encoding.UTF8.GetString(Convert.FromBase64String(resultString));
            ModelSettings.Instance = JsonConvert.DeserializeObject<ModelSettings>(jsonString);            
        }

        private void Browser_FrameLoadEnd(object sender, FrameLoadEndEventArgs e)
        {
            SetSettings();
            browser.FrameLoadEnd -= Browser_FrameLoadEnd;            
        }

        public static void SetSettings()
        {
            string base64json = Convert.ToBase64String(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(ModelSettings.Instance)));
            ModelWebControl.Instance.GetMainFrame().ExecuteJavaScriptAsync("load('" + base64json + "');");
        }

        public static bool IsValid()
        {
            string resultString = (ModelWebControl.Instance.GetMainFrame().EvaluateScriptAsync("isvalid();", null)).Result.Result.ToString();
            return Convert.ToBoolean(resultString);
        }
    }

    public class AngularBinding
    {
        protected void OnClick(Action<object> function, object input)
        {
            Thread threadGetFile = new Thread(new ParameterizedThreadStart(ClickStart));
            threadGetFile.SetApartmentState(ApartmentState.STA);
            threadGetFile.Start(new ClickInput(function, input));
        }

        private void ClickStart(object input)
        {
            AngularBuilder.GetSettings();

            ((ClickInput)input).Function.Invoke(((ClickInput)input).Input);

            AngularBuilder.SetSettings();
        }

        private class ClickInput
        {
            public Action<object> Function { get; set; }
            public object Input { get; set; }

            public ClickInput(Action<object> function, object input)
            {
                Function = function;
                Input = input;
            }
        }
    }
}
