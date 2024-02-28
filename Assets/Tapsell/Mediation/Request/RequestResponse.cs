using System;

namespace Tapsell.Mediation.Request
{
    [Serializable]
    public class RequestResponse
    {
        public string requestId;
        public string adId;

        public RequestResponse(string requestId, string adId)
        {
            this.requestId = requestId;
            this.adId = adId;
        }
    }
}