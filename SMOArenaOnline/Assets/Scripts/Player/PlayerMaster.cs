using UnityEngine;
using System.Collections;


public enum Team
{
    blue,
    red,
    none
}
public class PlayerMaster : Photon.MonoBehaviour {

    private static PlayerMaster LocalPlayer;
    private bool IsLocalPlayer = true;

    /// <summary>
    /// Big explosion that gets instantiated when the ship dies
    /// </summary>
    [SerializeField]
    public GameObject ExplosionPrefab;
    [SerializeField]
    private float m_Health = 50;
    
    public float Health
    {
        get
        {
            return m_Health;
        }
    }
    Team m_Team;

    /// <summary>
    /// Each player is either in the red or in the blue team
    /// </summary>
    /// <value>
    /// The team this ship belongs to
    /// </value>
    public Team Team
    {
        get
        {
            return m_Team;
        }
    }

    public void SetTeam(Team team)
    {
        //This method gets called right after a ship is created

        m_Team = team;
    }

    public void DealDamage(float damage, PlayerMaster damageDealer)
    {
        m_Health -= damage;

        OnHealthChanged(damageDealer);
    }

    public void SendHeal()
    {
        if (PhotonNetwork.offlineMode == true)
        {
            OnHeal();
        }
        else
        {
            if (photonView.isMine == true)
            {
                photonView.RPC("OnHeal", PhotonTargets.All);
            }
        }
    }

    void OnHealthChanged()
    {
        OnHealthChanged(null);
    }

    void OnHealthChanged(PlayerMaster damageDealer)
    {
        if (m_Health <= 0)
        {
            m_Health = 0;
            Instantiate(ExplosionPrefab, transform.position, Quaternion.identity);

            //If our local ship dies, call the respawn function after 2 seconds and award
            //the kill to the damage dealer
            if (photonView.isMine == true)
            {
                if (damageDealer != null)
                {
                   // damageDealer.OnKilledShip(this);
                }

                Invoke("SendRespawn", 2f);
            }
        }

        //The ShipVisuals component shows the smoke when the ship is damaged
    }

    [PunRPC]
    void OnHeal()
    {
        m_Health = 50;
        OnHealthChanged();
    }

    [PunRPC]
    void OnRespawn(Vector3 spawnPosition, Quaternion spawnRotation)
    {
        transform.position = spawnPosition;
        transform.rotation = spawnRotation;


        m_Health = 50;
        OnHealthChanged();

        //The OnRespawn functions of other components make sure that all values are properly reset to their initial values again
       // ShipMovement.OnRespawn(spawnRotation.eulerAngles.y);

    }

    void OnPhotonInstantiate(PhotonMessageInfo info)
    {
        //This method gets called right after a GameObject is created through PhotonNetwork.Instantiate
        //The fifth parameter in PhotonNetwork.instantiate sets the instantiationData and every client
        //can access them through the PhotonView. In our case we use this to send which team the ship
        //belongs to. This methodology is very useful to send data that only has to be sent once.

        if (photonView.isMine == false)
        {
            SetTeam((Team)photonView.instantiationData[0]);
        }
    }

    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        //Multiple components need to synchronize values over the network.
        //The SerializeState methods are made up, but they're useful to keep
        //all the data separated into their respective components

        SerializeState(stream, info);
       // ShipMovement.SerializeState(stream, info);
    }

    void SerializeState(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting == true)
        {
            stream.SendNext(m_Health);
        }
        else
        {
            float oldHealth = m_Health;
            m_Health = (float)stream.ReceiveNext();

            if (m_Health != oldHealth)
            {
                OnHealthChanged();
            }
        }
    }
}
