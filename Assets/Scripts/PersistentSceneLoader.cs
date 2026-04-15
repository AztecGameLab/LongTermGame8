using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Ltg8
{
    /// <summary>
    /// Ensures that the persistent scene is always loaded first when the game starts.
    /// <list type="number">
    ///       <item> <description><b><see cref="Initialize"/></b></description> </item>
    /// </list>
    /// </summary>
    public static class PersistentSceneLoader
    {
        // NOTE: Runs the following method before the first scene is loaded
        /// <summary>
        /// Initializes the <b>game's settings</b>, loads the <b>persistent scene</b>, and loads the <b>"starting" scene</b> (whether that is the
        /// current scene or a different one depends on the editor play strategy)
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">Throws if no Editor Play Strategy has been selected</exception>
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static async void Initialize()
        {
            // Sets the game's settings to the "ltg_settings" Scriptable Object in Assets/Resources
            Ltg8Settings settings = Resources.Load<Ltg8Settings>("ltg8_settings");
            
            // Gets the path of the current scene being loaded 
            string currentScenePath = SceneManager.GetActiveScene().path;

            // If the current scene is not the persistent scene, load the persistent scene
            if (currentScenePath != settings.persistentScenePath)
                SceneManager.LoadScene(settings.persistentScenePath);

            // Wait one frame so entrypoint can initialize itself (Daniel)
            await UniTask.Yield();
            
#if UNITY_EDITOR // If in the Unity Editor, determine the play strategy
            switch (Ltg8.Settings.editorPlayStrategy)
            {
                case EditorPlayStrategy.FromStartOfGame:
                {
                    // If have a more complex start-up, we would make a new state for that here. (Daniel)
                    // Transition to the start game state (typically the MainMenu GameState)
                    await Ltg8.GameState.TransitionTo(new MainMenuGameState());
                    break;
                }
                case EditorPlayStrategy.FromCurrentScene:
                {
                    // If we are trying to run only the persistent scene, no more steps need to be taken. We've already loaded it. (Daniel)
                    if (currentScenePath == settings.persistentScenePath)
                        break;

                    await Ltg8.Serializer.ReadFromDisk(settings.editorSaveId); // Read the editor save data from the disk
                    await Ltg8.GameState.TransitionTo(new OverworldGameState(currentScenePath)); // Transition to the state of the current scene path
                    break;
                }
                default: throw new ArgumentOutOfRangeException(); 
            }
#else
            await Ltg8.GameState.TransitionTo(new OverworldGameState("Assets/Scenes/rockyjumpyroad/rockyjumpyroad M.unity")); // todo : LAME
            // await Ltg8.StateMachine.TransitionTo(new MainMenuGameState());
            // await SceneManager.LoadSceneAsync(1, LoadSceneMode.Additive);
            // SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(1));
#endif
        }
    }
}
