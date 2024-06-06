using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;


public class UIManager : MonoBehaviour
{
    private static UIManager _instance;

    public static UIManager instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<UIManager>();
            }
            return _instance;
        }
    }

    public GameObject FirstMenu, GameMenu, PlayMenu, EndGame, NetworkMenu, MainPlayerTurn, MainCamera;

    [Header ("AnimationEffect")]
    public GameObject FirstPosition, SecondPosition, ThirdPosition, Card, Center;

    [Header ("AsEffect")]
    public GameObject AsMenu, FirstPlayer, SecondPlayer, ThirdPlayer;
    public void OnClick_StartButton(){
        FirstMenu.SetActive(false);
        PlayMenu.SetActive(true);
        
    }

    public void OnClick_PlayButton(int cpuPlayers){
        PlayMenu.SetActive(false);
        NetworkMenu.SetActive(false);
        GameMenu.SetActive(true);
        GameLogicManager.instance.InitializeScape(cpuPlayers);
    }

    public void EndGameEffect(){
        Debug.Log("END");
    }

    public void ErrorEffect(){
        AudioManager.instance.PlayClip(0);
    }

    public void AsCardEffect(int NumOfPlayersInGame){
        if(GameLogicManager.instance.GetMainPlayer().isMyTurn){
            AsMenu.SetActive(true);
            FirstPlayer.SetActive(true);
            if(NumOfPlayersInGame == 3){
                SecondPlayer.SetActive(true);
            }
            if(NumOfPlayersInGame == 4){
                ThirdPlayer.SetActive(true);
            }
        }else{
            GameLogicManager.instance.TurnLogicAs();
        }
        
    }

    public void AsCardEffectEnd(){
        AsMenu.SetActive(false);
        FirstPlayer.SetActive(false);   
        SecondPlayer.SetActive(false);
        ThirdPlayer.SetActive(false);
        
    }

    public void MainPlayerTurnEffect(){
        MainPlayerTurn.SetActive(true);
        MainPlayerTurn.GetComponent<CanvasGroup>().alpha = 0;
        MainPlayerTurn.GetComponent<CanvasGroup>().DOFade(1, 1).OnComplete(OnCallback_MainPlayerTurnEffect);
    }

    public void OnCallback_MainPlayerTurnEffect(){
        MainPlayerTurn.GetComponent<CanvasGroup>().DOFade(0, 1).OnComplete(() => MainPlayerTurn.SetActive(false));
    }

    public Tween AnimationEffect(int i ){
        
        Card.SetActive(true);
        Card.transform.localScale = new Vector3(1,1,1);
        if( i == 1){
            Card.transform.position = FirstPosition.transform.position;
        }
        if( i == 2){
            Card.transform.position = SecondPosition.transform.position;
        }
        
        if( i == 3){
            Card.transform.position = ThirdPosition.transform.position;
        }
        Card.transform.DOScale(0, 1);
        return Card.transform.DOMove( Center.transform.position, 1);
    }

}
