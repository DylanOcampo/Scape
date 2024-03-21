using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LobbyPlayer : MonoBehaviour
{
    public TextMeshProUGUI playerName;


    public void ChangePlayer(string _name){
        playerName.text = _name;
    }
}
