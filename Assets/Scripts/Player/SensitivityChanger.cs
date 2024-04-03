using Ltg8.Player;
using UnityEngine;

public class SensitivityChanger : MonoBehaviour
{

    public void ChangeSensitivity(int input)
    {
        gameObject.GetComponent<PlayerController>().ChangePlayerSensitivity(input);
    }
    
}
