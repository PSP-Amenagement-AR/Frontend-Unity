using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SimpleJSON;

public class UserController : MonoBehaviour
{
    public GameObject popup;
    public Text popupText;
    public ScreenHandler canvas;

    // Buttons
    public Button logoutButton;
    public Button ProfilButton;
    public Button submitButton;
    public Button loginButton;
    public Button updateButton;

    // Login fields
    public InputField loginMailField;
    public InputField loginPasswordField;

    // Registration fields
    public InputField firstnameField;
    public InputField lastnameField;
    public InputField mailField;
    public InputField passwordField;
    public InputField confirmedPasswordField;

    // Profil page fields (update)
    public InputField firstnameProfilField;
    public InputField lastnameProfilField;
    public InputField mailProfilField;
    public InputField passwordProfilField;

   //APIrequestManager webApi = new APIrequestManager();

    private void Awake()
    {
        submitButton.onClick.AddListener(() =>
        {
            if (RegistrationCheckInformations())
            {
                try
                {
                    var sendRequest = Registration();
                    if (sendRequest != null)
                    {
                        canvas.CloseRegistration();
                        canvas.OpenLogin();
                    }
                }
                catch (Exception e)
                {
                    Debug.Log("Error : " + e.Message);
                }
            }
        });

        loginButton.onClick.AddListener(() =>
        {
            if (LoginCheckInformations())
            { 
                try
                {
                    if (GlobalStatus.token != "")
                    {
                        InitPopup("Please disconnect the actual account before reconnect");
                        Debug.Log("token already exist : " + GlobalStatus.token);
                    } else
                    {
                        var sendRequest = Login();
                        if (sendRequest != null)
                        {
                            canvas.CloseLogin();

                            GlobalStatus.token = sendRequest["token"].Value;
                            GlobalStatus.firstname = sendRequest["firstName"].Value;
                            GlobalStatus.lastname = sendRequest["lastName"].Value;

                            ProfilButton.gameObject.SetActive(true);
                            Debug.Log("token : " + GlobalStatus.token);

                            firstnameProfilField.text = GlobalStatus.firstname;
                            lastnameProfilField.text = GlobalStatus.lastname;
                            mailProfilField.text = loginMailField.text.ToString();
                            passwordProfilField.text = loginPasswordField.text.ToString();
                        }
                    }
                }
                catch (Exception e)
                {
                    Debug.Log("Error : " + e.Message);
                }
            }
        });

        logoutButton.onClick.AddListener(() => 
        {
            var sendRequest = Logout();
            canvas.CloseProfil();
            ProfilButton.gameObject.SetActive(false);
        });

        updateButton.onClick.AddListener(() =>
        {
            var sendRequest = UpdateUser();
            canvas.CloseProfil();
        });
    }

    void Start()
    {

    }

    void Update()
    {
        
    }

    JSONNode Registration()
    {
        var request = GlobalStatus.webApi.SendApiRequest("/users/register", "POST", new Users { email = mailField.text.ToString(), password = passwordField.text.ToString(), firstName = firstnameField.text.ToString(), lastName = lastnameField.text.ToString() });
        //var request = webApi.SendApiRequest("/users/register", "POST", new Users(mailField.text.ToString(), passwordField.text.ToString(), firstnameField.text.ToString(), lastnameField.text.ToString()));

        JSONNode dataJSON = JSON.Parse(request.downloadHandler.text);

        if (request.responseCode == 200 || request.responseCode == 201)
            return dataJSON;
        else if (request.responseCode == 0)
        {
            InitPopup("No connection ...");
            return null;
        }
        else if (request.responseCode == 400)
        {
            InitPopup("This account already exist");
            return null;
        }
        else
            return null;
    }

