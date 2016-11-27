using UnityEngine;
using System.Collections;

public class SpawnPlacementHolder : MonoBehaviour {

    [SerializeField]
    private bool isTaken;

    public bool getStatus() {
        return isTaken;
    }
    public void setStatus(bool isTaken)
    {
        this.isTaken = isTaken;
    }
}
