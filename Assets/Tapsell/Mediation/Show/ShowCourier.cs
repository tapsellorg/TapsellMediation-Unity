using System.Collections.Generic;
using Tapsell.Mediation.Show.Banner;
using Tapsell.Mediation.Show.Native;

namespace Tapsell.Mediation.Show
{
    internal class ShowCourier
    {
        private static ShowCourier _instance;
        private readonly Dictionary<string, IAdStateListener> _listeners = new();

        private static class NativeShowAPIs
        {
            public const string Rewarded = "showRewardedAd";
            public const string Interstitial = "showInterstitialAd";
            public const string BannerShow = "showBannerAd";
            public const string BannerDestroy = "destroyBannerAd";
            public const string NativeAdShow = "showNativeAd";
            public const string NativeAdDestroy = "destroyNativeAd";
        }

        internal static ShowCourier Get()
        {
            if (_instance != null) return _instance;

            _instance = new ShowCourier();
            return _instance;
        }
        
        internal void ShowRewardedAd(string adId, IAdStateListener.IRewarded listener)
        {
            _listeners[adId] = listener;
            MediatorAndroidCaller.Call(NativeShowAPIs.Rewarded, adId);
        }

        internal void ShowInterstitialAd(string adId, IAdStateListener.IInterstitial listener)
        {
            _listeners[adId] = listener;
            MediatorAndroidCaller.Call(NativeShowAPIs.Interstitial, adId);
        }

        internal void ShowBannerAd(string adId, BannerPosition position, IAdStateListener.IBanner listener)
        {
            _listeners[adId] = listener;
            MediatorAndroidCaller.Call(NativeShowAPIs.BannerShow, adId, (int) position);
        }

        internal void DestroyBannerAd(string adId)
        {
            _listeners.Remove(adId);
            MediatorAndroidCaller.Call(NativeShowAPIs.BannerDestroy, adId);
        }

        internal void ShowNativeAd(string adId, NativeAdView view, IAdStateListener.INative listener)
        {
            if (NativeAdViewHolder.Get().RegisterAdView(adId, view))
            {
                _listeners[adId] = listener;
                MediatorAndroidCaller.Call(NativeShowAPIs.NativeAdShow, adId);
            }
            else
            {
                listener.OnAdFailed("Registered GameObjects for native ad are invalid.");
            }
        }

        internal void DestroyNativeAd(string adId)
        {
            _listeners.Remove(adId);
            MediatorAndroidCaller.Call(NativeShowAPIs.NativeAdDestroy, adId);
        }

        // Called on ad impression event from the messenger
        internal void OnAdImpression(string adId)
        {
            _listeners[adId]?.OnAdImpression();
        }

        // Called on ad click event from the messenger
        internal void OnAdClicked(string adId)
        {
            _listeners[adId]?.OnAdClicked();
        }

        // Called on ad close event from the messenger
        internal void OnAdClosed(ShowCompletionMessage completionMessage)
        {
            (_listeners[completionMessage.adId] as IAdStateListener.IClosableAd)?
                .OnAdClosed(ShowCompletionStateHelper.FromString(completionMessage.completionState));
        }

        // Called on rewarded user event from the messenger
        internal void OnUserRewarded(string adId)
        {
            (_listeners[adId] as IAdStateListener.IRewarded)?.OnRewarded();
        }

        // Called on failed ad show event from the messenger
        internal void OnFailedShow(FailedShowResponse response)
        {
            _listeners[response.adId]?.OnAdFailed(response.message);
        }
    }
}