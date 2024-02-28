using System;
using System.Collections;
using Tapsell.Mediation.Adapter.Legacy.Utils;
using Tapsell.Mediation.Show.Native;
using Tapsell.Mediation.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace Tapsell.Mediation.Adapter.Legacy
{
    internal class NativeAdapter
    {
        private static NativeAdapter _instance;

        private static class AndroidAPIs
        {
            public const string ShowFailure = "onNativeAdImpressionFailure";
            public const string Click = "onNativeAdClick";
            public const string Impression = "onNativeAdImpression";
        }

        internal static NativeAdapter Get()
        {
            if (_instance != null) return _instance;

            _instance = new NativeAdapter();
            return _instance;
        }

        internal void ShowNativeAd(string requestId, NativeAdContent content, NativeAdView view)
        {
            GetLoader().LoadAd(requestId, content, view);
        }

        internal void DestroyNativeAd(string requestId) {}

        private static NativeAdLoader GetLoader()
        {
            return new GameObject("TapsellLegacyNativeAdLoader").AddComponent<NativeAdLoader>();
        }

        private class NativeAdLoader : MonoBehaviour
        {
            internal void LoadAd(string requestId, NativeAdContent content, NativeAdView view)
            {
                StartCoroutine(PopulateAd(requestId, content, view));
            }

            private static IEnumerator PopulateAd(string requestId, NativeAdContent content, NativeAdView view)
            {
                var failed = false;

                Texture2D bannerTexture = null;

                Texture2D iconTexture = null;
                if (content.iconUrl != null && !content.iconUrl.Equals(""))
                {
                    yield return ImageDownloader.Get()
                        .OnSuccess(icon => iconTexture = icon)
                        .OnError(message =>
                        {
                            failed = true;
                            OnAdImpressionFailure(requestId, message);
                        })
                        .Load(content.iconUrl);
                }

                if (failed) yield break;

                if (content.landscapeImageUrl != null && !content.landscapeImageUrl.Equals(""))
                {
                    yield return ImageDownloader.Get()
                        .OnSuccess(banner => bannerTexture = banner)
                        .OnError(message =>
                        {
                            failed = true;
                            OnAdImpressionFailure(requestId, message);
                        })
                        .Load(content.landscapeImageUrl);
                }

                if (failed) yield break;

                // If any object is successfully registered, ad impression is triggered
                var anyObjectRegistered = false;

                if (iconTexture != null && view.IconImage != null)
                {
                    ViewHelper.SetImage(iconTexture, view.IconImage);
                    RegisterGameObject(requestId, view.IconImage);
                    anyObjectRegistered = true;
                }

                if (bannerTexture != null && view.BannerImage != null)
                {
                    ViewHelper.SetImage(bannerTexture, view.BannerImage);
                    RegisterGameObject(requestId, view.BannerImage);
                    anyObjectRegistered = true;
                }

                if (content.title != null && view.TitleText != null)
                {
                    ViewHelper.SetText(content.title, view.TitleText);
                    RegisterGameObject(requestId, view.TitleText);
                    anyObjectRegistered = true;
                }

                if (content.description != null && view.DescriptionText != null)
                {
                    ViewHelper.SetText(content.description, view.DescriptionText);
                    RegisterGameObject(requestId, view.DescriptionText);
                    anyObjectRegistered = true;
                }

                if (content.callToActionText != null && view.CtaButton != null)
                {
                    ViewHelper.SetButtonText(content.callToActionText, view.CtaButton);
                    RegisterGameObject(requestId, view.CtaButton);
                    anyObjectRegistered = true;
                }

                if (anyObjectRegistered) OnAdImpression(requestId);
            }

            private static void RegisterGameObject(string requestId, GameObject gameObject)
            {
                if (gameObject.transform as RectTransform)
                {
                    var component = gameObject.GetComponent<Button>();

                    if (component == null) component = gameObject.AddComponent<Button>();

                    component.onClick.RemoveAllListeners();
                    component.onClick.AddListener(() => OnAdClick(requestId));
                }
                else
                {
                    var collider = gameObject.GetComponent<Collider>();
                    collider.gameObject.AddComponent<ClickHandler>().OnClick += () => OnAdClick(requestId);
                }
            }

            private static void OnAdImpression(string requestId)
            {
                LegacyAndroidCaller.Call(AndroidAPIs.Impression, requestId);
            }

            private static void OnAdImpressionFailure(string requestId, string message)
            {
                LegacyAndroidCaller.Call(AndroidAPIs.ShowFailure, requestId, message);
            }

            private static void OnAdClick(string requestId)
            {
                LegacyAndroidCaller.Call(AndroidAPIs.Click, requestId);
            }

            public class ClickHandler : MonoBehaviour
            {
                private void OnMouseUpAsButton()
                {
                    OnClick?.Invoke();
                }

                public event Action OnClick;
            }
        }
    }
}