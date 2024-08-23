
using UnityEngine;

public class UnhideAmogus : MonoBehaviour
{

    [SerializeField] private GameObject hiddenAmogus;
    
    public void UnhideAmogusScript()
    {
        hiddenAmogus.SetActive(true);
    }
}
