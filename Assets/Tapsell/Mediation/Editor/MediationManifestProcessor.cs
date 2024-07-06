#if UNITY_ANDROID

using Tapsell.Mediation.Editor.Utils;

namespace Tapsell.Mediation.Editor
{
    internal class MediationManifestProcessor : ManifestProcessor
    {
        private protected override string PluginName()
        {
            return "TapsellMediation";
        }

        private protected override string ManifestRelativePath()
        {
#if UNITY_2021_2_OR_NEWER
            return "Plugins/Android/TapsellMediationPlugin/AndroidManifest.xml";
#else
            return "Plugins/Android/AndroidManifest.xml";       
#endif
        }

        private protected override string MetadataApplicationKey()
        {
            return "ir.tapsell.mediation.APPLICATION_KEY";
        }

        private protected override string MetadataApplicationValue()
        {
            return TapsellMediationSettings.LoadInstance().TapsellAndroidAppKey;
        }
        
        private protected override bool ShouldValidateMetadataApplicationValue()
        {
            return true;
        }
        
        private protected override string MetadataApplicationMarketKey()
        {
            return "ir.tapsell.mediation.APPLICATION_MARKET";
        }
        
        private protected override string MetadataApplicationMarketValue()
        {
            return TapsellMediationSettings.LoadInstance().TapsellAndroidAppMarketKey;
        }

        public override int callbackOrder => 0;
    }
}

#endif