using Sample.Scripts;
using UnityEngine;

public class InterstitialScene : MonoBehaviour
{
    private const string ZoneID = TapsellMediationKeys.INTERSTITIAL;
    private static string _adId = "";

    public void Request()
    {
        Tapsell.Mediation.Tapsell.RequestInterstitialAd(ZoneID,
            adId =>
            {
                Debug.Log("onInterstitialAd requestSuccess");
                _adId = adId;
            },
            (error) =>
            {
                Debug.Log("onInterstitialAd requestFailed: " + error);
            }
        );
    }

    public void Show()
    {
        if (_adId != "")
        {
            Tapsell.Mediation.Tapsell.ShowInterstitialAd(_adId,
                () => { Debug.Log("onInterstitialAd impression"); },
                () => { Debug.Log("onInterstitialAd click"); },
                completionState => { Debug.Log("onInterstitialAd close: " + completionState); },
                message => { Debug.Log("onInterstitialAd showFailed: " + message); }
            );
        }
    }
}