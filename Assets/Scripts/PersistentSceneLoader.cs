using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Ensures that the persistent scene is always loaded
/// first when the game starts. 
/// </summary>
public static class PersistentSceneLoader
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static async void Initialize()
    {
        string initialSceneName = SceneManager.GetActiveScene().name;

        if (initialSceneName != "persistent")
        {
            SceneManager.LoadScene("persistent");
        }

        // Wait one frame so entrypoint can initialize itself
        await UniTask.Yield();

        // If we had another scene open initially, now we load it.
        if (initialSceneName != "persistent")
        {
            // note: we may need a more complicated loading system if levels require more setup
            await SceneManager.LoadSceneAsync(initialSceneName, LoadSceneMode.Additive);
            SceneManager.SetActiveScene(SceneManager.GetSceneByName(initialSceneName));
        }
        else
        {
            await SceneManager.LoadSceneAsync(1, LoadSceneMode.Additive);
            SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(1));
        }
    }
}
