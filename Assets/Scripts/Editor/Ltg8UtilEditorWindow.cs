using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;

namespace Ltg8.Editor
{
    public class Ltg8UtilEditorWindow : EditorWindow
    {
        private static readonly int Ltg8Curvature = Shader.PropertyToID("LTG8_CURVATURE");
        private Ltg8Settings _settings;
        
        [MenuItem("LTG8/Utilities")]
        private static void ShowWindow()
        {
            Ltg8UtilEditorWindow window = GetWindow<Ltg8UtilEditorWindow>();
            window.titleContent = new GUIContent("LTG8 Utilities");
            window.Show();
        }

        private void OnEnable()
        {
            _settings = Addressables.LoadAssetAsync<Ltg8Settings>("Ltg8Settings").WaitForCompletion();
        }
        
        private void OnGUI()
        {
            // Draw the 'Curvature' slider
            GUILayout.Label("Curvature");
            float newCurvature = GUILayout.HorizontalSlider(Shader.GetGlobalFloat(Ltg8Curvature), 0, 0.03f);
            Shader.SetGlobalFloat(Ltg8Curvature, newCurvature);
            GetWindow<SceneView>().Repaint();
            GUILayout.Space(25);

            // Draw the 'Load Persistent' button
            GUI.enabled = !SceneManager.GetSceneByPath(_settings.persistentScenePath).isLoaded;

            if (GUILayout.Button("Load Persistent"))
                EditorSceneManager.OpenScene(_settings.persistentScenePath, OpenSceneMode.Additive);

            GUI.enabled = true;

            // Draw the 'Settings' button
            if (GUILayout.Button("Settings"))
                Selection.activeObject = _settings;
        }
    }
}
