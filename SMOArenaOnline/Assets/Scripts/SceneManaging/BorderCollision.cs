using UnityEngine;
using System.Collections;

public class BorderCollision : MonoBehaviour {


    void OnCollisionEnter(Collision collision)
    {
        GameObject player = collision.gameObject;
        collision.gameObject.GetComponent<PlayerHealth>().changeHealth(1000, 1);

    }
}
