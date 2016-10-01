using UnityEngine;
using System.Collections;

public class PlayerEffects : MonoBehaviour {

    public bool isBoosting;
    public PlayerEffectController boostEffect;
    [SerializeField]
    private GameObject boostEffectGameobject;
    [SerializeField]
    private GameObject gunFlareEffectGameobject;
    [SerializeField]
    private GameObject bloodSplatterGameobject;

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
              boostEffect.enableEffect();
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
    public void setBoost(bool isBoosting)
    {
        this.isBoosting = isBoosting;
    }
    public void onArrayHit()
    {
        PlayerEffectController pEC = bloodSplatterGameobject.GetComponent<PlayerEffectController>();
        pEC.onShotTrigger();
    }
    public void onShoot()
    {
        
        gunFlareEffectGameobject.GetComponent<PlayerEffectController>().onShotTrigger();
    }
}