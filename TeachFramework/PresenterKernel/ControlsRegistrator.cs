using System.Collections.Generic;
using TeachFramework.Exceptions;
using TeachFramework.Interfaces;

namespace TeachFramework
{
    public static class ControlsRegistrator
    {
        private static readonly List<string> Descriptions = new List<string>();

        public static readonly Dictionary<string, IDataControlBuilder> ControlsBuilders =
            new Dictionary<string, IDataControlBuilder>();

        public static void Registrate(IDataControlBuilder controlBuilder)
        {
            if (Descriptions.Contains(controlBuilder.Description))
                throw new ControlDescriptionDuplicateException("Control " + controlBuilder.Description + " has already exist");
            Descriptions.Add(controlBuilder.Description);
            ControlsBuilders.Add(controlBuilder.Description, controlBuilder);
        }
    }
}
