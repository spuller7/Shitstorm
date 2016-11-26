using UnityEngine;
using System.Collections;

public class gamemode : MonoBehaviour {

    [SerializeField]
    private bool isLocalGame;

    private int gameModeInt;
    //local = 1
    //mp = 2
    void Start() {
        if (isLocalGame)
        {
            gameModeInt = 1;
        }
    }

    public int getGameMode()
    {
        return gameModeInt;
    }
}
