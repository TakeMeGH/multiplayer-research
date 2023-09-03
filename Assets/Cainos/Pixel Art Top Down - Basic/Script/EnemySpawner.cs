using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class EnemySpawner : NetworkBehaviour
{
    [SerializeField] GameObject enemy;
    bool isCanSpawn = false;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (IsOwner && isCanSpawn)
        {
            
            NetworkLog.LogInfoServer("Masuk Stay");

            if(Input.GetKeyDown(KeyCode.F)){
                NetworkLog.LogInfoServer("Pencet F");
            }
            if (IsServer && Input.GetKeyDown(KeyCode.F))
            {
                NetworkLog.LogInfoServer("masuk server");
                spawnEnemy();
            }
            else if (Input.GetKeyDown(KeyCode.F))
            {
                NetworkLog.LogInfoServer("masuk bukan server");
                testServerRpc();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (IsOwner)
        {
            isCanSpawn = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (IsOwner)
        {
            isCanSpawn = false;
        }
    }

    [ServerRpc]
    void testServerRpc()
    {
        NetworkLog.LogInfoServer("masuk spawn enemy");
        spawnEnemy();
    }
    void spawnEnemy()
    {
        NetworkLog.LogInfoServer("spawn object");
        GameObject spawnedEnemy = Instantiate(enemy, transform);
        spawnedEnemy.GetComponent<NetworkObject>().Spawn(true);
    }
}
