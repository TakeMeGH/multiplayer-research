using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Networking.Transport.Relay;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using UnityEngine;
using TMPro;
using System;

public class RelayController : MonoBehaviour
{
    [SerializeField] TMP_InputField inputField;
    private async void Start() {
        await UnityServices.InitializeAsync();

        AuthenticationService.Instance.SignedIn += () => {
            Debug.Log("Masuk " + AuthenticationService.Instance.PlayerId);
        };

        await AuthenticationService.Instance.SignInAnonymouslyAsync();
    }
    public void joinRelayCall(){
        joinRelay(inputField.text);
    }

    public async void createRelay(){
        try{
            Allocation allocation = await RelayService.Instance.CreateAllocationAsync(2);

            string gameCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);

            Debug.Log(gameCode);

            RelayServerData relayServerData = new RelayServerData(allocation, "dtls");

            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);

            NetworkManager.Singleton.StartHost();
        }
        catch(RelayServiceException e){
            Debug.Log(e);
        }
    }

    private async void joinRelay(string gameCode){
        try{
            Debug.Log(gameCode);
            JoinAllocation joinAllocation = await RelayService.Instance.JoinAllocationAsync(gameCode);

            RelayServerData relayServerData = new RelayServerData(joinAllocation, "dtls");

            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);

            NetworkManager.Singleton.StartClient();
        }
        catch(RelayServiceException e){
            Debug.Log(e);
        }
    }
}
