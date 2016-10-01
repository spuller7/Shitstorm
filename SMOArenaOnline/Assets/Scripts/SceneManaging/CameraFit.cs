using UnityEngine;
using System.Collections;

public class CameraFit : MonoBehaviour {

    [SerializeField]
    private float dampTime = 0.15f;
    private Vector3 velocity = Vector3.zero;
    private float MidX;
    private float MidY;
    private float MidZ;
    private Vector3 Midpoint;
    public float distance;
    private float camDistance;
    public float CamOffset;
    public float bounds;
    [SerializeField]
    private float objectDistanceMax;
    [SerializeField]
    private float objectDistanceMin;
    [SerializeField]
    public Transform target1;
    [SerializeField]
    public Transform target2;
    [SerializeField]
    private int cameraDistanceMinimum;
    [SerializeField]
    private int cameraDistanceMaximum;
    [SerializeField]
    private Transform defaultObject;
    private bool isDead;
    private bool isPlaying;

    // Use this for initialization
    void Start()
    {
        isDead = false;
        camDistance = 5f;
        bounds = 12.0f;
    }

    // Update is called once per frame
    void Update()
    {
        //When there is only one or zero players in the arena
        if(target2 == null || isDead || !isPlaying)
        {
            Vector3 delta = Midpoint - GetComponent<Camera>().ViewportToWorldPoint(new Vector3(0.5f, 0.5f, ((cameraDistanceMaximum + cameraDistanceMinimum)/2))); 
            Vector3 destination = defaultObject.position + delta;
            transform.position = Vector3.SmoothDamp(transform.position, destination, ref velocity, dampTime);
        }
        else
        {
            //Calculate distance between both players
            distance = Vector3.Distance(target1.transform.position, target2.transform.position);
            float distanceRange = (objectDistanceMax - objectDistanceMin);
            float smallPerc = distance / objectDistanceMin;
            float largePerc = distance / objectDistanceMax;
            
            if (smallPerc <= 1.0f)
            {
                camDistance = objectDistanceMin;
            }
            else if(largePerc >= 1.0f)
            {
                camDistance = objectDistanceMax;
            }
            else
            {
                //distance from players is proportional to distance camera is
                float perc = (distance - objectDistanceMin) / distanceRange;
                camDistance = cameraDistanceMinimum + ((cameraDistanceMaximum - cameraDistanceMinimum) * perc);
            }

            MidX = (target2.position.x + target1.position.x) / 2;
            MidY = (target2.position.y + target1.position.y) / 2;
            MidZ = (target2.position.z + target1.position.z) / 2;
            Midpoint = new Vector3(MidX, MidY, MidZ);
            //Actually move camera
            Vector3 delta = Midpoint - GetComponent<Camera>().ViewportToWorldPoint(new Vector3(0.5f, 0.5f, camDistance)); //(new Vector3(0.5, 0.5, point.z));
            Vector3 destination = transform.position + delta;
            transform.position = Vector3.SmoothDamp(transform.position, destination, ref velocity, dampTime);
            
        }
    }
}
