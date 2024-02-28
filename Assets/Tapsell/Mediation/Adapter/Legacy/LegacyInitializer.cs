using Tapsell.Mediation.Messaging;
using UnityEngine;

namespace Tapsell.Mediation.Adapter.Legacy
{
    internal static class LegacyInitializer
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void OnRuntimeInitialize()
        {
            AndroidMessageReceiver.RegisterReceiver<LegacyAndroidMessageListener>();
        }
    }
}