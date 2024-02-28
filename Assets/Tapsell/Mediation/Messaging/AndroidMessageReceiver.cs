#if UNITY_ANDROID

using UnityEngine;

namespace Tapsell.Mediation.Messaging
{
    /**
     * A static class holding the SDK `GameObject` to which our [Messenger] is attached
     */
    internal static class AndroidMessageReceiver
    {
        private const string TapsellObjectName = "TapsellMediationGameObject";

        private static GameObject _tapsellObject;

        internal static void RegisterReceiver<T>() where T : TapsellAndroidMessageListener
        {
            Initialize();
            _tapsellObject.AddComponent<T>();
        }

        private static void Initialize()
        {
            if (_tapsellObject != null) return;
            _tapsellObject = new GameObject(TapsellObjectName);
            Object.DontDestroyOnLoad(_tapsellObject);
        }
    }

    internal class TapsellAndroidMessageListener : MonoBehaviour {}
}

#endif