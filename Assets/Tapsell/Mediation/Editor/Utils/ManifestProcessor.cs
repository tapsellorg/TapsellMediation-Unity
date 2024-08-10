#if UNITY_ANDROID

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using UnityEditor;
using UnityEditor.Build;
using UnityEngine;
#if UNITY_2018_1_OR_NEWER
using UnityEditor.Build.Reporting;
#endif

namespace Tapsell.Mediation.Editor.Utils
{
#if UNITY_2018_1_OR_NEWER
    internal abstract class ManifestProcessor : IPreprocessBuildWithReport
#else
    internal abstract class ManifestProcessor : IPreprocessBuild
#endif
    {
        private protected abstract string PluginName();

        private string ManifestRelativePath()
        {
            return "Plugins/Android/AndroidManifest.xml";
        }
        private protected abstract string MetadataApplicationKey();
        private protected abstract string MetadataApplicationValue();
        private protected abstract bool ShouldValidateMetadataApplicationValue();

        public abstract int callbackOrder { get; }

        private readonly XNamespace _ns = "http://schemas.android.com/apk/res/android";

#if UNITY_2018_1_OR_NEWER
        public void OnPreprocessBuild(BuildReport report)
#else
        public void OnPreprocessBuild(BuildTarget target, string path)
#endif
        {
            var manifestPath = Path.Combine(Application.dataPath, ManifestRelativePath());
            XDocument manifest;
            try
            {
                manifest = XDocument.Load(manifestPath);
            }
#pragma warning disable 0168
            catch (IOException e)
#pragma warning restore 0168
            {
                StopBuildWithMessage("AndroidManifest.xml is missing. Try re-importing the plugin.");
                return;
            }

            var elemManifest = manifest.Element("manifest");
            if (elemManifest == null)
            {
                StopBuildWithMessage("AndroidManifest.xml is not valid. Try re-importing the plugin.");
                return;
            }

            var elemApplication = elemManifest.Element("application");
            if (elemApplication == null)
            {
                StopBuildWithMessage("AndroidManifest.xml is not valid. Try re-importing the plugin.");
                return;
            }

            var value = MetadataApplicationValue();

            if (value.Length == 0)
            {
                StopBuildWithMessage("Empty value received in settings. Please enter valid values to run ads properly.");
                return;
            }

            if (ShouldValidateMetadataApplicationValue() && !Regex.IsMatch(value, TapsellConstants.REGEX_STR_UUID))
            {
                StopBuildWithMessage("Incorrect AppKey received in settings. Please enter valid AppKey to run ads properly.");
                return;
            }

            var metas = elemApplication.Descendants()
                .Where( elem => elem.Name.LocalName.Equals("meta-data"));

            SetMetadataElement(elemApplication, metas, MetadataApplicationKey(), value);
            
            elemManifest.Save(manifestPath);
        }

        private XElement GetMetaElement(IEnumerable<XElement> metas, string metaName)
        {
            return (from elem in metas let attrs = elem.Attributes() where attrs.Any(attr => attr.Name.Namespace.Equals(_ns) && attr.Name.LocalName.Equals("name") && attr.Value.Equals(metaName)) select elem).FirstOrDefault();
        }

        /// <summary>
        /// Utility for setting a metadata element
        /// </summary>
        /// <param name="elemApplication">application element</param>
        /// <param name="metas">all metadata elements</param>
        /// <param name="metadataName">name of the element to set</param>
        /// <param name="metadataValue">value to set</param>
        private void SetMetadataElement(XElement elemApplication,
            IEnumerable<XElement> metas,
            string metadataName,
            string metadataValue)
        {
            var element = GetMetaElement(metas, metadataName);
            if (element == null)
            {
                elemApplication.Add(CreateMetaElement(metadataName, metadataValue));
            }
            else
            {
                element.SetAttributeValue(_ns + "value", metadataValue);
            }
        }

        private XElement CreateMetaElement(string name, object value)
        {
            return new XElement("meta-data",
                new XAttribute(_ns + "name", name), new XAttribute(_ns + "value", value));
        }

        private void StopBuildWithMessage(string message)
        {
            var prefix = "[" + PluginName() + "] ";
#if UNITY_2017_1_OR_NEWER
            throw new BuildPlayerWindow.BuildMethodException(prefix + message);
#else
            throw new OperationCanceledException(prefix + message);
#endif
        }

    }
}
#endif
