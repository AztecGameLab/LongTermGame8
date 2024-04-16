using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SigmundEjected : MonoBehaviour
{

    private GameObject _sigmund;
    [SerializeField] private TextMeshProUGUI textBox;
    [SerializeField] private Canvas canvas;
    [SerializeField] private GameObject amongSigmund; 
    private string _text;
    private bool _startText;
    private int _timer;
    private int _counter;
    private bool _startScene;
    
    
    private void OnEnable()
    {
        _sigmund = gameObject;
        _text = "Sigmund was not the Impostor";
        _counter = 0; _timer = 0;
        textBox.text = "";
        _startScene = true;
        _sigmund.transform.localPosition = new Vector3(-75.1999969f,29.7000008f,1.29779994f);
        _startText = false;
    }

    void Update()
    {
        _sigmund.transform.position += new Vector3(0.7f, 0, 0);
        _sigmund.transform.Rotate(2f,0,0);
        _timer++;
        if (_startText)
        {
            if (_timer % 5 != 0) return;
            if (!(_counter >= _text.Length))
                textBox.text += _text[_counter++];
            if (_sigmund.transform.localPosition.x > canvas.transform.localPosition.x + 120)
            {
                amongSigmund.SetActive(false);
            }
        }
        else
        {
            if (_sigmund.transform.localPosition.x > canvas.transform.localPosition.x)
            {
                _startText = true;
            }
        }
    }
}
