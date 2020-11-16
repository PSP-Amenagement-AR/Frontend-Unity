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

    public Button submitButton;
    public Button loginButton;

    public InputField loginMailField;
    public InputField loginPasswordField;

    public InputField firstnameField;
    public InputField lastnameField;
    public InputField mailField;
    public InputField passwordField;
    public InputField confirmedPasswordField;

    APIrequestManager webApi = new APIrequestManager();

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
                    var sendRequest = Login();
                    /*if (GlobalStatus.token != "")
                        InitPopup("Please disconnect the actual account before reconnect");*/
                    if (sendRequest != null)
                    {
                        canvas.CloseLogin();
                        GlobalStatus.token = sendRequest["token"].Value;
                        Debug.Log("token : " + GlobalStatus.token);
                    }
                }
                catch (Exception e)
                {
                    Debug.Log("Error : " + e.Message);
                }
            }
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
        var request = webApi.SendApiRequest("/users/register", "POST", new Users { email = mailField.text.ToString(), password = passwordField.text.ToString(), firstName = firstnameField.text.ToString(), lastName = lastnameField.text.ToString() }); // A complèter avec les champs "nom" et "prenom" lorsque le Back sera à jour
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
        if (GlobalStatus.token != "")
        {
            InitPopup("Please disconnect the actual account before reconnect");
            return null;
        }
        else
        {
            var request = webApi.SendApiRequest("/users/login", "POST", new Users { email = loginMailField.text.ToString(), password = loginPasswordField.text.ToString() });
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
}
