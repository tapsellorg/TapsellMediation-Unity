#if UNITY_ANDROID

using UnityEngine;

namespace Tapsell.Mediation.Adapter.Admob
{
    internal static class AdmobAndroidCaller
    {
        private static readonly AndroidJavaClass AndroidClass = new("ir.tapsell.mediation.adapter.admob.unity.Admob");

        internal static void Call(string methodName, params object[] args)
        {
            AndroidClass.CallStatic(methodName, args);
        }
    }
}

#endif