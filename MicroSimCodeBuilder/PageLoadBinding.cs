using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MicroSimSettings;

namespace MicroSimCodeBuilder
{
    public class PageLoadBinding
    {
        public void OnPageLoaded()
        {
            ModelWebControl.Instance.IsReady = true;
        }
    }
}
