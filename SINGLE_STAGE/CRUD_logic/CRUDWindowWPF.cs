using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SINGLE_STAGE;
using System.Windows.Controls;
using System.ComponentModel;
using System.Windows;

namespace SINGLE_STAGE.CRUD_logic
{
    public class CRUDWindowWPF
    {
        public static void ReturnToMainWindowAndClose(Window thisWindow)
        {
            // for "Back" buttons:
            // returns to the main dashboard and closes the window that is currently open
            MainWindow main = new();
            main.Show();
            thisWindow.Close();
        }
    }
}
