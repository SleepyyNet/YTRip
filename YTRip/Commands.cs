using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace YTRip
{
    /// <summary>
    /// Custom commands
    /// </summary>
    public static class Commands
    {
        /// <summary>
        /// The download command
        /// </summary>
        public static readonly RoutedUICommand Download = new RoutedUICommand(
            "Download",
            "Download",
            typeof(Commands),
            new InputGestureCollection()
            {
                new KeyGesture(Key.D, ModifierKeys.Control)
            }
            );
    }
}
