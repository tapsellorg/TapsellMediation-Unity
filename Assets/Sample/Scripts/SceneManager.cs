using System;
using UnityEngine;

public class SceneManager : MonoBehaviour
{
    private void Start()
    {
        // send result of user consent dialog to Tapsell.
        Tapsell.Mediation.Tapsell.SetUserConsent(true);
    }

    public void ChangeScene(string sceneName)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
    }
}