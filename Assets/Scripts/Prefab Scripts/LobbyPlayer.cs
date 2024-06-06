using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Fusion;

public class LobbyPlayer : NetworkBehaviour
{
    public TextMeshProUGUI playerName;

    [Networked(), OnChangedRender(nameof(RPC_ChangePlayer))]
    public NetworkString<_16> PlayerName { get; set; }


    public override void Spawned(){
        base.Spawned();
        this.transform.SetParent(FusionConnection.instance.LobbyContainer.transform);
        this.transform.localScale = new Vector3(1,1,1);
        RPC_ChangePlayer();

        if (Object.HasStateAuthority == true){

        }
    }

    public void RPC_ChangePlayer(){
        playerName.text = PlayerName.Value;
    }
}
