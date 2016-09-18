using UnityEngine;
using System.Collections;

public class PlayerEffects : MonoBehaviour {

    public bool isBoosting;
    public PlayerEffectController boostEffect;
    [SerializeField]
    private GameObject boostEffectGameobject;
    [SerializeField]
    private GameObject gunFlareEffectGameobject;

    void Start()
    {
        boostEffect = boostEffectGameobject.GetComponent<PlayerEffectController>();
        isBoosting = false;
    }


    void Update()
    {
        handleBoost();
    }

    private void handleBoost()
    {
        if(isBoosting == true)
        {
            if(boostEffect.getEffectActivity() == false)
            {
              //  boostEffect.enableEffect();
            }
        }
        else
        {
            if(boostEffect.getEffectActivity() == true)
            {
                boostEffect.disableEffect();
            }
        }
    }
}