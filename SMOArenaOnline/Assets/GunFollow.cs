using UnityEngine;
using System.Collections;

public class GunFollow : MonoBehaviour {

    //Public Vars
    public Camera camera;
    public float speed;
    public Rigidbody rigidbody;

    //Private Vars
    private Vector3 mousePosition;
    private Vector3 direction;
    private float distanceFromObject;

    void Start()
    {
        camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
    }

    void FixedUpdate()
    {

      

        }

    }

     
 