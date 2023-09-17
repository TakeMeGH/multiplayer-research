using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;


public class LobbyController : MonoBehaviour
{

    private Lobby joinedLobby;
    public List<GameObject> lobbyUIlist;
    float heartbeatTimer = 0;
    float showLobbyTimer = 10f;
    // Start is called before the first frame update
    async void Start()
    {
        await UnityServices.InitializeAsync();

        AuthenticationService.Instance.SignedIn += () =>
        {
            Debug.Log("Masuk " + AuthenticationService.Instance.PlayerId);
        };

        await AuthenticationService.Instance.SignInAnonymouslyAsync();
    }

    // Update is called once per frame
    void Update()
    {
        HandleLobbyHeartbeat();
        showLobby();
        // HandleLobbyPolling();
    }

    public async void CreateLobby()
    {
        try
        {
            string lobbyName = "Test Lobby";
            int maxPlayers = 10;

            Lobby lobby = await LobbyService.Instance.CreateLobbyAsync(lobbyName, maxPlayers);
            joinedLobby = lobby;

            Debug.Log("Lobby berhasil dibuat : " + lobbyName + " " + maxPlayers);
        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
        }
    }

    private async void HandleLobbyHeartbeat()
    {
        if (joinedLobby != null)
        {
            heartbeatTimer -= Time.deltaTime;
            if (heartbeatTimer < 0f)
            {
                float heartbeatTimerMax = 3f;
                heartbeatTimer = heartbeatTimerMax;

                Debug.Log("Heartbeat");
                await LobbyService.Instance.SendHeartbeatPingAsync(joinedLobby.Id);
            }
        }
    }

    public async void JoinLobby(string gameCode)
    {
        try
        {
            Debug.Log(gameCode);
            joinedLobby = await Lobbies.Instance.JoinLobbyByIdAsync(gameCode);

            Debug.Log("Berhasil join lobby : " + gameCode);
        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
        }
    }


    public async void debugListLobby()
    {
        try
        {
            QueryResponse queryResponse = await Lobbies.Instance.QueryLobbiesAsync();
            Debug.Log("Banyak lobby : " + queryResponse.Results.Count);
            foreach (Lobby lobby in queryResponse.Results)
            {
                Debug.Log(lobby.Name + " " + lobby.MaxPlayers);
            }
        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
        }
    }

    public async void debugCurrentLobby()
    {
        Lobby lobby = await LobbyService.Instance.GetLobbyAsync(joinedLobby.Id);
        joinedLobby = lobby;
        Debug.Log("Nama lobby : " + joinedLobby.Name);
        foreach (Player player in joinedLobby.Players)
        {
            Debug.Log("Player ID : " + player.Id);
        }
    }

    public async void showLobby()
    {
        if (showLobbyTimer < 0)
        {
            showLobbyTimer = 10f;
            Debug.Log("SHOW LOBBY");
            QueryResponse queryResponse = await Lobbies.Instance.QueryLobbiesAsync();

            int index = 0;
            foreach (Lobby lobby in queryResponse.Results)
            {
                if (index >= 3) break;
                lobbyUIlist[index].GetComponent<LobbyUIController>().gameCode = lobby.Id;
                lobbyUIlist[index].GetComponent<LobbyUIController>().changeText(lobby.Name);
                Debug.Log("Lobby Code Show : " + lobby.Id + " " + lobby.Name);
            }
        }
        else showLobbyTimer -= Time.deltaTime;
    }

    public static void test(){
        Debug.Log(321321);
    }
}
