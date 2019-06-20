using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MicroSim.DataSource.Entities
{
    /// <summary>
    /// InvokeExtensions
    /// </summary>
    public static class InvokeExtensions
    {
        /// <summary>
        /// Invokes the action.
        /// </summary>
        /// <param name="parent">The parent.</param>
        /// <param name="action">The action.</param>
        public static void InvokeAction(Control parent, Action action)
        { 
            if (parent.IsHandleCreated)
                parent.BeginInvoke(action);
            else action.Invoke();
        }
    }
}
