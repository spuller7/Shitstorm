using UnityEngine;
using System.Collections;

public class PlayerHealth : MonoBehaviour {

    private int health;
    [SerializeField]
    private GameObject deathEffect;
    [SerializeField]
    private GameObject bloodEffect;
	void Start () {
        health = 100;
	}
	[PunRPC]
	public void changeHealth(int damage, int bloodPosition)
    {
        Debug.Log(bloodPosition);
        health = health - damage;
        if(health <= 0)
        {
            Instantiate(deathEffect, gameObject.transform.position, gameObject.transform.rotation);
            Destroy(gameObject);

        }
        if(bloodPosition == 1)
        {
            
            Quaternion rot = Quaternion.Euler(new Vector3(gameObject.transform.rotation.x, 0, gameObject.transform.rotation.z));
            Instantiate(bloodEffect, gameObject.transform.position, rot);
        }
        else
        {
            Quaternion rot = Quaternion.Euler(new Vector3(gameObject.transform.rotation.x, 180, gameObject.transform.rotation.z));
            Instantiate(bloodEffect, gameObject.transform.position, gameObject.transform.rotation);
        }
    }
}
