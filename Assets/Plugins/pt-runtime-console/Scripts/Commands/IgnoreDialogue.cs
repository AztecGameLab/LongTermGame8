using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IgnoreDialogue : MonoBehaviour
{

    private GameObject _player;
    private bool _ignoreDialogue;
    
    // Start is called before the first frame update
    private void Start()
    {
        _player = gameObject;
    }

    public void ToggleIgnoreDialogue()
    {
        _ignoreDialogue = !_ignoreDialogue;
        Debug.Log("Dialogue Toggled");
    }

    public bool DoesIgnoreDialogue()
    {
        Debug.Log("Dialogue Checked");
        return (_ignoreDialogue);
    }
}
