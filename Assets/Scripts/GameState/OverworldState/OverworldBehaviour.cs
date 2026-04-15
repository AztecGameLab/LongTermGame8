using Ltg8;
using UnityEngine;

/// <summary>
/// <b>Description</b>: Not really sure what this class does actually, I don't think it's finished/utilized 
/// <list type="number">
///       <item> <description><b><see cref="Start()"/></b></description> </item>
/// </list>
/// </summary>
public class OverworldBehaviour : MonoBehaviour
{
    // Declares the Overworld Game State object
    protected OverworldGameState Overworld { get; private set; }
    
    private void Start()
    {
        // Initializes the Overworld Game State object to the Current State
        // NOTE: "as" typecasts Overworld to OverworldGameState, returns null if not possible
        Overworld = Ltg8.Ltg8.GameState.CurrentState as OverworldGameState;
    }
}
