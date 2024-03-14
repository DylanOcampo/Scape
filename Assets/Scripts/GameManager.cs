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
    public int NumOfPlayersInGame(){
        return playersInGame.Count;
    }

    public List<Player> PlayersInGame(){
        return playersInGame;
    }
    // Start is called before the first frame update


    public void InitializeScape(int players){
        playersInGame.Add(MainPlayer.GetComponent<Player>());
        for (int i = 0; i < players; i++)
        {
            possiblePlayers[i].GetComponent<Player>().SetCPUPlayer();
            playersInGame.Add(possiblePlayers[i].GetComponent<Player>());
            possiblePlayers[i].SetActive(true);
        }
        DeckManager.instance.InitializeDeck();
        CurrentTurn = MainPlayer.GetComponent<Player>();
        CurrentTurn.isMyTurn = true;
        TurnPosition = 0;
        if(CurrentTurn.CheckIfHandIsPossibleToPlay()){
            DeckManager.instance.DealPile(CurrentTurn);
        }
    }

    private void AssignTurn(int TurnRate){
        TurnRate = TurnRate + 1;
        TurnPosition = TurnPosition + TurnRate;
        if(TurnPosition >= NumOfPlayersInGame()){
            TurnPosition = 0;
        }
        CurrentTurn = playersInGame[TurnPosition];
        CurrentTurn.isMyTurn = true;
        
    }

    private void ForceTurn(int Playernumber){
        TurnPosition = Playernumber;
        CurrentTurn = playersInGame[TurnPosition];
        CurrentTurn.isMyTurn = true;
        
    }

    public void CardCpuAnimation(){
        UIManager.instance.AnimationEffect(TurnPosition);
    }


    public void TurnLogic(bool burn = false, int TurnRate = 0){
        if(!burn){
            CurrentTurn.isMyTurn = false;
            AssignTurn(TurnRate);            
        }

        if(CurrentTurn.CheckIfHandIsPossibleToPlay()){
            if(CurrentTurn.NeedsToDealACard()){
                if(!DeckManager.instance.DealCard(CurrentTurn)){
                    CurrentTurn.DealPackage();
                }
            }
        }else{
            DeckManager.instance.DealPile(CurrentTurn);
        }
        
        if(TurnPosition == 0){
                UIManager.instance.MainPlayerTurnEffect();
            }else{
                CurrentTurn.CPUPlayerTurn();
            }
        
       
    }

    public Player GetCurrentPlayer(){
        return CurrentTurn;
    }

    public void TurnLogicAs(int PlayerNumber){
        CurrentTurn.isMyTurn = false;
        ForceTurn(PlayerNumber);
        

        if(CurrentTurn.CheckIfHandIsPossibleToPlay()){
            if(CurrentTurn.NeedsToDealACard()){
                if(!DeckManager.instance.DealCard(CurrentTurn)){
                    CurrentTurn.DealPackage();
                }
            }
        }else{
            DeckManager.instance.DealPile(CurrentTurn);
        }

        if(TurnPosition == 0){
            UIManager.instance.MainPlayerTurnEffect();
        }else{
            CurrentTurn.CPUPlayerTurn();
        }
    }


}
