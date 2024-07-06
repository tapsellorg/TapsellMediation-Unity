using Tapsell.Mediation.Messaging;
using UnityEngine;

namespace Tapsell.Mediation.Adapter.Admob
{
    internal static class AdmobInitializer
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void OnRuntimeInitialize()
        {
            AndroidMessageReceiver.RegisterReceiver<AdmobAndroidMessageListener>();
        }
    }
}