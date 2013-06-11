using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

using TeachFramework.Interfaces;

namespace TeachFramework
{
    /// <summary>
    /// Static class for model loading
    /// </summary>
    public static class ModelsLoader
    {
        /// <summary>
        /// Loads all models from directory by path
        /// </summary>
        public static IEnumerable<IModel> LoadModels(string absolutePath)
        {
            var modelType = Type.GetType("TeachFramework.Interfaces.IModel");
            if (modelType == null) return null;

            var models = new List<IModel>();

            if (!Directory.Exists(absolutePath)) return models;

            foreach (var file in Directory.GetFiles(absolutePath, "*.dll"))
            {
                var assembly = Assembly.LoadFile(file);
                foreach (var type in assembly.GetTypes())
                {
                    foreach (var typeInterface in type.GetInterfaces())
                    {
                        if (typeInterface != modelType) continue;
                        models.Add((IModel)Activator.CreateInstance(type));
                        break;
                    }
                }
            }

            return models;
        }
    }
}
