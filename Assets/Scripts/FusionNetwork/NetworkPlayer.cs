using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;

public class NetworkPlayer : NetworkBehaviour
{
    [SerializeField]
    private Player _player;

    // Start is called before the first frame update
    public override void Spawned(){
        base.Spawned();
        this.transform.SetParent(FusionConnection.instance.RealPlayerContainer.transform);
        this.transform.localScale = new Vector3(1,1,1);

        if (Object.HasStateAuthority == true){

        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
