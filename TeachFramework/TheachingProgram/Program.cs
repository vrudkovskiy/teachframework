using System;
using System.Windows.Forms;
using TeachFramework.Exceptions;

namespace TeachFramework
{
    static class Program
    {

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            var viewBuilder = new WinFormBuilder(Application.StartupPath + "\\WinForm controls");

            var models = ModelsLoader.LoadModels(Application.StartupPath + "\\Teaching programs");
            
            try
            {
                new SolvingPresenter(viewBuilder, models);
            }
            catch (NotExistingControlException exception)
            {
                viewBuilder.Form.ShowMessage(exception.Message);
                return;
            }

            Application.Run(viewBuilder.Form);
        }
    }
}
