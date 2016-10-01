using UnityEngine;
using System.Collections;

public class PlayerHealth : MonoBehaviour {

    private int health;
    [SerializeField]
    private GameObject deathEffect;
	// Use this for initialization
	void Start () {
        health = 100;
	}
	[PunRPC]
	public void changeHealth(int damage)
    {
        health = health - damage;
        if(health <= 0)
        {
            //kill effect
        }
    }
}
