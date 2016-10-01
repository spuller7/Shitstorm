using UnityEngine;
using System.Collections;

public class PlayerEffectController : MonoBehaviour {

    [SerializeField]
    ParticleSystem[] effectsToDisable;
    [SerializeField]
    private bool isEnabled = false;
    [SerializeField]
    Light[] lightToDisable;

    void Start()
    {
        disableEffect();
    }

    public void disableEffect(){
        if (isEnabled == true)
        {
            isEnabled = false;
            for (int i = 0; i < effectsToDisable.Length; i++)
            {
                ParticleSystem ps = effectsToDisable[i];
                ParticleSystem.EmissionModule em = ps.emission;
                em.enabled = false;

            }
            for (int i = 0; i < lightToDisable.Length; i++)
            {
                lightToDisable[i].enabled = false;
            }
        }
    }
    
    public void enableEffect()
    {
        if (isEnabled == false)
        {
            isEnabled = true;
            for (int i = 0; i < effectsToDisable.Length; i++)
            {
                ParticleSystem ps = effectsToDisable[i];
                ParticleSystem.EmissionModule em = ps.emission;
                em.enabled = true;

            }
            for (int i = 0; i < lightToDisable.Length; i++)
            {
                lightToDisable[i].enabled = true;
            }
        }
    }
    public bool getEffectActivity()
    {
        return isEnabled;
    }
    //when particle effect is one shot
    public void onShotTrigger()
    {
        StartCoroutine(shot());
        for (int i = 0; i < effectsToDisable.Length; i++)
        {
            effectsToDisable[i].Emit(1);
        }

    }
    IEnumerator shot()
    {
            lightToDisable[0].enabled = true;
            yield return new WaitForSeconds(.15f);
            lightToDisable[0].enabled = false;
        
    }
}
