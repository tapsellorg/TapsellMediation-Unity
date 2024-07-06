using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Tapsell.Mediation.Show.Native
{
    internal class NativeAdViewHolder
    {
        private static NativeAdViewHolder _instance;

        private readonly Dictionary<string, NativeAdView> _adViews = new Dictionary<string, NativeAdView>();

        private NativeAdViewHolder() {}

        internal static NativeAdViewHolder Get()
        {
            if (_instance != null) return _instance;

            _instance = new NativeAdViewHolder();
            return _instance;
        }

        internal bool RegisterAdView(string adId, NativeAdView adView)
        {
            if (!ViewsAreValid(adView)) return false;

            _adViews[adId] = adView;
            return true;
        }

        internal NativeAdView GetNativeAdView(string adId)
        {
            return _adViews[adId];
        }

        private bool ViewsAreValid(NativeAdView views)
        {
            return ValidateText(views.AdvertiserText) && ValidateText(views.TitleText) &&
                   ValidateText(views.DescriptionText) && ValidateButton(views.CtaButton) &&
                   ValidateImage(views.BannerImage) && ValidateImage(views.IconImage);
        }

        private bool ValidateText(GameObject textObject)
        {
            if (textObject == null) return true;
            return HasCollider(textObject) &&
                   (textObject.GetComponent<Text>() != null ||
                    textObject.GetComponent<TMP_Text>() != null);
        }

        private bool ValidateButton(GameObject buttonObject)
        {
            if (buttonObject == null) return true;
            return HasCollider(buttonObject) &&
                   buttonObject.GetComponent<Button>() != null;
        }

        private bool ValidateImage(GameObject imageObject)
        {
            if (imageObject == null) return true;
            return HasCollider(imageObject) &&
                   (imageObject.GetComponent<Image>() != null ||
                    imageObject.GetComponent<RawImage>() != null);
        }

        private bool HasCollider(GameObject gameObject)
        {
            return gameObject.GetComponent<Collider>() != null;
        }
    }
}