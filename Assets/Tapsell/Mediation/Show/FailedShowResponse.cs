using System;

namespace Tapsell.Mediation.Show
{
    [Serializable]
    public class FailedShowResponse
    {
        public string adId;
        public string message;

        public FailedShowResponse(string adId, string message)
        {
            this.adId = adId;
            this.message = message;
        }
    }
}