using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SimpleJSON;

/// <summary>
/// Class for manage user controller.
/// </summary>
public class UserController : MonoBehaviour
{
    /// GameObject for popup interface.
    public GameObject popup;
    /// Text of popup interface.
    public Text popupText;
    /// ScreenHandler object for the canvas.
    /// @see ScreenHandler
    public ScreenHandler canvas;

    // Buttons
    /// Logout button.
    public Button logoutButton;
    /// Profil button.
    public Button ProfilButton;
    /// Submit button.
    public Button submitButton;
    /// Login button.
    public Button loginButton;
    /// Update button.
    public Button updateButton;
    /// Delete button.
    public Button deleteButton;
    /// Return from login button.
    public Button returnLoginButton;
    /// Return from registration button.
    public Button returnRegistrationButton;

    // Login fields
    /// Input filed for email in login interface.
    public InputField loginMailField;
    /// Input field for password in login interface.
    public InputField loginPasswordField;

    // Registration fields
    /// Input field for firstname in registration interface.
    public InputField firstnameField;
    /// Input field for lastname in registration interface.
    public InputField lastnameField;
    /// Input field for email in registration interface.
    public InputField mailField;
    /// Input field for password in registration interface.
    public InputField passwordField;
    /// Input field for confirm password in registration interface.
    public InputField confirmedPasswordField;

    // Profil page fields (update)
    /// Input field for firstname in profil interface.
    public InputField firstnameProfilField;
    /// Input field for lastname in profil interface.
    public InputField lastnameProfilField;
    /// Input field for email in profil interface.
    public InputField mailProfilField;
    /// Input field for password in profil interface.
    public InputField passwordProfilField;

    // User informations
    /// Firstname of the user connected.
    public string firstname = "";
    /// Lastname of the user connected.
    public string lastname = "";
    /// Id of the user connected.
    public string id = "";

    /// Initiate interfaces and buttons listener.
    /// @note Function executed when the script is started.
    public void Awake()
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
                        CleanFields(true);
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
                    } else
                    {
                        var sendRequest = Login();
                        if (sendRequest != null)
                        {
                            canvas.CloseLogin();

                            GlobalStatus.token = sendRequest["token"].Value;
                            this.firstname = sendRequest["firstName"].Value;
                            this.lastname = sendRequest["lastName"].Value;
                            this.id = sendRequest["id"].Value;
                            InitPopup("Welcome " + this.firstname + " " + this.lastname + " !");
                            ProfilButton.gameObject.SetActive(true);

                            firstnameProfilField.text = this.firstname;
                            lastnameProfilField.text = this.lastname;
                            mailProfilField.text = loginMailField.text.ToString();
                            passwordProfilField.text = loginPasswordField.text.ToString();

                            CleanFields(false);
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
            var sendRequest = UpdateInfos();
            canvas.CloseProfil();
        });

        deleteButton.onClick.AddListener(() =>
        {
            //var sendRequest = Deletion();
            canvas.CloseProfil();
        });

        returnLoginButton.onClick.AddListener(() =>
        {
            CleanFields(false);
        });

        returnRegistrationButton.onClick.AddListener(() =>
        {
            CleanFields(true);
        });
    }

    void Start()
    {

    }

    void Update()
    {
        
    }

    /// Send registration request to Back service.
    /// @see Users
    /// @returns JSON response or null if error.
    public JSONNode Registration()
    {
        var request = GlobalStatus.webApi.SendApiRequest("/users/register", "POST", new Users { email = mailField.text.ToString(), password = passwordField.text.ToString(), firstName = firstnameField.text.ToString(), lastName = lastnameField.text.ToString() });
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

    /// Send login request to Back service.
    /// @see Users
    /// @returns JSON response or null if error.
    public JSONNode Login()
    {
        var request = GlobalStatus.webApi.SendApiRequest("/users/login", "POST", new Users { email = loginMailField.text.ToString(), password = loginPasswordField.text.ToString() });
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

    /// Send logout request to Back service.
    /// @returns JSON response or null if error.
    public JSONNode Logout()
    {
        var request = GlobalStatus.webApi.SendApiRequest("/users/disconnect", "GET");
        JSONNode dataJSON = JSON.Parse(request.downloadHandler.text);

        if (request.responseCode == 204)
        {
            InitPopup("Disconnected");
            DeleteUserInformations();
            return dataJSON;
        } else
        {
            InitPopup("Error connection");
            return null;
        }
        
    }

    /// Send update user informations request to Back service.
    /// @see Users
    /// @returns JSON response or null if error.
    public JSONNode UpdateInfos()
    {
        var request = GlobalStatus.webApi.SendApiRequest("/users", "PUT", new Users { email = mailProfilField.text.ToString(), password = passwordProfilField.text.ToString(), firstName = firstnameProfilField.text.ToString(), lastName = lastnameProfilField.text.ToString() });
        JSONNode dataJSON = JSON.Parse(request.downloadHandler.text);

        if (request.responseCode == 200)
        {
            InitPopup("Informations updated");
            return dataJSON;
        } else
        {
            InitPopup("Error connection");
            return null;
        }
    }

    /// Send delete user request to Back service
    /// @returns JSON response or null if error.
    public JSONNode Deletion()
    {
        var url = "/users/" + this.id;
        var request = GlobalStatus.webApi.SendApiRequest(url, "DELETE");
        JSONNode dataJSON = JSON.Parse(request.downloadHandler.text);
        Debug.Log("id : " + this.id);
        Debug.Log("Deletion : " + request.responseCode);
        if (request.responseCode == 401)
        {
            InitPopup("You don't have the rights for delete an account");
            return null;
        }
        else
        {
            InitPopup("Account deleted" + request.responseCode);
            DeleteUserInformations();
            ProfilButton.gameObject.SetActive(false);
            return dataJSON;
        }
    }

    /// Check user informations for validate a registration.
    /// @returns Boolean if the validation is good or not.
    public bool RegistrationCheckInformations()
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

    /// Check user information for validate the login.
    /// @returns Boolean if the validation is good or not.
    public bool LoginCheckInformations()
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

    /// Initiate the popup text.
    /// 
    public void InitPopup(string str)
    {
        popupText.text = str;
        if (str != "Connected")
        {
            StartCoroutine(PopupText());
        } 
    }

    /// Activate for a time the popup interface.
    /// @returns IEnumerator delay
    public IEnumerator PopupText()
    {
        popup.SetActive(true);
        yield return new WaitForSeconds(2);
        popup.SetActive(false);
    }

    /// Delete the user informations after a logout.
    public void DeleteUserInformations()
    {
        GlobalStatus.token = "";
        this.firstname = "";
        this.lastname = "";
        this.id = "";
    }

    /// Clean all fields in the interface wich can contains any informations about the user.
    public void CleanFields(bool flag)
    {
        if (flag)
        {
            firstnameField.text = "";
            lastnameField.text = "";
            mailField.text = "";
            passwordField.text = "";
            confirmedPasswordField.text = "";
        } else
        {
            loginMailField.text = "";
            loginPasswordField.text = "";
        }
        
    }
}

/// Class specifying user informations in order to construct JSON body.
public class Users
{
    /// User email.
    public string email;
    /// User password.
    public string password;
    /// User firstname.
    public string firstName;
    /// User lastname;
    public string lastName;
}
