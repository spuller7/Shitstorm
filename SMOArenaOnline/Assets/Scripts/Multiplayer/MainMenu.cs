using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MainMenu : Photon.MonoBehaviour {


    int totalPlayers = 0;   // sum of players in all listed rooms
    float screenWidth = 960;

    private string createAccountUrl = "http://127.0.0.1/CreateAccountT.php";
    private string LoginUrl = "http://127.0.0.1/LoginAccount.php";
    public static string Email = "";
    public static string Password = "";
    public static string Alias = "";
    public static string cPassword = "";
    public static string cEmail = "";
    public static string ConfirmPassword = "";
    public static string ConfirmEmail = "";


    private Vector2 scrollPos = Vector2.zero;
    public GUIStyle nullStyle;
    public GUISkin skin;
    private string currentUI = "loginGUI";

    public string stringToEdit = "Hello World";
    public GameObject mainImage;
    public Texture backgroundTexture;
    public Texture bgLogin;
    public Texture bLogin;
    public Texture ca;
    public Texture title;

   

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    void OnGUI()
    {
        GUI.skin = skin;
        GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), backgroundTexture, ScaleMode.StretchToFill);
        GUI.DrawTexture(new Rect(Screen.width / 2 - 275, 130, 550, 550), bgLogin);

        GUI.DrawTexture(new Rect(Screen.width / 2 - 153, 175, 306, 128), title);

        screenWidth = Mathf.Min(Screen.width, 960);
    
            if (currentUI == "loginGUI")
            {
                loginGUI();
            }
            else if (currentUI == "createAccountUI")
            {
                createAccountGUI();
            }
      
    }

   
    void loginGUI()
    {

        if(GUI.Button(new Rect(Screen.width / 2 - 50, 520, 100, 38), ca))
        {
            currentUI = "createAccountUI";
        }
        //login
        if(GUI.Button(new Rect(Screen.width / 2 , 600, 256, 128), bLogin)){
            StartCoroutine("LogInAccount");
        }
        //Quit Game
        GUI.Button(new Rect(Screen.width / 2 - 256, 600, 256, 128), bLogin);

        Email = GUI.TextField(new Rect(Screen.width / 2 - 200, 370, 400, 50), Email, 25);
        Password = GUI.TextField(new Rect(Screen.width / 2 - 200, 440, 400, 50), Password);
        
        GUI.Label(new Rect(Screen.width / 2 - 20, 320, 350, 50), "Login:");
        GUI.Label(new Rect(Screen.width / 2 - 190, 350, 350, 50), "Email:");
        GUI.Label(new Rect(Screen.width / 2 - 190, 420, 350, 50), "Password:");
    }
    void createAccountGUI()
    {
       
        Alias = GUI.TextField(new Rect(Screen.width / 2 - 200, 325, 400, 50), Alias, 25);
        cPassword = GUI.TextField(new Rect(Screen.width / 2 - 200, 385, 400, 50), cPassword, 25);
        ConfirmPassword = GUI.TextField(new Rect(Screen.width / 2 - 200, 445, 400, 50), ConfirmPassword, 25);
        cEmail = GUI.TextField(new Rect(Screen.width / 2 - 200, 505, 400, 50), cEmail);
        ConfirmEmail = GUI.TextField(new Rect(Screen.width / 2 - 200, 565, 400, 50), ConfirmEmail);

        //Create Account
        if (GUI.Button(new Rect(Screen.width / 2, 600, 256, 128), bLogin))
        {
            if (ConfirmPassword == cPassword && ConfirmEmail == cEmail)
            {
                StartCoroutine("CreateAccount");
            }
            else
            {
               if(ConfirmPassword != cPassword)
                {
                    Debug.Log("Passwords are not the same");
                }
                else
                {
                    Debug.Log("Emails are not the same");
                }
            }
        }
        //Back
        if (GUI.Button(new Rect(Screen.width / 2 - 256, 600, 256, 128), bLogin))
        {
            currentUI = "loginGUI";
        }
        GUI.Label(new Rect(Screen.width / 2 - 65, 290, 350, 50), "Create Account:");
        GUI.Label(new Rect(Screen.width / 2 - 190, 310, 350, 50), "Alias:");
        GUI.Label(new Rect(Screen.width / 2 - 190, 370, 350, 50), "Password:");
        GUI.Label(new Rect(Screen.width / 2 - 190, 430, 350, 50), "Confirm Password:");
        GUI.Label(new Rect(Screen.width / 2 - 190, 490, 350, 50), "Email:");
        GUI.Label(new Rect(Screen.width / 2 - 190, 550, 350, 50), "Confirm Email:");

    }
    IEnumerator CreateAccount()
    {
        WWWForm form = new WWWForm();
        form.AddField("Email", cEmail);
        form.AddField("Password", cPassword);
        form.AddField("Alias", Alias);
        WWW createAccountWWW = new WWW(createAccountUrl, form);
        yield return createAccountWWW;
        if(createAccountWWW.error != null)
        {
            Debug.LogError("Cannot Create Account");
        }
        else
        {
            string createAccountReturn = createAccountWWW.text;
            if(createAccountReturn == "Success")
            {
                Debug.Log("Account has been created");
                currentUI = "loginGUI";
            }
        }
    }
    IEnumerator LogInAccount()
    {
        WWWForm Form = new WWWForm();
        Form.AddField("Email", Email);
        Form.AddField("Password", Password);
        WWW LoginWWW = new WWW(LoginUrl, Form);
        yield return LoginWWW;
        if (LoginWWW.error != null) {
            Debug.LogError("Cannot connect to login");
        }
        else
        {
            string logText = LoginWWW.text;
            Debug.Log(logText);
            string[] logTextSplit = logText.Split(':');
            if (logTextSplit[0] == "Success")
            {
                Debug.Log("Login Succesful");
                Debug.Log("Welcome " + logTextSplit[1]);
                //Save name
                PhotonNetwork.playerName = logTextSplit[1];           
                Application.LoadLevel(Application.loadedLevel + 1);
            }
            
        }

    }


}