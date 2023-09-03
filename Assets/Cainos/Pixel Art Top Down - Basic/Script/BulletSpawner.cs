using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class BulletSpawner : NetworkBehaviour
{
    [SerializeField] GameObject bullet;
    [SerializeField] BoxCollider2D boxCollider2D;
    Camera cam;
    // Start is called before the first frame update
    void Start()
    {
        cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        if (IsOwner && Input.GetKeyDown(KeyCode.Space))
        {
            spawnBullet(transform.position.x, transform.position.y, checkFacing());

        }

    }

    float checkFacing(){
        Vector3 mousePoint = cam.ScreenToWorldPoint(Input.mousePosition);
        Vector3 gunPoint = (mousePoint - boxCollider2D.bounds.center).normalized;
        float angle = Mathf.Atan2(gunPoint.y, gunPoint.x) * Mathf.Rad2Deg;
        return angle;
    }

    void spawnBullet(float posX, float posY, float angle)
    {

        if (IsServer)
        {
            Vector2 pos = new Vector2(transform.position.x, transform.position.y);
            GameObject obj = Instantiate(bullet);
            obj.transform.position = new Vector2(posX, posY);
            obj.transform.rotation = Quaternion.Euler(0, 0, angle);
            obj.GetComponent<NetworkObject>().Spawn(true);
        }
        else
        {
            testServerRpc(posX, posY, angle);
        }

    }

    [ServerRpc]
    void testServerRpc(float posX, float posY, float angle)
    {
        spawnBullet(posX, posY, angle);
    }
}
