using System;

namespace Tapsell.Mediation.Adapter.Legacy
{
    [Serializable]
    public class NativeAdInfo
    {
        public string requestId;
        public NativeAdContent content;

        public NativeAdInfo(string requestId, NativeAdContent content)
        {
            this.requestId = requestId;
            this.content = content;
        }
    }

    [Serializable]
    public class NativeAdContent
    {
        public string adId;
        public string title;
        public string description;
        public string iconUrl;
        public string callToActionText;
        public string portraitImageUrl;
        public string landscapeImageUrl;

        public NativeAdContent(string adId, string title, string description, string iconUrl, string callToActionText, string portraitImageUrl, string landscapeImageUrl)
        {
            this.adId = adId;
            this.title = title;
            this.description = description;
            this.iconUrl = iconUrl;
            this.callToActionText = callToActionText;
            this.portraitImageUrl = portraitImageUrl;
            this.landscapeImageUrl = landscapeImageUrl;
        }
    }
}