using Cysharp.Threading.Tasks;
using TriInspector;
using UnityEngine;

namespace Ltg8.Gameplay
{
    public class LevelConnection : MonoBehaviour
    {
        [Scene] public string targetScenePath;
        public string connectionId = "MATCH ME TO ANOTHER CONNECTION";

        public void MovePlayerThrough()
        {
            MovePlayerTask().Forget();
        }
        
        private async UniTask MovePlayerTask()
        {
            await Ltg8.LevelChangeTransition.Show();
            await Ltg8.GameState.TransitionTo(new OverworldGameState(targetScenePath));
            
            await Ltg8.LevelChangeTransition.Hide();
        }
    }
}
