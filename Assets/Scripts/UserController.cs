using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    private bool created = false;
    private bool connected = false;

    private void Awake()
    {
        submitButton.onClick.AddListener(() =>
        {
            if (RegistrationCheckInformations())
            {
                canvas.CloseRegistration();
                canvas.OpenLogin();
            }
        });

        loginButton.onClick.AddListener(() =>
        {
            if (LoginCheckInformations())
            {
                canvas.CloseLogin();
            }
        });
    }

    void Start()
    {

    }

    void Update()
    {
        
    }

    bool RegistrationCheckInformations()
    {
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
        StartCoroutine(PopupText());
    }

    IEnumerator PopupText()
    {
        popup.SetActive(true);
        yield return new WaitForSeconds(2);
        popup.SetActive(false);
    }
}
