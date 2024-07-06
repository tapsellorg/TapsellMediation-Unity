#if UNITY_ANDROID

using UnityEngine;

namespace Tapsell.Mediation
{
    internal static class MediatorAndroidCaller
    {
        private static readonly AndroidJavaClass AndroidClass = new AndroidJavaClass("ir.tapsell.mediation.Tapsell");

        internal static void Call(string methodName, params object[] args)
        {
            AndroidClass.CallStatic(methodName, args);
        }
    }
}

#endif