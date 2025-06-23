#if UNITY_EDITOR

using System;
using System.Text;
using Convai.Scripts.Runtime.Core;
using Convai.Scripts.Runtime.LoggerSystem;
using UnityEditor;
using UnityEngine;

namespace Convai.Scripts.Editor.NPC
{
    /// <summary>
    ///     Custom editor for the ConvaiNPC component.
    ///     Provides functionalities to cache and restore states of all convai scripts whenever a scene is saved.
    /// </summary>
    [CustomEditor(typeof(ConvaiNPC))]
    [HelpURL("https://docs.convai.com/api-docs/plugins-and-integrations/unity-plugin/scripts-overview")]
    public class ConvaiNPCEditor : UnityEditor.Editor
    {
        private ConvaiNPC _convaiNPC;

        private void OnEnable()
        {
            _convaiNPC = (ConvaiNPC)target;
        }
        /// <summary>
        ///     Overrides the default inspector GUI to add custom buttons and functionality.
        /// </summary>
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            GUILayout.BeginHorizontal();

            // Add Components button to add necessary components and assign a random color to the character.
            if (GUILayout.Button(new GUIContent(
                        "Add Components",
                        "Adds necessary components to the NPC and assigns a random color to the character's text"
                    ),
                    GUILayout.Width(120)
                )
               ) AddComponentsToNPC();

            if (GUILayout.Button(new GUIContent(
                        "Copy Debug",
                        "Copies the session id and other essential properties to clipboard for easier debugging"
                    ),
                    GUILayout.Width(120)
                )
               ) CopyToClipboard();
            GUILayout.EndHorizontal();
        }

        private void CopyToClipboard()
        {
            StringBuilder stringBuilder = new();
            stringBuilder.AppendLine($"Endpoint: {_convaiNPC.GetEndPointURL}");
            stringBuilder.AppendLine($"Character ID: {_convaiNPC.characterID}");
            stringBuilder.AppendLine($"Session ID: {_convaiNPC.sessionID}");

            GUIUtility.systemCopyBuffer = stringBuilder.ToString();
        }

        /// <summary>
        ///     Adds components to the NPC and assigns a random color to the character's text.
        /// </summary>
        private void AddComponentsToNPC()
        {
            try
            {
                ConvaiNPCComponentSettingsWindow.Open(_convaiNPC);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            }
            catch (Exception ex)
            {
                ConvaiLogger.Exception($"Unexpected error occurred when applying changes. Error: {ex}", ConvaiLogger.LogCategory.UI);
            }
        }
    }

    /// <summary>
    ///     Provides extension methods for Unity editor components.
    /// </summary>
    public static class EditorExtensions
    {
        /// <summary>
        ///     Adds a component to the GameObject safely, catching any exceptions that occur during the process.
        /// </summary>
        /// <param name="go">The GameObject to which the component will be added.</param>
        /// <typeparam name="T">The type of the component to be added, derived from UnityEngine.Component.</typeparam>
        /// <returns>The newly added component, or null if the operation failed.</returns>
        public static T AddComponentSafe<T>(this GameObject go) where T : Component
        {
            try
            {
                return go.AddComponent<T>();
            }
            catch (Exception ex)
            {
                ConvaiLogger.Exception($"Failed to add component of type {typeof(T).Name}, Error: {ex}", ConvaiLogger.LogCategory.UI);
                return null;
            }
        }
    }
}

#endif
