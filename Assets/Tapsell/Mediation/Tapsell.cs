using System;
using System.Text.RegularExpressions;
using Tapsell.Mediation.Request;
using Tapsell.Mediation.Show;
using Tapsell.Mediation.Show.Banner;
using Tapsell.Mediation.Show.Native;
using UnityEngine;

namespace Tapsell.Mediation
{
    public static class Tapsell
    {
        public static void RequestRewardedAd(string zoneId, Action<string> onSuccess, Action onFailure)
        {
            RequestRewardedAd(zoneId, new RequestListenerImpl(onSuccess, onFailure));
        }

        public static void RequestRewardedAd(string zoneId, IRequestListener listener)
#if UNITY_ANDROID
        {
            if (IsZoneIdValid(zoneId) && listener != null) RequestCourier.Get().RequestRewardedAd(zoneId, listener);
            else LogParameterError("rewarded ad request");
        }
#else
        {}
#endif

        public static void RequestInterstitialAd(string zoneId, Action<string> onSuccess, Action onFailure)
        {
            RequestInterstitialAd(zoneId, new RequestListenerImpl(onSuccess, onFailure));
        }

        public static void RequestInterstitialAd(string zoneId, IRequestListener listener)
#if UNITY_ANDROID
        {
            if (IsZoneIdValid(zoneId) && listener != null) RequestCourier.Get().RequestInterstitialAd(zoneId, listener);
            else LogParameterError("interstitial ad request");
        }
#else
        {}
#endif

        public static void RequestBannerAd(string zoneId, Action<string> onSuccess, Action onFailure)
        {
            RequestBannerAd(zoneId, new RequestListenerImpl(onSuccess, onFailure));
        }

        public static void RequestBannerAd(string zoneId, BannerSize bannerSize, Action<string> onSuccess, Action onFailure)
        {
            RequestBannerAd(zoneId, bannerSize, new RequestListenerImpl(onSuccess, onFailure));
        }

        public static void RequestBannerAd(string zoneId, IRequestListener listener)
        {
            RequestBannerAd(zoneId, BannerSize.Banner32050, listener);
        }

        public static void RequestBannerAd(string zoneId, BannerSize bannerSize, IRequestListener listener)
#if UNITY_ANDROID
        {
            if (IsZoneIdValid(zoneId) && listener != null) RequestCourier.Get().RequestBannerAd(zoneId, bannerSize, listener);
            else LogParameterError("banner ad request");
        }
#else
        {}
#endif

        public static void RequestNativeAd(string zoneId, Action<string> onSuccess, Action onFailure)
        {
            RequestNativeAd(zoneId, new RequestListenerImpl(onSuccess, onFailure));
        }

        public static void RequestNativeAd(string zoneId, IRequestListener listener)
        {
            if (IsZoneIdValid(zoneId) && listener != null) RequestCourier.Get().RequestNativeAd(zoneId, listener);
            else LogParameterError("native ad request");
        }

        public static void RequestMultipleNativeAds(string zoneId, int maximumCount, Action<string> onSuccess, Action onFailure)
        {
            RequestMultipleNativeAds(zoneId, maximumCount, new RequestListenerImpl(onSuccess, onFailure));
        }

        public static void RequestMultipleNativeAds(string zoneId, int maximumCount, IRequestListener listener)
        {
            if (IsZoneIdValid(zoneId) && listener != null && maximumCount > 1) RequestCourier.Get().RequestMultipleNativeAds(zoneId, maximumCount, listener);
            else LogParameterError("multiple native ads request");
        }

        public static void ShowRewardedAd(string adId, Action onImpression = null, Action onClicked = null, Action<ShowCompletionState> onClosed = null, Action<string> onFailed = null, Action onRewarded = null)
        {
            ShowRewardedAd(adId, new RewardedAdStateListenerImpl(onImpression, onClicked, onClosed, onFailed, onRewarded));
        }

        public static void ShowRewardedAd(string adId, IAdStateListener.IRewarded listener)
#if UNITY_ANDROID
        {
            if (adId != null && listener != null) ShowCourier.Get().ShowRewardedAd(adId, listener);
            else LogParameterError("rewarded ad show");
        }
#else
        {}
#endif

        public static void ShowInterstitialAd(string adId, Action onImpression = null, Action onClicked = null, Action<ShowCompletionState> onClosed = null, Action<string> onFailed = null)
        {
            ShowInterstitialAd(adId, new InterstitialAdStateListenerImpl(onImpression, onClicked, onClosed, onFailed));
        }

        public static void ShowInterstitialAd(string adId, IAdStateListener.IInterstitial listener)
#if UNITY_ANDROID
        {
            if (adId != null && listener != null) ShowCourier.Get().ShowInterstitialAd(adId, listener);
            else LogParameterError("interstitial ad show");
        }
#else
        {}
#endif

        public static void ShowBannerAd(string adId, BannerPosition position, Action onImpression = null, Action onClicked = null, Action<string> onFailed = null)
        {
            ShowBannerAd(adId, position, new BannerAdStateListenerImpl(onImpression, onClicked, onFailed));
        }

        public static void ShowBannerAd(string adId, BannerPosition position, IAdStateListener.IBanner listener)
#if UNITY_ANDROID
        {
            if (adId != null && listener != null) ShowCourier.Get().ShowBannerAd(adId, position, listener);
            else LogParameterError("banner ad show");
        }
#else
        {}
#endif

        public static void DestroyBannerAd(string adId)
#if UNITY_ANDROID
        {
            if (adId != null) ShowCourier.Get().DestroyBannerAd(adId);
            else LogParameterError("banner ad destroy");
        }
#else
        {}
#endif

        public static void ShowNativeAd(string adId, NativeAdView view, Action onImpression = null, Action onClicked = null, Action<string> onFailed = null)
        {
            ShowNativeAd(adId, view, new NativeAdStateListenerImpl(onImpression, onClicked, onFailed));
        }

        public static void ShowNativeAd(string adId, NativeAdView view, IAdStateListener.INative listener)
#if UNITY_ANDROID
        {
            if (adId != null && view != null && listener != null) ShowCourier.Get().ShowNativeAd(adId, view, listener);
            else LogParameterError("native ad show");
        }
#else
        {}
#endif

        public static void DestroyNativeAd(string adId)
#if UNITY_ANDROID
        {
            if (adId != null) ShowCourier.Get().DestroyNativeAd(adId);
            else LogParameterError("native ad destroy");
        }
#else
        {}
#endif
        
        public static void SetUserConsent(bool consent)
#if UNITY_ANDROID
        {
            RequestCourier.Get().SetUserConsent(consent);
        }
#else
        {}
#endif

        private static void LogParameterError(string api)
        {
            Debug.Log("[Tapsell]: Invalid parameters were received for " + api + " api. The call will be ignored.");
        }
        
        private static void LogZoneIdError(String zoneId) {
            Debug.Log("[Tapsell]: Invalid Zone Id were received for " + zoneId);
        }

        private static Boolean IsZoneIdValid(String zoneId)
        {
            Boolean isMatch = Regex.IsMatch(zoneId, TapsellConstants.REGEX_STR_UUID);
            if (isMatch) return true;
            LogZoneIdError(zoneId);
            return false;
        }
    }
}
