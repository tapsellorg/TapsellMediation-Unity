#if UNITY_ANDROID

using UnityEngine;

namespace Tapsell.Mediation.Adapter.Legacy
{
    internal static class LegacyAndroidCaller
    {
        private static readonly AndroidJavaClass AndroidClass = new AndroidJavaClass("ir.tapsell.mediation.adapter.legacy.unity.Legacy");

        internal static void Call(string methodName, params object[] args)
        {
            AndroidClass.CallStatic(methodName, args);
        }
    }
}

#endif