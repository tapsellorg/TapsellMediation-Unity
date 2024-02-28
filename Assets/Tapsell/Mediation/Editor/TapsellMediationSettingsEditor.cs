using System.IO;
using UnityEditor;
using UnityEngine;

namespace Tapsell.Mediation.Editor
{
    [InitializeOnLoad]
    [CustomEditor(typeof(TapsellMediationSettings))]
    public class TapsellMediationSettingsEditor : UnityEditor.Editor
    {
        private const string AdaptersDirectory = "Assets/Tapsell/Mediation/Adapter/";

        private SerializedProperty _appKeyAndroid;
        private SerializedProperty _appMarketKeyAndroid;
        private SerializedProperty _admobSignatureAndroid;
        private SerializedProperty _applovinSignatureAndroid;

        [MenuItem("Assets/Tapsell/Settings...")]
        public static void OpenInspector()
        {
            Selection.activeObject = TapsellMediationSettings.LoadInstance();
        }

        public void OnEnable()
        {
            _appKeyAndroid = serializedObject.FindProperty("tapsellAndroidAppKey");
            _appMarketKeyAndroid = serializedObject.FindProperty("tapsellAndroidAppMarketKey");
            _admobSignatureAndroid = serializedObject.FindProperty("admobAdapterSignature");
            _applovinSignatureAndroid = serializedObject.FindProperty("applovinAdapterSignature");
        }

        public override void OnInspectorGUI()
        {
            // Make sure the Settings object has all recent changes.
            serializedObject.Update();

            var settings = (TapsellMediationSettings)target;

            if(settings == null)
            {
              Debug.LogError("TapsellMediationSettings is null.");
              return;
            }

            // Tapsell App keys
            EditorGUILayout.LabelField("Tapsell Mediation App Key", EditorStyles.boldLabel);
            EditorGUI.indentLevel++;

            EditorGUILayout.PropertyField(_appKeyAndroid, new GUIContent("Android"));

            EditorGUILayout.HelpBox(
                    "Tapsell Mediation App Key can be found in your Tapsell app dashboard.",
                    MessageType.Info);

            EditorGUI.indentLevel--;
            EditorGUILayout.Separator();
            
            // App Market key
            EditorGUI.indentLevel--;
            EditorGUILayout.Separator();
            
            EditorGUILayout.LabelField("Tapsell Mediation App Market Key", EditorStyles.boldLabel);
            EditorGUI.indentLevel++;

            EditorGUILayout.PropertyField(_appMarketKeyAndroid, new GUIContent("Android Market"));

            EditorGUILayout.HelpBox(
                "Tapsell Mediation App Market Key is the market that your app is published in",
                MessageType.Info);

            // Adapters signatures
            var admobImported = Directory.Exists(AdaptersDirectory + "Admob");
            var applovinImported = Directory.Exists(AdaptersDirectory + "Applovin");

            if (admobImported || applovinImported)
            {
                EditorGUILayout.LabelField("Mediation Adapters Signature (Android)",
                    EditorStyles.boldLabel);
                EditorGUI.indentLevel++;

                if (admobImported)
                {
                    EditorGUILayout.PropertyField(_admobSignatureAndroid,
                        new GUIContent("Admob Signature"));
                }

                if (applovinImported)
                {
                    EditorGUILayout.PropertyField(_applovinSignatureAndroid,
                        new GUIContent("Applovin Signature"));
                }

                EditorGUILayout.HelpBox(
                    "For more information on these values, contact Tapsell support.",
                    MessageType.Info);

                EditorGUI.indentLevel--;
                EditorGUILayout.Separator();
            }


            serializedObject.ApplyModifiedProperties();
        }
    }
}
