using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroSimSettings
{
    public class Constant
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsMultipleValue { get; set; }
        private double from;
        public double From
        {
            get { return from; }
            set
            {
                from = value;
                CurrentValue = from;
            }
        }
        public double Step { get; set; }
        public double To { get; set; }
        public double CurrentValue { get; set; }

        public Constant(string name, double from)
        {
            Name = name;
            IsMultipleValue = false;
            From = from;
            Step = 0;
            To = 0;
        }
    }
}
