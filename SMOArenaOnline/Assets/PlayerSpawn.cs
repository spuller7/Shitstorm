using UnityEngine;
using System.Collections;

public class PlayerSpawn : MonoBehaviour {

    [SerializeField]
    Transform[] spawnSpots;

    [SerializeField]
    GameObject playerPrefab;

    [SerializeField]
    GameObject player2Prefab;

    public void onPlayer1Spawn()
    {
        int spot = Random.Range(0, spawnSpots.Length);
        Instantiate(playerPrefab, spawnSpots[spot].position, spawnSpots[spot].rotation);
    }
    public void onPlayer2Spawn()
    {
        int spot = Random.Range(0, spawnSpots.Length);
        Instantiate(player2Prefab, spawnSpots[spot].position, spawnSpots[spot].rotation);
    }
}
