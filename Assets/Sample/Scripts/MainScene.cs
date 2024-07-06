using System;
using UnityEngine;

public class MainScene : MonoBehaviour
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    private static void OnRuntimeInitialize()
    {
        // send result of user consent dialog to Tapsell.
        SetUserConsent(); 
    }

    private static void SetUserConsent()
    {
        Tapsell.Mediation.Tapsell.SetUserConsent(true);
    }
}