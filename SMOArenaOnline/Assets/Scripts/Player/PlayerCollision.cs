using UnityEngine;
using System.Collections;

public class PlayerCollision : MonoBehaviour {
    [SerializeField]
    private PlayerBoost pB;

    void Start() {
        if(pB == null)
        {
            Debug.LogError("PlayerBoost is not assigned");
        }
    }


    public PlayerBoost getPlayerBoost()
    {
        return pB;
    }
}
