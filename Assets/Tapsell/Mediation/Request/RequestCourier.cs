using System.Collections.Generic;
using Tapsell.Mediation.Utils;

namespace Tapsell.Mediation.Request
{
    internal class RequestCourier
    {
        private static RequestCourier _instance;
        private readonly Dictionary<string, IRequestListener> _listeners = new();

        private static class NativeRequestAPIs
        {
            public const string Rewarded = "requestRewardedAd";
            public const string Interstitial = "requestInterstitialAd";
            public const string Banner = "requestBannerAd";
            public const string Native = "requestNativeAd";
            public const string MultipleNative = "requestMultipleNativeAds";
            public const string Consent = "setUserConsent";
        }

        internal static RequestCourier Get()
        {
            if (_instance != null) return _instance;

            _instance = new RequestCourier();
            return _instance;
        }
        
        internal void RequestRewardedAd(string zoneId, IRequestListener listener)
        {
            var id = InitiateRequest(listener);
            MediatorAndroidCaller.Call(NativeRequestAPIs.Rewarded, id, zoneId);
        }

        internal void RequestInterstitialAd(string zoneId, IRequestListener listener)
        {
            var id = InitiateRequest(listener);
            MediatorAndroidCaller.Call(NativeRequestAPIs.Interstitial, id, zoneId);
        }

        internal void RequestBannerAd(string zoneId, BannerSize bannerSize, IRequestListener listener)
        {
            var id = InitiateRequest(listener);
            MediatorAndroidCaller.Call(NativeRequestAPIs.Banner, id, zoneId, (int) bannerSize);
        }

        internal void RequestNativeAd(string zoneId, IRequestListener listener)
        {
            var id = InitiateRequest(listener);
            MediatorAndroidCaller.Call(NativeRequestAPIs.Native, id, zoneId);
        }

        internal void RequestMultipleNativeAds(string zoneId, int maximumCount, IRequestListener listener)
        {
            var id = InitiateRequest(listener);
            MediatorAndroidCaller.Call(NativeRequestAPIs.MultipleNative, id, zoneId, maximumCount);
        }
        
        internal void SetUserConsent(bool consent)
        {
            MediatorAndroidCaller.Call(NativeRequestAPIs.Consent, consent);
        }

        // Called on successful request event from the messenger
        internal void OnSuccessfulRequest(RequestResponse response)
        {
            _listeners[response.requestId]?.OnSuccess(response.adId);
        }

        // Called on failed request event from the messenger
        internal void OnFailedRequest(string requestId)
        {
            _listeners[requestId]?.OnFailure();
        }

        private string InitiateRequest(IRequestListener listener)
        {
            var requestId = IdGenerator.GenerateId();
            _listeners[requestId] = listener;
            return requestId;
        }
    }
}