using UnityEngine;
using System.Collections;

[RequireComponent(typeof(SphereCollider))]

public class OnPickUp : MonoBehaviour {

    [SerializeField]
    private bool isWeapon;
    [SerializeField]
    private int weaponId;
    [SerializeField]
    private float fuelAdditiveLevel = 45;
    private GameObject spawn;

    void OnTriggerEnter(Collider collision)
    {
        Debug.Log("Has Collided");
        if (!isWeapon)
        {
            boostHandler(collision.gameObject);
        }
        else {
            weaponHandler(collision.gameObject);
        }
        Destroy(gameObject);
        spawn.GetComponent<SpawnPlacementHolder>().setStatus(false);

    }
    public void setSpawnObject(GameObject spawn)
    {
        this.spawn = spawn;
    }

    private void boostHandler(GameObject player)
    {
        if (player.GetComponent<PlayerCollision>() != null)
        {
            player.GetComponent<PlayerCollision>().getPlayerBoost().addBoost(fuelAdditiveLevel);
        }
        else
        {
            Debug.LogError("Collision with powerup does not contain 'PlayerBoost'");
        }

    }
    private void weaponHandler(GameObject player)
    {

    }


        void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
