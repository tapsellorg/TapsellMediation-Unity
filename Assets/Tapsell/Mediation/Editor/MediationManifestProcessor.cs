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
            return "Plugins/Android/TapsellMediationPlugin.androidlib/AndroidManifest.xml";
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