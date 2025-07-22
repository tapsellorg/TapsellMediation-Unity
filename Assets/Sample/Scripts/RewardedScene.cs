using Sample.Scripts;
using UnityEngine;

public class RewardedScene : MonoBehaviour
{
    private const string ZoneID = TapsellMediationKeys.REWARDED;
    private static string _adId;

    public void Request()
    {
        Tapsell.Mediation.Tapsell.RequestRewardedAd(ZoneID,
            adId =>
            {
                Debug.Log("onRewardedAd requestSuccess");
                _adId = adId;
            },
            (error) =>
            {
                Debug.Log("onRewardedAd requestFailed: " + error);
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