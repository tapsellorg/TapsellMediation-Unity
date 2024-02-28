using Tapsell.Mediation.Messaging;
using UnityEngine;

namespace Tapsell.Mediation
{
    internal static class MediationInitializer
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void OnRuntimeInitialize()
        {
            AndroidMessageReceiver.RegisterReceiver<MediatorAndroidMessageListener>();
        }
    }
}