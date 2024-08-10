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

        public override int callbackOrder => 0;
    }
}

#endif