using UnityEngine;
using Fusion;
using Fusion.Sockets;
using System.Collections.Generic;
using System;

/// <summary>
/// Basic player spawn based on the main shared mode sample.
/// </summary>
public class PlayerSpawner : SimulationBehaviour, INetworkRunnerCallbacks
{
    public GameObject PlayerPrefab, RoomPlayerPrefab;
    private Dictionary<PlayerRef, NetworkObject> _spawnedCharacters = new Dictionary<PlayerRef, NetworkObject>();
    public void OnInput(NetworkRunner runner, NetworkInput input) { }
    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input) { }
    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason) { }
    public void OnConnectedToServer(NetworkRunner runner) { }
    public void OnDisconnectedFromServer(NetworkRunner runner, NetDisconnectReason reason) { }
    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token) { }
    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason) { }
    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message) { }
    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList) { }
    public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data) { }
    public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken) { }
    public void OnSceneLoadDone(NetworkRunner runner) { }
    public void OnSceneLoadStart(NetworkRunner runner) { }
    public void OnObjectExitAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player){ }
    public void OnObjectEnterAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player){ }
    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ReliableKey key, ArraySegment<byte> data){ }
    public void OnReliableDataProgress(NetworkRunner runner, PlayerRef player, ReliableKey key, float progress){ }
    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {
        if (runner.IsServer)
        {
            NetworkObject resultingPlayer = Runner.Spawn(RoomPlayerPrefab, new Vector3( 0,0,0), Quaternion.identity);
            NetworkObject RealPlayer = Runner.Spawn(PlayerPrefab, new Vector3( 0,0,0), Quaternion.identity);
            FusionConnection connector = FusionConnection.instance;
            if (connector != null)
            {
                resultingPlayer.transform.parent = FusionConnection.instance.LobbyContainer.transform;
                RealPlayer.transform.parent = FusionConnection.instance.RealPlayerContainer.transform;
                var testPlayer = resultingPlayer.GetComponent<LobbyPlayer>();

                string playerName = connector.LocalPlayerName;

                if (string.IsNullOrEmpty(playerName))
                    testPlayer.ChangePlayer("Player " + resultingPlayer.StateAuthority.PlayerId);
                else
                    testPlayer.ChangePlayer(playerName);

                _spawnedCharacters.Add(player, RealPlayer);
            }       
        }

        FusionConnection.instance.OnPlayerJoin(Runner);
        Debug.Log("-------------------------------------------");
        Debug.Log(runner.SessionInfo);
    }

    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player) { 

        if (_spawnedCharacters.TryGetValue(player, out NetworkObject networkObject))
        {
            runner.Despawn(networkObject);
            _spawnedCharacters.Remove(player);
        }
    }

}
