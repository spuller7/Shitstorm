using UnityEngine;
using System.Collections;

public class PlayerShoot : MonoBehaviour {

    //damage given to hit player
    private int damage;

    void Start()
    {
        damage = 15;
    }
	public void shoot()
    {
        //originate ray from attached gameobject
        //in this case will be gunPoint
        Ray ray = new Ray(gameObject.transform.position, gameObject.transform.forward);
        Debug.DrawRay(gameObject.transform.position, -gameObject.transform.right, Color.green, 20f);
    //local variable of hit object
    Transform hitTransform;
        //Hitpoint of closest hit transform
        Vector3 hitPoint;

        //FindClosestHitObject() is located below
        hitTransform = FindClosestHitObject(ray, out hitPoint);
        Debug.Log("Shot ray");
        if (hitTransform != null)
        {
            //Returns the objects the ray hit
            Debug.Log("Hit: " + hitTransform.name);

            // Come back to do a special effect at the hit location
            // DoRicochetEffectAt( hitPoint );

            PlayerHealth pH = hitTransform.GetComponent<PlayerHealth>();

            //Don't know why this is here so disabling it
            /*while (pH == null && hitTransform.parent)
            {
                hitTransform = hitTransform.parent;
                pH = hitTransform.GetComponent<PlayerHealth>();
            }*/

            // Once we reach here, hitTransform may not be the hitTransform we started with!

            if (pH != null)
            {
                PhotonView pv = pH.GetComponent<PhotonView>();
                //must have a photonview on player
                if (pv == null)
                {
                    Debug.LogError("Target object with tag: 'Player,' does not contain a photon view.");
                }
                else
                {
                    if (hitTransform != gameObject)
                    {
                        //h.TakeDamage(health.healthDamage, health.speedDamage, health.crewDamage);
                        pH.GetComponent<PhotonView>().RPC("changeHealth", PhotonTargets.All, damage);
                    }
                }
            }
        }
    }
    Transform FindClosestHitObject(Ray ray, out Vector3 hitPoint)
    {

        RaycastHit[] hits = Physics.RaycastAll(ray);

        Transform closestHit = null;
        float distance = 0;
        hitPoint = Vector3.zero;

        foreach (RaycastHit hit in hits)
        {
            if (hit.transform != this.transform && (closestHit == null || hit.distance < distance))
            {
                // We have hit something that is:
                // a) not us
                // b) the first thing we hit (that is not us)
                // c) or, if not b, is at least closer than the previous closest thing

                closestHit = hit.transform;
                distance = hit.distance;
                hitPoint = hit.point;
            }
        }

        // closestHit is now either still null (i.e. we hit nothing) OR it contains the closest thing that is a valid thing to hit

        return closestHit;
    }
    public void changeDamage(int damage)
    {
        this.damage = damage;
    }
}
