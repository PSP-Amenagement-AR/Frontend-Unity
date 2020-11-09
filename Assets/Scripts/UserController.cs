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

    private string token;

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
                    else
                        InitPopup("This email is already used");
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
                    if (this.token != null)
                        InitPopup("The user is already connected");
                    else if (sendRequest != null)
                    {
                        canvas.CloseLogin();
                        this.token = sendRequest["token"].Value;
                        Debug.Log("token : " + this.token);
                    }
                    
                    else
                        InitPopup("The credentials are incorrect");
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
        var request = webApi.SendApiRequest("/users/register", "POST", new Users { email = mailField.text.ToString(), password = passwordField.text.ToString() }); // A complèter avec les champs "nom" et "prenom" lorsque le Back sera à jour
        JSONNode dataJSON = JSON.Parse(request.downloadHandler.text);

        if (request.responseCode == 200 || request.responseCode == 201)
            return dataJSON;
        else
            return null;
    }

    JSONNode Login()
    {
        var request = webApi.SendApiRequest("/users/login", "POST", new Users { email = loginMailField.text.ToString(), password = loginPasswordField.text.ToString() });
        JSONNode dataJSON = JSON.Parse(request.downloadHandler.text);

        if (request.responseCode == 200 || request.responseCode == 201)
            return dataJSON;
        else
            return null;
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
}
