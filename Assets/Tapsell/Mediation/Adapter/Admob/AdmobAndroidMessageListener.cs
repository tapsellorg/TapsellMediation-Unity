using Tapsell.Mediation.Messaging;
using Tapsell.Mediation.Show.Native;
using UnityEngine;

namespace Tapsell.Mediation.Adapter.Admob
{
    internal class AdmobAndroidMessageListener : TapsellAndroidMessageListener
    {
        public void OnAdmobNativeAdRequest(string response)
        {
            NativeAdapter.Get()
                .RequestNativeAd(JsonUtility.FromJson<NativeRequestInfo>(response));
        }

        public void OnAdmobNativeAdShow(string requestId)
        {
            NativeAdapter.Get().ShowNativeAd(requestId, NativeAdViewHolder.Get().GetNativeAdView(requestId));
        }

        public void OnAdmobNativeAdDestroy(string requestId)
        {
            NativeAdapter.Get().DestroyNativeAd(requestId);
        }
    }
}