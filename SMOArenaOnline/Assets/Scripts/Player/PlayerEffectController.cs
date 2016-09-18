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
        isEnabled = false;
        for (int i = 0; i < effectsToDisable.Length; i++)
        {
            if (effectsToDisable[i].isPlaying)
            {
                effectsToDisable[i].Pause();

                effectsToDisable[i].enableEmission = false;
                Debug.Log("Disabled");
            }
            else
            {
                Debug.Log("Not playing");
            }
        }
        for (int i = 0; i < lightToDisable.Length; i++)
        {
            lightToDisable[i].enabled = false;
        }

    }
    /*
    public void enableEffect()
    {
        isEnabled = true;
        for (int i = 0; i < effectsToDisable.Length; i++)
        {
            Debug.Log("Enabled");
            effectsToDisable[i].enableEmission = true;
        }
        for (int i = 0; i < lightToDisable.Length; i++)
        {
            lightToDisable[i].enabled = true;
        }
    }*/
    public bool getEffectActivity()
    {
        return isEnabled;
    }
}
