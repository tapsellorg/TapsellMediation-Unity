using Tapsell.Mediation.Messaging;
using Tapsell.Mediation.Show.Native;
using UnityEngine;

namespace Tapsell.Mediation.Adapter.Legacy
{
    internal class LegacyAndroidMessageListener : TapsellAndroidMessageListener
    {
        public void OnLegacyNativeAdShow(string response)
        {
            var info = JsonUtility.FromJson<NativeAdInfo>(response);
            NativeAdapter.Get().ShowNativeAd(info.requestId, info.content, NativeAdViewHolder.Get().GetNativeAdView(info.requestId));
        }

        public void OnLegacyNativeAdDestroy(string requestId)
        {
            NativeAdapter.Get().DestroyNativeAd(requestId);
        }
    }
}