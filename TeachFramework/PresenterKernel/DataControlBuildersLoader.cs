using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

using TeachFramework.Interfaces;

namespace TeachFramework
{
    public static class DataControlBuildersLoader
    {
        public static IEnumerable<IDataControlBuilder> LoadControlBuilders(string absolutePath)
        {
            var controlBuilderType = Type.GetType("TeachFramework.Interfaces.IDataControlBuilder");
            if (controlBuilderType == null) return null;

            var controlBuilders = new List<IDataControlBuilder>();

            if (!Directory.Exists(absolutePath)) return controlBuilders;

            foreach (var file in Directory.GetFiles(absolutePath, "*.dll"))
            {
                var assembly = Assembly.LoadFile(file);
                foreach (var type in assembly.GetTypes())
                {
                    foreach (var typeInterface in type.GetInterfaces())
                    {
                        if (typeInterface != controlBuilderType) continue;
                        controlBuilders.Add((IDataControlBuilder)Activator.CreateInstance(type));
                        break;
                    }
                }
            }

            return controlBuilders;
        }
    }
}
