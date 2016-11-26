using UnityEngine;
using System.Collections;

public class Score : MonoBehaviour {

    [SerializeField]
    private int teamOneScore;
    private int teamTwoScore;

    void Start () {
        teamOneScore = 0;
        teamTwoScore = 0;
	}
    [PunRPC]
    public void changeScore(Team team)
    {
        if(team == Team.blue)
        {
            teamOneScore = teamOneScore++;
        }
        else if(team == Team.red)
        {
            teamTwoScore = teamTwoScore++;
        }
        else
        {
            Debug.LogError("Unknown Team: Can't Change Score");
        }
    }
}
