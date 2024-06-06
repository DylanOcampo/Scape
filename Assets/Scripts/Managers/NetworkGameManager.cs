using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class NetworkGameManager : NetworkBehaviour, IStateAuthorityChanged
{
    private DeckManager DeckInstance;
    private GameLogicManager GameInstance;
    public void StateAuthorityChanged()
    {
        
    }

    public override void Spawned(){
        base.Spawned();


        if (Object.HasStateAuthority == true){
            HostOnlystuff();
        }
    }

    private void HostOnlystuff(){
        
        DeckInstance =gameObject.GetComponent<DeckManager>();
        GameInstance = gameObject.GetComponent<GameLogicManager>();
    }
}
