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
        private SerializedProperty _admobSignatureAndroid;

        [MenuItem("Assets/Tapsell/Settings...")]
        public static void OpenInspector()
        {
            Selection.activeObject = TapsellMediationSettings.LoadInstance();
        }

        public void OnEnable()
        {
            _appKeyAndroid = serializedObject.FindProperty("tapsellAndroidAppKey");
            _admobSignatureAndroid = serializedObject.FindProperty("admobAdapterSignature");
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

            // Adapters signatures
            var admobImported = Directory.Exists(AdaptersDirectory + "Admob");

            if (admobImported)
            {
                EditorGUILayout.LabelField("Mediation Adapters Signature (Android)",
                    EditorStyles.boldLabel);
                EditorGUI.indentLevel++;
                
                EditorGUILayout.PropertyField(_admobSignatureAndroid, new GUIContent("Admob Signature"));

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
