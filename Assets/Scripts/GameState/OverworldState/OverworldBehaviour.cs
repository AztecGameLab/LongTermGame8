using Ltg8;
using UnityEngine;

public class OverworldBehaviour : MonoBehaviour
{
    protected OverworldGameState Overworld { get; private set; }

    private void Start()
    {
        Overworld = Ltg8.Ltg8.GameState.CurrentState as OverworldGameState;
    }
}
