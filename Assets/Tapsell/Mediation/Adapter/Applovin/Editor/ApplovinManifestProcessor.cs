#if UNITY_ANDROID

using Tapsell.Mediation.Editor;
using Tapsell.Mediation.Editor.Utils;

namespace Tapsell.Mediation.Adapter.Applovin.Editor
{
    internal class ApplovinManifestProcessor : ManifestProcessor
    {
        private protected override string PluginName()
        {
            return "TapsellMediationApplovinAdapter";
        }

        private protected override string MetadataApplicationKey()
        {
            return "applovin.sdk.key";
        }

        private protected override string MetadataApplicationValue()
        {
            return TapsellMediationSettings.LoadInstance().ApplovinAdapterSignature;
        }

        private protected override bool ShouldValidateMetadataApplicationValue()
        {
            return false;
        }

        public override int callbackOrder => 1;
    }
}

#endif