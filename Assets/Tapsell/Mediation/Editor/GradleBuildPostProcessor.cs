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
            var gradlePropertiesFile = path + "/../" + GradlePropertiesFile;
            if (!File.Exists(gradlePropertiesFile))
            {
                File.Create(gradlePropertiesFile);
            }

            var propertiesReader = File.OpenText(gradlePropertiesFile);
            var properties = PropertiesHelper.Load(propertiesReader);
            propertiesReader.Close();

            if (properties.ContainsKey(JetifierIgnorePropertyKey))
            {
                properties[JetifierIgnorePropertyKey] =
                    AddPackageIfNotPresent(properties[JetifierIgnorePropertyKey], MoshiPackage);
            }
            else properties[JetifierIgnorePropertyKey] = MoshiPackage;

            var writer = File.CreateText(gradlePropertiesFile);

            PropertiesHelper.Write(properties, writer);

            writer.Flush();
        }

        private static string AddPackageIfNotPresent(string currentValue, string newPackage)
        {
            var trimmedValue = currentValue.Trim();
            if (trimmedValue.Length == 0) return newPackage;

            if (trimmedValue.Split(",").Any(package => package.Trim().Equals(newPackage)))
            {
                return currentValue;
            }

            return currentValue + ", " + newPackage;
        }
    }
}