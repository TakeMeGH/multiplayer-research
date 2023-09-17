using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LobbyUIController : MonoBehaviour
{

    [SerializeField] LobbyController lobbyController;
    [SerializeField] TMP_Text tMP_Text;
    
    public string gameCode;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void joinLobby(){
        lobbyController.JoinLobby(gameCode);
    }

    public void changeText(string name){
        tMP_Text.text = name;
    }
}
