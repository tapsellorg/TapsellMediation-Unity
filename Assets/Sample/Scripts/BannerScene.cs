using Sample.Scripts;
using Tapsell.Mediation.Request;
using Tapsell.Mediation.Show.Banner;
using UnityEngine;

public class BannerScene : MonoBehaviour
{
    private const string ZoneID = TapsellMediationKeys.BANNER;
    private static string _adId;
    private static BannerPosition _position = BannerPosition.Top;
    private static BannerSize _size = BannerSize.Banner32050;

    public void Request()
    {
        Tapsell.Mediation.Tapsell.RequestBannerAd(ZoneID, _size,
            adId =>
            {
                Debug.Log("onBannerAd requestSuccess");
                _adId = adId;
            },
            () =>
            {
                Debug.Log("onBannerAd requestFailed");
            }
        );
    }

    public void Show()
    {
        if (_adId != "")
        {
            Tapsell.Mediation.Tapsell.ShowBannerAd(_adId, _position,
                () => { Debug.Log("onBannerAd impression"); },
                () => { Debug.Log("onBannerAd click"); },
                message => { Debug.Log("onBannerAd showFailed: " + message); }
            );
        }
    }

    public void Destroy()
    {
        if (_adId != "")
        {
            Tapsell.Mediation.Tapsell.DestroyBannerAd(_adId);
        }
    }

    public void OnPositionChanged(int newPosition)
    {
        _position = (BannerPosition)newPosition;
    }

    public void OnSizeChanged(int newSize)
    {
        _size = (BannerSize)newSize;
    }
}