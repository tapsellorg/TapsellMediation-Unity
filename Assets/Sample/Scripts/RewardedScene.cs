using UnityEngine;

public class RewardedScene : MonoBehaviour
{
    private const string ZoneID = "cce935f9-15a0-4cfa-98a6-3f3e3981bca0";
    private static string _adId;

    public void Request()
    {
        Tapsell.Mediation.Tapsell.RequestRewardedAd(ZoneID,
            adId =>
            {
                Debug.Log("onRewardedAd requestSuccess");
                _adId = adId;
            },
            () =>
            {
                Debug.Log("onRewardedAd requestFailed");
            }
        );
    }

    public void Show()
    {
        if (_adId != "")
        {
            Tapsell.Mediation.Tapsell.ShowRewardedAd(_adId,
                () => { Debug.Log("onRewardedAd impression"); },
                () => { Debug.Log("onRewardedAd click"); },
                completionState => { Debug.Log("onRewardedAd close: " + completionState); },
                message => { Debug.Log("onRewardedAd showFailed: " + message); },
                () => { Debug.Log("onRewardedAd rewarded"); }
            );
        }
    }
}