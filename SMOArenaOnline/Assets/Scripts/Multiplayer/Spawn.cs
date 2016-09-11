using UnityEngine;
using System.Collections;

/// <summary>
/// Creates the synchronized ship objects
/// </summary>
public class Spawn : MonoBehaviour
{
    void Start()
    {
        //if we are not connected, than we probably pressed play in a level in editor mode.
        //In this case go back to the main menu to connect to the server first
        if (PhotonNetwork.connected == false)
        {
            Application.LoadLevel(0);
            return;
        }
    }

    public void CreateLocalPlayer(Team team)
    {
        object[] instantiationData = new object[] { (int)team };

        //Notice the differences from PhotonNetwork.Instantiate to Unitys GameObject.Instantiate

        Transform spawnPoint = GamemodeManager.CurrentGamemode.GetSpawnPoint(team);
   
        GameObject newShipObject = PhotonNetwork.Instantiate(
            "Ship Player",
            spawnPoint.transform.position,
            spawnPoint.transform.rotation,
            0,
            instantiationData
        );


        Ship newShip = newShipObject.GetComponent<Ship>();
        newShip.SetTeam(team);
        //Since this function is called on every machine to create it's one and only local player, the new ship is always the camera target
        GameObject.Find("Camera").SetActive(false);
    }

}
