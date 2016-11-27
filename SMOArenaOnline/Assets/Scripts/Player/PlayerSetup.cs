using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class PlayerSetup : NetworkBehaviour {

    [SerializeField]
    Behaviour[] componentsToDisable;
    public gamemode gamemode;

    void Start()
    {
        gamemode = Camera.main.GetComponent<gamemode>();

        //ensure player has the right tag for script management
        gameObject.tag = "Player";

        //disable scripts that other players can't control
        if (gamemode.getGameMode() != 1)    
        {
            if (!isLocalPlayer)
            {
                Debug.Log("disabled");
                for (int i = 0; i < componentsToDisable.Length; i++)
                {
                    componentsToDisable[i].enabled = false;
                }
            }
        }
    }
}
