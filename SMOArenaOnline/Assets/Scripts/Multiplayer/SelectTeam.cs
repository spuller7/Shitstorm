using UnityEngine;
using System.Collections;

public enum Team
{
    //Buccanners
    b,
    //GreatBritain
    gb,
    None,
}

/// <summary>
/// This class draws the team selection menu from the beginning of a match
/// </summary>
public class SelectTeam : MonoBehaviour
{
    public Font ButtonFont;
    public Texture2D ButtonBackground;

    GUIStyle m_PickButtonStyle;

    void Start()
    {
        if (GamemodeManager.CurrentGamemode.IsUsingTeams() == false)
        {
            ChooseTeam(Team.None);
        }
    }

    void Update()
    {
        if (GamemodeManager.CurrentGamemode.IsRoundFinished() == true)
        {
            return;
        }
    }

    void ChooseAnyTeam()
    {
        int numberOfRedTeamPlayers = 0;
        int numberOfBlueTeamPlayer = 0;

        GameObject[] shipObjects = GameObject.FindGameObjectsWithTag("Ship");

        for (int i = 0; i < shipObjects.Length; ++i)
        {
            Ship ship = shipObjects[i].GetComponent<Ship>();

            if (ship != null)
            {
                if (ship.Team == Team.gb)
                {
                    numberOfRedTeamPlayers++;
                }
                else if (ship.Team == Team.b)
                {
                    numberOfBlueTeamPlayer++;
                }
            }
        }

        if (numberOfRedTeamPlayers > numberOfBlueTeamPlayer)
        {
            ChooseTeam(Team.b);
        }
        else
        {
            ChooseTeam(Team.gb);
        }
    }

    void ChooseTeam(Team team)
    {
        GetComponent<Spawn>().CreateLocalPlayer(team);
        enabled = false;
    }

    void OnGUI()
    {
        if (GamemodeManager.CurrentGamemode.IsRoundFinished() == true)
        {
            return;
        }

        LoadStyles();

        GUILayout.BeginArea(new Rect(10, 10, Screen.width - 20, Screen.height - 20));
        {
            GUILayout.BeginHorizontal();
            {
                if (GUILayout.Button(GetButtonLabel(Team.b), m_PickButtonStyle, GUILayout.Width(Screen.width * 0.5f - 20), GUILayout.Height(Screen.height - 140)))
                {
                    ChooseTeam(Team.gb);
                }

                GUILayout.FlexibleSpace();

                if (GUILayout.Button(GetButtonLabel(Team.gb), m_PickButtonStyle, GUILayout.Width(Screen.width * 0.5f - 20), GUILayout.Height(Screen.height - 140)))
                {
                    ChooseTeam(Team.b);
                }
            }
            GUILayout.EndHorizontal();

            GUILayout.Space(20);

            if (GUILayout.Button("Choose random team", m_PickButtonStyle, GUILayout.Width(Screen.width - 40), GUILayout.Height(100)))
            {
                ChooseAnyTeam();
            }
        }
        GUILayout.EndArea();
    }

    string GetButtonLabel(Team team)
    {
        GameObject[] shipObjects = GameObject.FindGameObjectsWithTag("Ship");
        int playerCount = 0;

        for (int i = 0; i < shipObjects.Length; ++i)
        {
            if (shipObjects[i].GetComponent<Ship>() != null && shipObjects[i].GetComponent<Ship>().Team == team)
            {
                playerCount++;
            }
        }

        string label = team.ToString() + " Team\n";
        label += playerCount.ToString();

        if (playerCount == 1)
        {
            label += " Player";
        }
        else
        {
            label += " Players";
        }

        return label;
    }

    void LoadStyles()
    {
        if (m_PickButtonStyle == null)
        {
            m_PickButtonStyle = new GUIStyle(Styles.Button);
            m_PickButtonStyle.fontSize = 60;
        }
    }
}