    JSONNode Login()
    {
        var request = GlobalStatus.webApi.SendApiRequest("/users/login", "POST", new Users { email = loginMailField.text.ToString(), password = loginPasswordField.text.ToString() });
        //var request = webApi.SendApiRequest("/users/login", "POST", new Users ( loginMailField.text.ToString(), loginPasswordField.text.ToString() ));
        JSONNode dataJSON = JSON.Parse(request.downloadHandler.text);

      if (request.responseCode == 200 || request.responseCode == 201)
            return dataJSON;
       else if (request.responseCode == 404)
       {
           InitPopup("The credentials are incorrect");
           return null;
       }
       else if (request.responseCode == 0)
       {
           InitPopup("No connection ...");
           return null;
       }
       else
           return null;
    }

    JSONNode Logout()
    {
        var request = GlobalStatus.webApi.SendApiRequest("/users/disconnect", "GET");
        JSONNode dataJSON = JSON.Parse(request.downloadHandler.text);

        if (request.responseCode == 204)
        {
            InitPopup("Disconnected");
            GlobalStatus.token = "";
            GlobalStatus.firstname = "";
            GlobalStatus.lastname = "";
            Debug.Log("Bye : " + GlobalStatus.token);
            return dataJSON;
        } else
        {
            InitPopup("Error connection");
            return null;
        }
        
    }

    JSONNode UpdateUser()
    {
        var request = GlobalStatus.webApi.SendApiRequest("/users", "PUT", new Users { email = mailProfilField.text.ToString(), password = passwordProfilField.text.ToString(), firstName = firstnameProfilField.text.ToString(), lastName = lastnameProfilField.text.ToString() });

        JSONNode dataJSON = JSON.Parse(request.downloadHandler.text);
        Debug.Log(request.downloadHandler.text);
        return dataJSON;
    }

    bool RegistrationCheckInformations()
    {
        bool created;
        if ((firstnameField.text.ToString() != "") && (lastnameField.text.ToString() != "") &&
                (mailField.text.ToString() != "") && (passwordField.text.ToString() != "") && (confirmedPasswordField.text.ToString() != ""))
        {
            if ((mailField.text.ToString().Contains("@") == false) || (mailField.text.ToString().Contains(".com") == false))
            {
                InitPopup("The mail is not correct");
                created = false;
            }
            else if(passwordField.text.ToString() != confirmedPasswordField.text.ToString())
            {
                InitPopup("Not the same password");
                created = false;
            }
            else if (passwordField.text.ToString().Length < 9)
            {
                InitPopup("Password too short");
                created = false;
            }
            else
            {
                InitPopup("User created");
                created = true;
            }
        }
        else
        {
            InitPopup("One or many field(s) are missing");
            created = false;
        }
        return created;
    }

    bool LoginCheckInformations()
    {
        bool connected;
        if ((loginMailField.text.ToString() != "") && (loginPasswordField.text.ToString() != ""))
        {
            if ((loginMailField.text.ToString().Contains("@") == false) || (loginMailField.text.ToString().Contains(".com") == false))
            {
                InitPopup("The mail is not correct");
                connected = false;
            }
            else
            {
                InitPopup("Connected");
                connected = true;
            }
        }
        else
        {
            InitPopup("One or many field(s) are missing");
            connected = false;
        }
        return connected;
    }

    void InitPopup(string str)
    {
        popupText.text = str;
        if (str != "Connected")
        {
            StartCoroutine(PopupText());
        } 
    }

    IEnumerator PopupText()
    {
        popup.SetActive(true);
        yield return new WaitForSeconds(2);
        popup.SetActive(false);
    }
}

public class Users
{
    public string email;
    public string password;
    public string firstName;
    public string lastName;

    /*public Users(string email, string password, string firstname, string lastname)
    {
        this.email = email;
        this.password = password;
        this.firstName = firstname;
        this.lastName = lastname;
    }

    public Users(string email, string password)
    {
        this.email = email;
        this.password = password;
    }

    public Users(string password)
    {
        this.password = password;
    }*/
}
