using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using UnityEngine.SceneManagement;


public class FusionConnection : MonoBehaviour
{
    private static FusionConnection _instance;

    public static FusionConnection instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<FusionConnection>();
            }
            return _instance;
        }
    }
    public string LocalPlayerName { get; set; }
    public string LocalRoomName { get; set; }

    public NetworkRunner _networkRunnerPrefab;
    [Header ("Menus")]
    public GameObject LobbyMenu, Game, PlayMenu;
    [Header ("Lobby")]
    public GameObject PF_Player, LobbyContainer, networkGameManager, RealPlayerContainer;
    StartGameArgs startGameArgs = new StartGameArgs();

    public async void StartGame(bool CreateRoom)
    {
        if(CreateRoom && LocalRoomName == null){
            return;
        }

        var scene = SceneRef.FromIndex(SceneManager.GetActiveScene().buildIndex);
        var sceneInfo = new NetworkSceneInfo();
        if (scene.IsValid) {
            sceneInfo.AddSceneRef(scene, LoadSceneMode.Additive);
        }

        StartGameArgs startGameArgs = new StartGameArgs();
        startGameArgs.GameMode= GameMode.Shared;
        startGameArgs.PlayerCount = 4;
        startGameArgs.Scene = scene;

        if(LocalRoomName == null){
            startGameArgs.SessionName = string.Empty;
        }else{
            startGameArgs.SessionName = LocalRoomName;
        }


        NetworkRunner newRunner = Instantiate(_networkRunnerPrefab);

        StartGameResult result = await newRunner.StartGame(startGameArgs);

        if (result.Ok)
        {
            LobbyMenu.GetComponent<LobbyInterface>().RoomName.text = "Room:  " + newRunner.SessionInfo.Name;
            GoToLobby();
        }
        else
        {
            LobbyMenu.GetComponent<LobbyInterface>().RoomName.text = "Room:  ";

            GoToMainMenu();

            Debug.LogError(result.ErrorMessage);
        }

    }

    internal void OnPlayerJoin(NetworkRunner runner)
    {
        Debug.Log(runner.ActivePlayers);
        Debug.Log("PLAYER JOINED");
    }

    public void GoToLobby()
    {
        LobbyMenu.SetActive(true);
        //Change window to go to lobby
        
    }

    public void GoToMainMenu()
    {
        LobbyMenu.SetActive(false);
        //Change window to go to menu
        
    }

    public void OnClick_StartGame(){
        
        
        NetworkRunner runner = null;
        // If no runner has been assigned, we cannot start the game
        if (NetworkRunner.Instances.Count > 0)
        {
            runner = NetworkRunner.Instances[0];
        }

        if(runner.IsSharedModeMasterClient){
            runner.Spawn(networkGameManager);
            

        }
        UIManager.instance.OnClick_PlayButton(1);
    }

}
