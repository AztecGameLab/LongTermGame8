using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Ltg8
{
    /// <summary>
    /// Ensures that the persistent scene is always loaded
    /// first when the game starts. 
    /// </summary>
    public static class PersistentSceneLoader
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static async void Initialize()
        {
            Ltg8Settings settings = Resources.Load<Ltg8Settings>("ltg8_settings");
            string currentScenePath = SceneManager.GetActiveScene().path;

            if (currentScenePath != settings.persistentScenePath)
                SceneManager.LoadScene(settings.persistentScenePath);

            // Wait one frame so entrypoint can initialize itself
            await UniTask.Yield();
            
#if UNITY_EDITOR
            switch (Ltg8.Settings.editorPlayStrategy)
            {
                case EditorPlayStrategy.FromStartOfGame:
                {
                    // If have a more complex start-up, we would make a new state for that here.
                    await Ltg8.GameState.TransitionTo(new MainMenuGameState());
                    break;
                }
                case EditorPlayStrategy.FromCurrentScene:
                {
                    // If we are trying to run only the persistent scene, no more steps need to be taken. We've already loaded it.
                    if (currentScenePath == settings.persistentScenePath)
                        break;

                    await Ltg8.Serializer.ReadFromDisk(settings.editorSaveId);
                    await Ltg8.GameState.TransitionTo(new OverworldGameState(currentScenePath));
                    break;
                }
                default: throw new ArgumentOutOfRangeException();
            }
#else
            // await Ltg8.StateMachine.TransitionTo(new MainMenuGameState());
            await SceneManager.LoadSceneAsync(1, LoadSceneMode.Additive);
            SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(1));
#endif
        }
    }
}
