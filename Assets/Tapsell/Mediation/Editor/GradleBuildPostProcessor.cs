using System.Collections.Generic;
using System.IO;
using System.Linq;
using Tapsell.Mediation.Editor.Utils;
using UnityEditor.Android;

namespace Tapsell.Mediation.Editor
{
    public class GradleBuildPostProcessor : IPostGenerateGradleAndroidProject
    {
        private const string GradlePropertiesFile = "gradle.properties";
        private const string JetifierIgnorePropertyKey = "android.jetifier.ignorelist";
        private const string MoshiPackage = "com.squareup.moshi";

        public int callbackOrder => 10;

        public void OnPostGenerateGradleAndroidProject(string path)
        {
            var gradlePropertiesFile = Path.GetFullPath(Path.Combine(path, "..", GradlePropertiesFile));

            IDictionary<string, string> properties;

            // Safely read the file if it already exists
            if (File.Exists(gradlePropertiesFile))
            {
                // The 'using' statement ensures the reader is closed automatically
                using (var propertiesReader = File.OpenText(gradlePropertiesFile))
                {
                    properties = PropertiesHelper.Load(propertiesReader);
                }
            }
            else
            {
                properties = new Dictionary<string, string>();
            }

            // Modify the properties in memory
            if (properties.ContainsKey(JetifierIgnorePropertyKey))
            {
                properties[JetifierIgnorePropertyKey] =
                    AddPackageIfNotPresent(properties[JetifierIgnorePropertyKey], MoshiPackage);
            }
            else
            {
                properties[JetifierIgnorePropertyKey] = MoshiPackage;
            }

            // Safely write the properties back.
            using (var writer = File.CreateText(gradlePropertiesFile))
            {
                PropertiesHelper.Write(properties, writer);
            }
        }

        private static string AddPackageIfNotPresent(string currentValue, string newPackage)
        {
            var trimmedValue = currentValue.Trim();
            if (trimmedValue.Length == 0) return newPackage;

            if (trimmedValue.Split(',').Any(package => package.Trim().Equals(newPackage)))
            {
                return currentValue;
            }

            return currentValue + ", " + newPackage;
        }
    }
}