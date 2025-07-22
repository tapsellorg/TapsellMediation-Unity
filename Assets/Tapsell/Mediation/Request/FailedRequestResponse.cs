using System;

namespace Tapsell.Mediation.Request
{
    [Serializable]
    public class FailedRequestResponse
    {
        public string requestId;
        public string message;

        public FailedRequestResponse(string requestId, string message)
        {
            this.requestId = requestId;
            this.message = message;
        }
    }
}