using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine;
using UnityEngine.UI;

public class NetworkManagerUI : MonoBehaviour
{

    [SerializeField] Button host;
    [SerializeField] Button client;
    // Start is called before the first frame update
    void Start()
    {
        host.onClick.AddListener(() =>{
            NetworkManager.Singleton.StartHost();
        });
        client.onClick.AddListener(() =>{
            NetworkManager.Singleton.StartClient();
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
