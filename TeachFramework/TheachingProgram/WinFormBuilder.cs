using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

using TeachFramework.Exceptions;
using TeachFramework.Interfaces;

namespace TeachFramework
{
    /// <summary>
    /// WinForm view builder
    /// </summary>
    public class WinFormBuilder : IViewBuilder
    {
        private int _controlY = 5;
        private const int Space = 5;
        private readonly string _pathToControlBuilders;
        //--------------------------------------------------
        /// <summary>
        /// Represents WinForm view builder
        /// </summary>
        public WinFormBuilder(string pathToControlBuilders)
        {
            Form = new WinAppForm();
            _pathToControlBuilders = pathToControlBuilders;
            // Load
            // Registrate
            RegistrateControlsBuilders();
        }

        /// <summary>
        /// Gets view
        /// </summary>
        public WinAppForm Form { get; private set; }

        #region IViewBuilder Members

        /// <summary>
        /// Builds view according to user interface description
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public IView Build(IEnumerable<UiDescriptionItem> data)
        {
            SetControls(data);
            return Form;
        }

        #endregion

        private void RegistrateControlsBuilders()
        {
            // should be similar names
            var controlBuilders = /**/DataControlBuildersLoader.LoadControlBuilders(_pathToControlBuilders);

            foreach (var controlBuilder in controlBuilders)
            {
                if(!/**/ControlsRegistrator.ControlsBuilders.ContainsKey(controlBuilder.Description))
                    ControlsRegistrator.Registrate(controlBuilder);
            }
        }

        /// <summary>
        /// Adds controls to form
        /// </summary>
        /// <param name="controlData">Controls options</param>
        private void SetControls(IEnumerable<UiDescriptionItem> controlData)
        {
            var notExistingControls = new List<string>();
            Form.ClearControlsList();
            _controlY = 5;
            if (controlData == null) return;
            foreach (var data in controlData)
            {
                try
                {
                    var control = ControlsRegistrator.ControlsBuilders[data.ControlType].Create(data);
                    Form.AddControl(control);
                    ((Control)control).Location = new Point(10, _controlY);
                    _controlY += ((Control)control).Height + Space;
                }
                catch (KeyNotFoundException)
                {
                    if (!notExistingControls.Contains(data.ControlType))
                        notExistingControls.Add(data.ControlType);
                }
                catch(InvalidCastException)
                {
                    throw new InvalidControlDataValueException(data.Name);
                }
            }
            if (notExistingControls.Count != 0)
                throw new NotExistingControlException(notExistingControls);
        }
    }
}
