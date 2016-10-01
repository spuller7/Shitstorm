using UnityEngine;
using System.Collections;

public class SpawnPickUpItems : MonoBehaviour {

    [SerializeField]
    Transform[] spawnSpots;                 //Different for every map, where players can get abilities
    [SerializeField]
    private float spawnTime = 3f;
    [Header("Allign Pick Up Item with the Spawn Rate")]
    [SerializeField]
    GameObject[] pickUpItems;               //powerups
    [Tooltip("Spawn Rate must be between 0.0 - 1.0")]
    [SerializeField]
    float[] spawnRate;                      // How long between each spawn.

    bool isPlaying;

    void Start()
    {
        isPlaying = true;
        InvokeRepeating("Spawn", spawnTime, spawnTime);
    }
    void Update()
    {
        if (isPlaying)
        {

        }
    }
    void Spawn()
    {
        if (!isPlaying)
        {

            return;
        }
        Debug.Log("here");
        int spawnPointIndex = Random.Range(0, spawnSpots.Length);
        //Need to photon later
        if(spawnSpots[spawnPointIndex].GetComponent<SpawnPlacementHolder>().getStatus() == true)
        {
            Debug.Log("Spawn taken");
            return;
        }
        int choosePickUp = Random.Range(0, pickUpItems.Length);
        
        if(Random.value > 1.0 - spawnRate[choosePickUp])
        {
            Instantiate(pickUpItems[choosePickUp], spawnSpots[spawnPointIndex].position, spawnSpots[spawnPointIndex].rotation);
            spawnSpots[spawnPointIndex].GetComponent<SpawnPlacementHolder>().setStatus(true);
        }
        else
        {
            Debug.Log("no");
        }

    }
}
