using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EndGame : MonoBehaviour
{
    public TextMeshProUGUI winText;


    public void SetText(string value)
    {
        winText.text = value;
    }
}
