using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Basic player spawn based on the main shared mode sample.
/// </summary>
public class PlayerSpawner : SimulationBehaviour, IPlayerJoined, IPlayerLeft
{
    public GameObject LobbyPlayerPrefab, RealPlayerPrefab;

    public void PlayerJoined(PlayerRef player)
    {
        if (player == Runner.LocalPlayer)
        {
            var resultingPlayer = Runner.Spawn(LobbyPlayerPrefab, new Vector3(0, 1, 0), Quaternion.identity);
            var realPlayer = Runner.Spawn(RealPlayerPrefab, new Vector3(0, 1, 0), Quaternion.identity);

            FusionConnection connector = GameObject.FindObjectOfType<FusionConnection>();
            if (connector != null)
            {
                var testPlayer = resultingPlayer.GetComponent<LobbyPlayer>();
                
                
                string playerName = connector.LocalPlayerName;
                Debug.Log(playerName);
                if (string.IsNullOrEmpty(playerName))
                    testPlayer.PlayerName = "Player " + resultingPlayer.StateAuthority.PlayerId;
                    
                else
                    testPlayer.PlayerName = playerName;

                // Assigns a random avatar
                testPlayer.RPC_ChangePlayer();
            }
        }

        FusionConnector.Instance?.OnPlayerJoin(Runner);
    }

    public void PlayerLeft(PlayerRef player)
    {

    }
}
