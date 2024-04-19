using System;
using Cinemachine;
using Cysharp.Threading.Tasks;
using Ltg8.Player;
using TriInspector;
using UnityEngine;
using UnityEngine.Events;

namespace Ltg8.Gameplay
{
    public class LevelConnection : MonoBehaviour
    {
        [Scene] public string targetScenePath;
        public int connectionId = 0;
        public int targetId = 0;
        public Transform spawnPosition;
        public UnityEvent onPlayerEnter;
        public UnityEvent onPlayerExit;
        public CinemachineVirtualCamera enterCamera;

        public void MovePlayerThrough()
        {
            MovePlayerTask().Forget();
        }
        
        private async UniTask MovePlayerTask()
        {
            onPlayerExit.Invoke();
            await Ltg8.LevelChangeTransition.Show();
            await Ltg8.GameState.TransitionTo(new OverworldGameState(targetScenePath));
            await UniTask.Delay(TimeSpan.FromSeconds(1));

            LevelConnection targetConnection = null;
            
            foreach (LevelConnection connection in FindObjectsOfType<LevelConnection>())
            {
                if (connection.connectionId == targetId)
                {
                    targetConnection = connection;
                    break;
                }
            }

            if (targetConnection == null)
                throw new Exception($"No connection with id {targetId} found!");

            PlayerController player = FindAnyObjectByType<PlayerController>();
            player.transform.SetPositionAndRotation(targetConnection.spawnPosition.position, targetConnection.spawnPosition.rotation);
            targetConnection.onPlayerEnter.Invoke();
            targetConnection.enterCamera.enabled = true;
            CinemachineCore.GetBlendOverride = GetBlendOverrideCut;
            await UniTask.Yield();
            await UniTask.Yield();
            CinemachineCore.GetBlendOverride = null;
            
            await Ltg8.LevelChangeTransition.Hide();
        }
        
        private CinemachineBlendDefinition GetBlendOverrideCut(
            ICinemachineCamera fromvcam,
            ICinemachineCamera tovcam,
            CinemachineBlendDefinition defaultblend,
            MonoBehaviour owner)
        {
            return new CinemachineBlendDefinition { m_Style = CinemachineBlendDefinition.Style.Cut };
        }
    }
}
