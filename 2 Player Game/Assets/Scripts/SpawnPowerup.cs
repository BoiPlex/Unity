using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPowerup : MonoBehaviour
{
    public GameObject[] spawnObjects;
    // public Transform spawnPoint;
    public GameObject player1;
    public GameObject player2;
    private bool powerupSpawned = false;

    public GameObject powerupSpawnEffect;
    
    void Start()
    {
        InvokeRepeating("PowerupSpawnTime", 1f, 1f);
    }
    
    void Update()
    {

    }

    public void PowerupSpawn()
    {
        while (!powerupSpawned)
        {
            Vector3 position = new Vector3(Random.Range(-7.5f, 7.5f), Random.Range(-3.5f, 2.5f), 0f);

            if ((position - player1.transform.position).magnitude < 3 || (position - player1.transform.position).magnitude < 3)
                continue;
            else
            {
                GameObject powerup = (GameObject)Instantiate(spawnObjects[Random.Range(0, spawnObjects.Length)], position, Quaternion.identity);
                powerup.transform.localScale = transform.localScale;
                powerupSpawned = true;
                Instantiate(powerupSpawnEffect, powerup.transform.position, powerup.transform.rotation);
            }
        }
    }

    public void PowerupCollected()
    {
        powerupSpawned = false;
        InvokeRepeating("PowerupSpawnTime", 1f, 1f);
    }

    private void PowerupSpawnTime()
    {
        if (Random.Range(0, 5) < 1)
        {
            CancelInvoke();
            PowerupSpawn();
        }
    }
}
