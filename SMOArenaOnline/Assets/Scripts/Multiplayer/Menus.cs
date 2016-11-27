using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Menus : Photon.MonoBehaviour {

    //initial gui set up
    public Texture backgroundTexture;
    public GUISkin skin;
    private Vector2 scrollPos = Vector2.zero;
    public GUIStyle nullStyle;
    private string currentUI = "Main Menu";
    float screenWidth = 960;

    //Game networking/status
    private string serverIP = "angrybots.exitgames.com";
    private int serverPort = 5055;
    private string defaultAppId = "AngryBotsDemo";
    private string roomName = PhotonNetwork.playerName + "'s Room";
    private bool overridePhotonConfigFile = false;
    private string gameVersion = "Version: Alpha 1.0";

    private bool photonConnectionFailed = false;
    int totalPlayers = 0;   // sum of players in all listed rooms

    void Awake()
    {
        //PhotonNetwork.logLevel = NetworkLogLevel.Full;

        //Connect to the main photon server. This is the only IP and port we ever need to set(!)
        if (!PhotonNetwork.connected)
            PhotonNetwork.ConnectUsingSettings(this.gameVersion); // the game's version. can be used to separate players with older and newer client

    }
    void OnJoinedRoom()
    {
        PhotonNetwork.isMessageQueueRunning = false;
        PhotonNetwork.automaticallySyncScene = true;
        PhotonNetwork.LoadLevel("Map1");
        
    }

    void OnGUI()
    {
        screenWidth = Mathf.Min(Screen.width, 960);

        GUI.skin = skin;
        GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), backgroundTexture, ScaleMode.StretchToFill);
        
        GUI.Label(new Rect(Screen.width - 200, 0, 200, Screen.height), PhotonNetwork.playerName);

        if (!Application.CanStreamedLevelBeLoaded(3) || Application.GetStreamProgressForLevel(3) < 1)
        {
            Debug.Log("Working?");
            GameGUI();
            return;
        }
        
        if (!PhotonNetwork.connected) {
            ConnectGUI();
        }
           
        else if (PhotonNetwork.room == null)
        {
            if (currentUI == "Main Menu")
            {
                MainMenuGUI();
            }
            else if (currentUI == "Multiplayer")
            {
                MultiplayerGUI();
            }
        }

        else
        {
            GameGUI();
        }   
    }
    void MainMenuGUI()
    {
        GUI.Button(new Rect(100, 10, 200, 40), "Main Menu");
        if(GUI.Button(new Rect(350, 10, 200, 40), "Multiplayer"))
        {
            currentUI = "Multiplayer";
        }
    }
    void MultiplayerGUI()
    {
        if(GUI.Button(new Rect(100, 10, 200, 40), "Main Menu"))
        {
            currentUI = "Main Menu";
        }
        if (GUI.Button(new Rect(350, 10, 200, 40), "Multiplayer"))
        {
            currentUI = "Multiplayer";
        }

        float thisW = screenWidth;
        float thisH = Mathf.Min(640, Screen.height);
        GUILayout.BeginArea(new Rect((Screen.width - thisW) / 2, (Screen.height - thisH) / 2, thisW, thisH));

        float leftWindowPos = Mathf.Max(0, Screen.width - screenWidth);

        GUI.Window(0, new Rect(leftWindowPos / 2 + 200, 85, screenWidth - 200, thisH - 160), RoomBrowser, "Room listing");

        //Left bar
        if (PhotonNetwork.GetRoomList().Length > 0)
        {
            if (GUI.Button(new Rect(14, 150, 180, 20), "Join random"))
            {
                PhotonNetwork.JoinRandomRoom();
            }
        }
        GUI.Label(new Rect(14, 190, 180, 50), "Players: " + totalPlayers + "\nRooms: " + PhotonNetwork.GetRoomList().Length + "");
        GUILayout.EndArea();
    }

    void ConnectGUI()
    #region Connect to servers once logged in
    {
        GUILayout.BeginArea(new Rect((Screen.width - 400) / 2, (Screen.height - 300) / 2, 400, 300));
        GUILayout.Label("Connecting to the Photon server..");
        GUILayout.Label("See \"Photon Unity Networking/readme.txt\" for installation instructions.");
        if (photonConnectionFailed)
        {
            GUILayout.Label("Connection to Photon Failed.");
            GUILayout.Label("Possible reasons:");
            GUILayout.Label("-No internet connection");
            GUILayout.Label("-Wrong hostname");
        }
        GUILayout.Space(10);

        GUILayout.BeginHorizontal();
        if (overridePhotonConfigFile)
        {
            serverIP = GUILayout.TextField(serverIP, GUILayout.Width(120));
            serverPort = int.Parse("0" + GUILayout.TextField(serverPort.ToString(), GUILayout.Width(60)));
            if (GUILayout.Button("Retry", GUILayout.Width(75)))
            {
                PhotonNetwork.ConnectToMaster(serverIP, serverPort, defaultAppId, this.gameVersion);
                photonConnectionFailed = false;
            }
        }
        else
        {
            if (GUILayout.Button("Retry", GUILayout.Width(75)))
            {
                PhotonNetwork.ConnectUsingSettings(this.gameVersion);
                photonConnectionFailed = false;
            }
        }

        GUILayout.EndHorizontal();

        GUILayout.EndArea();
    }
    #endregion

    void RoomBrowser(int id)
    #region Display rooms available to join
    {
        //Join room by title
        GUILayout.BeginHorizontal();
        GUILayout.Label("Create room:", GUILayout.Width(150));
        roomName = GUILayout.TextField(roomName, GUILayout.Width(150));
        if (GUILayout.Button("GO", GUILayout.Height(20), GUILayout.Width(40)))
        {
            PhotonNetwork.JoinOrCreateRoom(roomName, new RoomOptions() { maxPlayers = 4 }, TypedLobby.Default);
            Debug.Log("Created room: " + roomName);
            Debug.Log(PhotonNetwork.GetRoomList().Length);
        }
        GUILayout.EndHorizontal();

        GUILayout.Space(25);
        if (PhotonNetwork.GetRoomList().Length == 0)
        {
            GUILayout.Label("No games running at the moment.\nYou can wait or create a game yourself.");
        }
        else
        {
            //Room listing: simply call GetRoomList: no need to fetch/poll whatever!

            GUILayout.BeginHorizontal();
            GUILayout.Space(110);
            GUILayout.Label("Players", GUILayout.Width(110));
            GUILayout.Label("Title");

            GUILayout.EndHorizontal();


            scrollPos = GUILayout.BeginScrollView(scrollPos);
            foreach (RoomInfo game in PhotonNetwork.GetRoomList())
            {
                string maxP = game.playerCount + "/" + game.maxPlayers;
                if (game.maxPlayers < 1) maxP = "" + game.playerCount;

                GUILayout.BeginHorizontal();
                GUILayout.Space(10);
                if (GUILayout.Button("JOIN", GUILayout.Width(100)))
                {
                    PhotonNetwork.JoinRoom(game.name);
                }
                GUILayout.Label("    " + maxP, GUILayout.Width(110));
                GUILayout.Label(game.name);

                GUILayout.EndHorizontal();
            }
            GUILayout.EndScrollView();
        }
    }
    #endregion

    void OnFailedToConnectToPhoton()
    {
        photonConnectionFailed = true;
    }

    void OnReceivedRoomListUpdate()
    {
        //Recalculate playercount
        totalPlayers = 0;
        RoomInfo[] roomList = PhotonNetwork.GetRoomList();
        foreach (RoomInfo game in roomList)
        {
            totalPlayers += game.playerCount;
        }
    }
    void GameGUI()
    {
        GUI.Label(new Rect(Screen.width / 2 - 70, Screen.height / 2 - 12, 250, 50), "      Loading: " + (int)(Application.GetStreamProgressForLevel(3) * 100) + "%\n\nThis will take a minute");
        Debug.Log("Building Map");
    }
}
