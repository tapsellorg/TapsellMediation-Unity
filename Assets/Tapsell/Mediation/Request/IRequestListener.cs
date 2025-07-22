using System;

namespace Tapsell.Mediation.Request
{
    public interface IRequestListener
    {
        public void OnSuccess(string adId);
        public void OnFailure(string message);
    }

    internal class RequestListenerImpl: IRequestListener
    {
        private readonly Action<string> _onSuccess;
        private readonly Action<string> _onFailure;

        public RequestListenerImpl(Action<string> onSuccess, Action<string> onFailure)
        {
            _onSuccess = onSuccess;
            _onFailure = onFailure;
        }
        
        public void OnSuccess(string adId)
        {
            _onSuccess(adId);
        }

        public void OnFailure(string message)
        {
            _onFailure(message);
        }
    }
}