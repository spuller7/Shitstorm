using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Login : Photon.MonoBehaviour
{ 
    int totalPlayers = 0;   // sum of players in all listed rooms
    float screenWidth = 960;

    //Insert php urls
    public string createAccountUrl = "http://127.0.0.1/CreateAccountT.php";
    public string LoginUrl = "http://127.0.0.1/LoginAccount.php";

    public static string Email = "";
    public static string Password = "";
    public static string Alias = "";
    public static string cPassword = "";
    public static string cEmail = "";
    public static string ConfirmPassword = "";
    public static string ConfirmEmail = "";

    public GameObject createAccountGUI;
    public GameObject loginGUI;
    public Text errorCreateAccountText;
    public Text errorLoginText;

    private Vector2 scrollPos = Vector2.zero;
    private string currentUI = "loginGUI";

    public string stringToEdit = "Hello World";
    public GameObject mainImage;
    public Texture backgroundImage;
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

    public void getAlias(string input)
    {
        Alias = input;
        Debug.Log("Alias is now " + input);
    }
    public void getPassword(string input)
    {
        Password = input;
        Debug.Log("Password is now ******");
    }
    public void getCreatePassword(string input)
    {
        cPassword = input;
        Debug.Log("Registering password as *****");
    }
    public void getConfirmPassword(string input)
    {
        ConfirmPassword = input;
    }
    public void getEmail(string input)
    {
        Email = input;
        Debug.Log("Email is now " + input);
    }
    public void getCreateEmail(string input)
    {
        cEmail = input;
        Debug.Log("Registering email as " + input);
    }
    public void getConfirmEmail(string input)
    {
        ConfirmEmail = input;
    }

    void OnGUI()
    {
        GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), backgroundImage, ScaleMode.StretchToFill);
  
        GUI.DrawTexture(new Rect(Screen.width / 2 - 153, 175, 306, 128), title);

        screenWidth = Mathf.Min(Screen.width, 960);
    }

    public void switchUI()
    {
        if(loginGUI.activeSelf == true)
        {
            loginGUI.SetActive(false);
            createAccountGUI.SetActive(true);
        }
        else
        {
            loginGUI.SetActive(true);
            createAccountGUI.SetActive(false);
        }
    }

    public void onLogin()
    {
        StartCoroutine("LogInAccount");
    }

    public void onCreateAccount()
    {

        if (ConfirmPassword == cPassword && ConfirmEmail == cEmail)
        {
            StartCoroutine("CreateAccount");
        }
        else
        {
            if (ConfirmPassword != cPassword)
            {
                    Debug.Log("Passwords are not the same");
                    errorCreateAccountText.text = "Passwords are not the same";
                }
                else
                {
                    Debug.Log("Emails are not the same");
                     errorCreateAccountText.text = "Emails are not the same";
            }
        }

    }
    IEnumerator CreateAccount()
    {
        WWWForm form = new WWWForm();
        form.AddField("Email", cEmail);
        form.AddField("Password", cPassword);
        form.AddField("Alias", Alias);
        WWW createAccountWWW = new WWW(createAccountUrl, form);
        yield return createAccountWWW;
        if (createAccountWWW.error != null)
        {
            Debug.LogError("Cannot Create Account");
        }
        else
        {
            string createAccountReturn = createAccountWWW.text;
            if (createAccountReturn == "Success")
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
        if (LoginWWW.error != null)
        {
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