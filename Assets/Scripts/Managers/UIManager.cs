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

    public GameObject FirstMenu, GameMenu, EndGame, MainPlayerTurn, DealPile;

    [Header("AnimationEffect")]
    public GameObject FirstPosition, SecondPosition, ThirdPosition, Card, Center;

    [Header("AsEffect")]
    public GameObject AsMenu, FirstPlayer, SecondPlayer, ThirdPlayer;

    public void OnClick_PlayButton(int cpuPlayers)
    {
        GameMenu.SetActive(true);
        FirstMenu.SetActive(false);
        GameManager.instance.InitializeScape(cpuPlayers);
    }

    public void OnClick_Restart()
    {
        FirstMenu.SetActive(true);
        GameMenu.SetActive(false);
        EndGame.SetActive(false);
        GameManager.instance.ResetPlayers();
        Card.SetActive(false);
    }

    public void EndGameEffect(bool win)
    {
        EndGame.SetActive(true);
        if (win)
        {
            EndGame.GetComponent<EndGame>().SetText("You win");
        }
        else
        {
            EndGame.GetComponent<EndGame>().SetText("You lost");
        }
    }

    public void DealPileEffect()
    {
        DealPile.SetActive(true);
        DealPile.GetComponent<CanvasGroup>().alpha = 1;
        DealPile.GetComponent<CanvasGroup>().DOFade(0, 1).OnComplete(() => DealPile.SetActive(false));
    }

    public void ErrorEffect()
    {
        AudioManager.instance.PlayClip(0);
    }

    public void AsCardEffect(int NumOfPlayersInGame)
    {
        if (GameManager.instance.MainPlayer.GetComponent<Player>().isMyTurn)
        {
            AsMenu.SetActive(true);
            FirstPlayer.SetActive(true);
            if (NumOfPlayersInGame == 3)
            {
                SecondPlayer.SetActive(true);
            }
            if (NumOfPlayersInGame == 4)
            {
                ThirdPlayer.SetActive(true);
            }
        }
        else
        {
            GameManager.instance.TurnLogicAs();
        }

    }

    public void AsCardEffectEnd()
    {
        AsMenu.SetActive(false);
        FirstPlayer.SetActive(false);
        SecondPlayer.SetActive(false);
        ThirdPlayer.SetActive(false);

    }

    public void MainPlayerTurnEffect()
    {
        MainPlayerTurn.SetActive(true);
        MainPlayerTurn.GetComponent<CanvasGroup>().alpha = 0;
        MainPlayerTurn.GetComponent<CanvasGroup>().DOFade(1, 1).OnComplete(OnCallback_MainPlayerTurnEffect);
    }

    public void OnCallback_MainPlayerTurnEffect()
    {
        MainPlayerTurn.GetComponent<CanvasGroup>().DOFade(0, 1).OnComplete(() => MainPlayerTurn.SetActive(false));
    }

    public Tween AnimationEffect(int i)
    {

        Card.SetActive(true);
        Card.transform.localScale = new Vector3(1, 1, 1);
        if (i == 1)
        {
            Card.transform.position = FirstPosition.transform.position;
        }
        if (i == 2)
        {
            Card.transform.position = SecondPosition.transform.position;
        }

        if (i == 3)
        {
            Card.transform.position = ThirdPosition.transform.position;
        }
        Card.transform.DOScale(0, 1);
        return Card.transform.DOMove(Center.transform.position, 1);
    }

}
