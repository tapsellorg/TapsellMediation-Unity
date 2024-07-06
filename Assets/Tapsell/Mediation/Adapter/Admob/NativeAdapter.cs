using System.Collections.Generic;
using GoogleMobileAds.Api;
using Tapsell.Mediation.Show.Native;
using Tapsell.Mediation.Utils;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Tapsell.Mediation.Adapter.Admob
{
    internal class NativeAdapter
    {
        private static NativeAdapter _instance;

        private readonly Dictionary<string, NativeAd> _ads = new Dictionary<string, NativeAd>();

        // Since Admob impression callback is not always called, we call the listener in multiple triggers.
        // This List is used to prevent multiple calls.
        private readonly List<string> _impressionCalled = new List<string>();

        // Since Admob click callback is not always called, we call the listener in both open and click triggers.
        // For each filled ad there can be only one click source activated.
        private readonly Dictionary<string, ClickCallbackSource> _clickCallSources = new Dictionary<string, ClickCallbackSource>();

        private static class AndroidAPIs
        {
            public const string LoadSuccess = "nativeRequestSucceeded";
            public const string LoadFailure = "nativeRequestFailed";

            public const string ShowFailure = "onNativeAdImpressionFailure";
            public const string Click = "onNativeAdClick";
            public const string Impression = "onNativeAdImpression";
            // not currently provided by Admob in Unity
            public const string Revenue = "onNativeAdPaidEvent";
        }

        internal static NativeAdapter Get()
        {
            if (_instance != null) return _instance;

            _instance = new NativeAdapter();
            return _instance;
        }

        internal void RequestNativeAd(NativeRequestInfo info)
        {
            var adLoader = new AdLoader.Builder(info.zoneId)
                .ForNativeAd()
                .Build();

            adLoader.OnNativeAdLoaded += (_, args) => OnAdLoadSuccess(info.requestId, args.nativeAd);
            adLoader.OnAdFailedToLoad += (_, args) => OnAdLoadFailure(info.requestId, args);

            adLoader.OnNativeAdClicked += (_, args) => OnAdClick(info.requestId);
            adLoader.OnNativeAdImpression += (_, args) => OnAdImpression(info.requestId);
            adLoader.OnNativeAdOpening += (_, args) => OnAdOpened(info.requestId);

            adLoader.LoadAd(new AdRequest());
        }

        internal void ShowNativeAd(string requestId, NativeAdView view)
        {
            // If any object is successfully registered to Admob, ad impression is triggered manually
            var anyObjectRegistered = false;

            var ad = _ads[requestId];
            if (ad == null)
            {
                OnAdImpressionFailure(requestId, "Internal Error: No ad was not found for the provided id.");
                return;
            }

            var iconTexture = ad.GetIconTexture();
            if (iconTexture != null && view.IconImage != null)
            {
                ViewHelper.SetImage(iconTexture, view.IconImage);
                if (ad.RegisterIconImageGameObject(view.IconImage)) anyObjectRegistered = true;
            }

            var adChoicesTexture = ad.GetAdChoicesLogoTexture();
            if (adChoicesTexture != null && view.AdChoicesImage != null)
            {
                ViewHelper.SetImage(adChoicesTexture, view.AdChoicesImage);
                if (ad.RegisterAdChoicesLogoGameObject(view.AdChoicesImage)) anyObjectRegistered = true;
            }

            if (ad.GetImageTextures().Count > 0 && view.BannerImage != null)
            {
                var bannerTexture = ad.GetImageTextures()[0];
                ViewHelper.SetImage(bannerTexture, view.BannerImage);
                if (ad.RegisterImageGameObjects(new List<GameObject> { view.BannerImage }) >= 1) anyObjectRegistered = true;
            }

            var title = ad.GetHeadlineText();
            if (title != null && view.TitleText != null)
            {
                ViewHelper.SetText(title, view.TitleText);
                if (ad.RegisterHeadlineTextGameObject(view.TitleText)) anyObjectRegistered = true;
            }

            var description = ad.GetBodyText();
            if (description != null && view.DescriptionText != null)
            {
                ViewHelper.SetText(description, view.DescriptionText);
                if (ad.RegisterBodyTextGameObject(view.DescriptionText)) anyObjectRegistered = true;
            }

            var advertiser = ad.GetAdvertiserText();
            if (advertiser != null && view.AdvertiserText != null)
            {
                ViewHelper.SetText(advertiser, view.AdvertiserText);
                if (ad.RegisterAdvertiserTextGameObject(view.AdvertiserText)) anyObjectRegistered = true;
            }

            var ctaText = ad.GetCallToActionText();
            if (ctaText != null && view.CtaButton != null)
            {
                ViewHelper.SetButtonText(ctaText, view.CtaButton);
                if (ad.RegisterCallToActionGameObject(view.CtaButton)) anyObjectRegistered = true;
            }

            if (anyObjectRegistered) OnAdImpression(requestId);
        }

        internal void DestroyNativeAd(string requestId)
        {
            _ads[requestId].Destroy();
        }

        private void OnAdLoadSuccess(string requestId, NativeAd ad)
        {
            _ads[requestId] = ad;
            AdmobAndroidCaller.Call(AndroidAPIs.LoadSuccess, requestId);
        }

        private void OnAdLoadFailure(string requestId, AdFailedToLoadEventArgs args)
        {
            AdmobAndroidCaller.Call(AndroidAPIs.LoadFailure, requestId, args.LoadAdError.ToString());
        }

        private void OnAdImpression(string requestId)
        {
            if (_impressionCalled.Contains(requestId)) return;
            AdmobAndroidCaller.Call(AndroidAPIs.Impression, requestId);
            _impressionCalled.Add(requestId);
        }

        private void OnAdOpened(string requestId)
        {
            OnAdClick(requestId, ClickCallbackSource.Open);
        }

        private void OnAdImpressionFailure(string requestId, string message)
        {
            AdmobAndroidCaller.Call(AndroidAPIs.ShowFailure, requestId, message);
        }

        private void OnAdClick(string requestId, ClickCallbackSource source = ClickCallbackSource.Click)
        {
            if (ShouldCallClickListener(requestId, source))
                AdmobAndroidCaller.Call(AndroidAPIs.Click, requestId);
        }

        private bool ShouldCallClickListener(string requestId,
            ClickCallbackSource source = ClickCallbackSource.Click)
        {
            if (!_clickCallSources.ContainsKey(requestId))
            {
                _clickCallSources[requestId] = source;
                return true;
            }

            return _clickCallSources[requestId] == source;
        }
    }

    enum ClickCallbackSource
    {
        Click,
        Open
    }
}