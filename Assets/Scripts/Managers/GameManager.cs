using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;

    public static GameManager instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<GameManager>();
            }
            return _instance;
        }
    }
    public List<Player> playersInGame = new List<Player>();
    public List<GameObject> possiblePlayers = new List<GameObject>();

    public GameObject otherPlayersContainer, MainPlayer;

    private Player CurrentTurn;
    public int TurnPosition;
    public int NumOfPlayersInGame()
    {
        return playersInGame.Count;
    }

    public List<Player> PlayersInGame()
    {
        return playersInGame;
    }
    // Start is called before the first frame update

    public void ResetPlayers()
    {
        MainPlayer.GetComponent<Player>().ResetPlayer();
        foreach (GameObject item in possiblePlayers)
        {
            item.GetComponent<Player>().ResetPlayer();
        }
    }



    public void InitializeScape(int players)
    {
        ConfigPlayers(players);
        DeckManager.instance.InitializeDeck();
        CurrentTurn = MainPlayer.GetComponent<Player>();
        CurrentTurn.isMyTurn = true;
        TurnPosition = 0;
        UIManager.instance.MainPlayerTurnEffect();
        MainMenuManager.instance.CardMenuReset(false);
        PileManager.instance.CleanCardPile();
    }

    private void ConfigPlayers(int players)
    {
        for (int i = 1; i < playersInGame.Count; i++)
        {
            playersInGame[i].gameObject.SetActive(false);
        }
        playersInGame.Clear();
        playersInGame.Add(MainPlayer.GetComponent<Player>());
        for (int i = 0; i < players; i++)
        {
            possiblePlayers[i].GetComponent<Player>().SetCPUPlayer();
            playersInGame.Add(possiblePlayers[i].GetComponent<Player>());
            possiblePlayers[i].SetActive(true);
        }
    }

    private int CorrectTurnPosition(int TurnPosition)
    {

        while (TurnPosition >= NumOfPlayersInGame())
        {
            TurnPosition = TurnPosition - NumOfPlayersInGame();
        }
        return TurnPosition;
    }

    private void AssignTurn(int TurnRate)
    {
        TurnRate = TurnRate + 1;
        TurnPosition = TurnPosition + TurnRate;
        TurnPosition = CorrectTurnPosition(TurnPosition);
        CurrentTurn = playersInGame[TurnPosition];
        CurrentTurn.isMyTurn = true;

    }

    private void ForceTurn(int Playernumber)
    {
        TurnPosition = Playernumber;
        CurrentTurn = playersInGame[TurnPosition];
        CurrentTurn.isMyTurn = true;

    }


    public void TurnLogic(bool burn = false, int TurnRate = 0, CardHolder _card = null, bool identifier = false)
    {
        if (!identifier)
        {
            CurrentTurn.DeleteCardFromHand(_card);
        }

        if (CurrentTurn.NeedsToDealACard())
        {
            if (!DeckManager.instance.DealCard(CurrentTurn))
            {
                if (CurrentTurn.DealPackage())
                {
                    return;
                }
            }
        }
        if (!burn)
        {
            CurrentTurn.isMyTurn = false;
            AssignTurn(TurnRate);
        }


        if (!CurrentTurn.CheckIfHandIsPossibleToPlay())
        {
            DeckManager.instance.DealPile(CurrentTurn);
        }
        if (TurnPosition == 0)
        {
            CurrentTurn.CorrectHand();
            UIManager.instance.MainPlayerTurnEffect();
            CurrentTurn.NeedsToDealACard();
        }
        else
        {
            CurrentTurn.CorrectHand();
            CurrentTurn.CPUPlayerTurn();
        }


    }

    public Player GetCurrentPlayer()
    {
        return CurrentTurn;
    }

    public void RandomTurn()
    {
        if (NumOfPlayersInGame() == 2)
        {
            ForceTurn(0);
            return;
        }
        else
        {
            int tempTurn = -1;
            while (TurnPosition != -1 && TurnPosition != tempTurn)
            {
                tempTurn = Random.Range(0, NumOfPlayersInGame());
            }
            ForceTurn(tempTurn);
            return;
        }

    }

    public void TurnLogicAs()
    {
        if (CurrentTurn.NeedsToDealACard())
        {
            if (!DeckManager.instance.DealCard(CurrentTurn))
            {
                CurrentTurn.DealPackage();
            }
        }
        UIManager.instance.AsCardEffectEnd();
        CurrentTurn.isMyTurn = false;
        RandomTurn();
        if (!CurrentTurn.CheckIfHandIsPossibleToPlay())
        {
            DeckManager.instance.DealPile(CurrentTurn);
        }

        if (TurnPosition == 0)
        {
            UIManager.instance.MainPlayerTurnEffect();
        }
        else
        {
            CurrentTurn.CPUPlayerTurn();
        }
    }



    public void TurnLogicAs(int PlayerNumber)
    {
        if (CurrentTurn.NeedsToDealACard())
        {
            if (!DeckManager.instance.DealCard(CurrentTurn))
            {
                CurrentTurn.DealPackage();
            }
        }

        UIManager.instance.AsCardEffectEnd();
        CurrentTurn.isMyTurn = false;
        ForceTurn(PlayerNumber);


        if (CurrentTurn.CheckIfHandIsPossibleToPlay())
        {
            if (CurrentTurn.NeedsToDealACard())
            {
                if (!DeckManager.instance.DealCard(CurrentTurn))
                {
                    CurrentTurn.DealPackage();
                }
            }
        }
        else
        {
            DeckManager.instance.DealPile(CurrentTurn);
        }

        if (TurnPosition == 0)
        {
            UIManager.instance.MainPlayerTurnEffect();
        }
        else
        {
            CurrentTurn.CPUPlayerTurn();
        }
    }


}
