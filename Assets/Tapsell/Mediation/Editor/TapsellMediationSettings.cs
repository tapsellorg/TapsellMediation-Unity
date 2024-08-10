using System.IO;
using UnityEditor;
using UnityEngine;

namespace Tapsell.Mediation.Editor
{
    internal class TapsellMediationSettings : ScriptableObject
    {
        private const string TapsellMediationSettingsResDir = "Assets/Tapsell/Mediation/Resources";

        private const string TapsellMediationSettingsFile = "TapsellMediationSettings";

        private const string TapsellMediationSettingsFileExtension = ".asset";

        internal static TapsellMediationSettings LoadInstance()
        {
            // Read from resources.
            var instance = Resources.Load<TapsellMediationSettings>(TapsellMediationSettingsFile);

            if (instance != null) return instance;

            // Create instance if null.
            Directory.CreateDirectory(TapsellMediationSettingsResDir);
            instance = CreateInstance<TapsellMediationSettings>();
            var assetPath = Path.Combine(
                TapsellMediationSettingsResDir,
                TapsellMediationSettingsFile + TapsellMediationSettingsFileExtension);
            AssetDatabase.CreateAsset(instance, assetPath);
            AssetDatabase.SaveAssets();

            return instance;
        }

        [SerializeField]
        private string tapsellAndroidAppKey = string.Empty;

        public string TapsellAndroidAppKey
        {
            get => tapsellAndroidAppKey;

            set => tapsellAndroidAppKey = value;
        }
        
        [SerializeField]
        private string admobAdapterSignature = string.Empty;

        public string AdmobAdapterSignature
        {
            get => admobAdapterSignature;

            set => admobAdapterSignature = value;
        }

        [SerializeField]
        private string applovinAdapterSignature = string.Empty;

        public string ApplovinAdapterSignature
        {
            get => applovinAdapterSignature;

            set => applovinAdapterSignature = value;
        }
    }
}
