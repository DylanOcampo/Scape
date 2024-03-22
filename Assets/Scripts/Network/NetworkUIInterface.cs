using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class NetworkUIInterface : MonoBehaviour
{
    // Start is called before the first frame update

    
    public void OnClick_Server(){
        NetworkManager.Singleton.StartServer();
    }

    public void OnClick_Host(){
        NetworkManager.Singleton.StartHost();
    }

    public void OnClick_Client(){
        NetworkManager.Singleton.StartClient();
    }
}
