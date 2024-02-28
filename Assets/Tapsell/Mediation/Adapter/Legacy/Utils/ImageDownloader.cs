using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;

namespace Tapsell.Mediation.Adapter.Legacy.Utils
{
    internal class ImageDownloader : MonoBehaviour
    {
        private string _url;

        private UnityAction<Texture2D> _onSuccessAction;
        private UnityAction<string> _onErrorAction;

        internal static ImageDownloader Get()
        {
            return new GameObject("TapsellLegacyImageDownloader").AddComponent<ImageDownloader>();
        }

        internal ImageDownloader OnSuccess(UnityAction<Texture2D> action)
        {
            _onSuccessAction = action;
            return this;
        }

        internal ImageDownloader OnError(UnityAction<string> action)
        {
            _onErrorAction = action;
            return this;
        }

        internal IEnumerator Load(string url)
        {
            _url = url;
            if (_url == null)
            {
                Error("Url has not been set.");
                yield return null;
            }

            try
            {
                var uri = new Uri(_url);
                _url = uri.AbsoluteUri;
            }
            catch (Exception)
            {
                Error("Url is not correct.");
            }

            yield return Downloader();
        }

        private IEnumerator Downloader()
        {
            var www = UnityWebRequestTexture.GetTexture(_url);
            www.certificateHandler = new CertificateWhore();
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Error("Error while downloading the image: " + www.error);
            }
            else
            {
                _onSuccessAction?.Invoke(DownloadHandlerTexture.GetContent(www));
            }

            www.Dispose();
        }

        private void Error(string message)
        {
            _onErrorAction?.Invoke("Tapsell legacy internal error in image downloader: " + message);
            Finish();
        }

        private void Finish()
        {
            Invoke(nameof(Destroyer), 0.5f);
        }

        private void Destroyer()
        {
            Destroy(gameObject);
        }
    }
}