#if UNITY_ANDROID

using Tapsell.Mediation.Messaging;
using Tapsell.Mediation.Request;
using Tapsell.Mediation.Show;
using UnityEngine;

namespace Tapsell.Mediation
{
    internal class MediatorAndroidMessageListener : TapsellAndroidMessageListener
    {
        public void OnSuccessfulRequest(string response)
        {
            RequestCourier.Get()
                .OnSuccessfulRequest(JsonUtility.FromJson<RequestResponse>(response));
        }

        public void OnFailedRequest(string response)
        {
            RequestCourier.Get().OnFailedRequest(JsonUtility.FromJson<FailedRequestResponse>(response));
        }

        public void OnUserRewarded(string adId)
        {
            ShowCourier.Get().OnUserRewarded(adId);
        }

        public void OnAdClicked(string adId)
        {
            ShowCourier.Get().OnAdClicked(adId);
        }

        public void OnAdClosed(string response)
        {
            ShowCourier.Get().OnAdClosed(JsonUtility.FromJson<ShowCompletionMessage>(response));
        }

        public void OnAdImpression(string adId)
        {
            ShowCourier.Get().OnAdImpression(adId);
        }

        public void OnFailedShow(string response)
        {
            ShowCourier.Get().OnFailedShow(JsonUtility.FromJson<FailedShowResponse>(response));
        }
    }
}

#endif