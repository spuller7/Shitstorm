using UnityEngine;
using System.Collections;

public class PlayerBoost : MonoBehaviour {

    [SerializeField]
    private float fuelAmount = 100;
    [SerializeField]
    private float fuelDepletionRate = 20;

    public float getFuel()
    {
        return fuelAmount;
    }

    public void addBoost(float amount)
    {
        fuelAmount = fuelAmount + amount;
        if(fuelAmount > 100.0f)
        {
            fuelAmount = 100.0f;
        }
    }
    public void depleteFuel()
    {
        fuelAmount = fuelAmount - fuelDepletionRate * Time.deltaTime;
    }
}
