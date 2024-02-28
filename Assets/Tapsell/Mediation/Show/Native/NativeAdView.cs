using UnityEngine;

namespace Tapsell.Mediation.Show.Native
{
    public class NativeAdView {
        public GameObject AdvertiserText { get; private set; }
        public GameObject CtaButton { get; private set; }
        public GameObject TitleText { get; private set; }
        public GameObject IconImage { get; private set; }
        public GameObject DescriptionText { get; private set; }
        public GameObject BannerImage { get; private set; }
        public GameObject AdChoicesImage { get; private set; }

        private NativeAdView() {}

        public class Builder
        {
            private readonly NativeAdView _adView;

            public Builder()
            {
                _adView = new NativeAdView();
            }

            public Builder WithAdvertiserText(GameObject text)
            {
                _adView.AdvertiserText = text;
                return this;
            }

            public Builder WithCtaButton(GameObject button)
            {
                _adView.CtaButton = button;
                return this;
            }

            public Builder WithTitleText(GameObject text)
            {
                _adView.TitleText = text;
                return this;
            }

            public Builder WithIconImage(GameObject image)
            {
                _adView.IconImage = image;
                return this;
            }

            public Builder WithDescriptionText(GameObject text)
            {
                _adView.DescriptionText = text;
                return this;
            }

            public Builder WithBannerImage(GameObject image)
            {
                _adView.BannerImage = image;
                return this;
            }

            public Builder WithAdChoicesImage(GameObject image)
            {
                _adView.AdChoicesImage = image;
                return this;
            }

            public NativeAdView Build()
            {
                return _adView;
            }
        }
    }
}