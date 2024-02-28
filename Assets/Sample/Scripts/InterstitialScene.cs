using UnityEngine;

public class InterstitialScene : MonoBehaviour
{
    private const string ZoneID = "313f90de-f6f0-4e37-a3b3-d9bacceba92f";
    private static string _adId = "";

    public void Request()
    {
        Tapsell.Mediation.Tapsell.RequestInterstitialAd(ZoneID,
            adId =>
            {
                Debug.Log("onInterstitialAd requestSuccess");
                _adId = adId;
            },
            () =>
            {
                Debug.Log("onInterstitialAd requestFailed");
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