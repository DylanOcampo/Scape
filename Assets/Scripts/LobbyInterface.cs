using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LobbyInterface : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject Check, CheckFor2, CheckFor3, CheckFor4;
    public TextMeshProUGUI RoomName;
    private int Players = 2;

    public void OnClick_StartGame(){
        
    }

    public void OnClick_NumberofPlayers(int _Players){
        Players = _Players;
        if(_Players == 2){
            CheckFor2.SetActive(true);
            CheckFor3.SetActive(false);
            CheckFor4.SetActive(false);
            return;
        }

        if(_Players == 3){
            CheckFor2.SetActive(false);
            CheckFor3.SetActive(true);
            CheckFor4.SetActive(false);
            return;
        }

        if(_Players == 4){
            CheckFor2.SetActive(false);
            CheckFor3.SetActive(false);
            CheckFor4.SetActive(true);
            return;
        }
    }




}
