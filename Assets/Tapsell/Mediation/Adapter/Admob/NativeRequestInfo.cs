using System;

namespace Tapsell.Mediation.Adapter.Admob
{
    [Serializable]
    public class NativeRequestInfo
    {
        public string requestId;
        public string zoneId;

        public NativeRequestInfo(string requestId, string zoneId)
        {
            this.requestId = requestId;
            this.zoneId = zoneId;
        }
    }
}