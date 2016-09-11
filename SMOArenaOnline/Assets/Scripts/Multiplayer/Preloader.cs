using UnityEngine;
using System.Collections;

public class Preloader : Photon.MonoBehaviour
{
    public GUISkin skin;

    void Update()
    {

        //Wait for both mainmenu&game scene to be loaded (since mainmenu doesnt check on game scene)
        if (Application.GetStreamProgressForLevel(1) >= 1 && Application.GetStreamProgressForLevel(2) >= 1)
        {
            //After loading, go to next login level
            Application.LoadLevel(1);
        }
        else
        {
            Debug.Log("Error load levels");
        }

    }

    void OnGUI()
    {
        GUI.skin = skin;

        GUI.Label(new Rect(Screen.width / 2 - 70, Screen.height / 2 - 12, 140, 25), "Loading: " + (int)(Application.GetStreamProgressForLevel(1) * 100) + "%");
    }

}
