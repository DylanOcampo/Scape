using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
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

    public GameObject FirstMenu, GameMenu, PlayMenu, EndGame, AsCardMenu, MainPlayerTurn, MainCamera;

    public GameObject FirstPosition, SecondPosition, ThirdPosition, Card, Center;

    public void OnClick_StartButton(){
        FirstMenu.SetActive(false);
        PlayMenu.SetActive(true);
    }

    public void OnClick_PlayButton(int cpuPlayers){
        PlayMenu.SetActive(false);
        GameMenu.SetActive(true);
        GameManager.instance.InitializeScape(cpuPlayers);
    }

    public void EndGameEffect(){

    }

    public void ErrorEffect(){
        AudioManager.instance.PlayClip(0);
    }

    public void AsCardEffect(){

    }

    public void MainPlayerTurnEffect(){
        Debug.Log(123);
        MainPlayerTurn.SetActive(true);
        MainPlayerTurn.GetComponent<CanvasGroup>().alpha = 0;
        MainPlayerTurn.GetComponent<CanvasGroup>().DOFade(1, 3).OnComplete(OnCallback_MainPlayerTurnEffect);
    }

    public void OnCallback_MainPlayerTurnEffect(){
        MainPlayerTurn.GetComponent<CanvasGroup>().DOFade(0, 1).OnComplete(() => MainPlayerTurn.SetActive(false));
    }

    public void AnimationEffect(int i ){
        Card.SetActive(true);
        if( i == 1){
            Card.transform.position = FirstPosition.transform.position;
            Card.transform.DOMove( Center.transform.position, 1).OnComplete(OnCallback_AnimationEffect);
        }
        if( i == 2){
            Card.transform.position = SecondPosition.transform.position;
            Card.transform.DOMove( Center.transform.position, 1).OnComplete(OnCallback_AnimationEffect);
        }
        
        if( i == 3){
            Card.transform.position = ThirdPosition.transform.position;
            Card.transform.DOMove( Center.transform.position, 1).OnComplete(OnCallback_AnimationEffect);
        }
        
    }

    public void OnCallback_AnimationEffect(){
        Card.SetActive(false);

    }
}
