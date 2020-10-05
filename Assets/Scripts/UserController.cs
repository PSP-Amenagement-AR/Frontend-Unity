using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UserController : MonoBehaviour
{
    public Button submitButton;
    public InputField firstnameField;
    public InputField lastnameField;
    public InputField mailField;
    public InputField passwordField;
    public InputField confirmedPasswordField;
    private bool created = false;

    private void Awake()
    {
        submitButton.onClick.AddListener(() =>
        {
            if ((firstnameField.text.ToString() != "") && (lastnameField.text.ToString() != "") &&
                (mailField.text.ToString() != "") && (passwordField.text.ToString() != "") && (confirmedPasswordField.text.ToString() != ""))
            {
                if (passwordField.text.ToString() != confirmedPasswordField.text.ToString())
                {
                    Debug.Log("Not the same password");
                    return;
                }
                else if (passwordField.text.ToString().Length < 9)
                {
                    Debug.Log("Password too short");
                }
                else if ((mailField.text.ToString().Contains("@") == false) || (mailField.text.ToString().Contains(".com") == false))
                {
                    Debug.Log("The mail is not correct");
                }
                else
                {
                    Debug.Log("User created " + firstnameField.text.ToString() + " "
                        + lastnameField.text.ToString() + " "
                        + mailField.text.ToString() + " "
                        + passwordField.text.ToString() + " "
                        + confirmedPasswordField.text.ToString());

                    created = true;
                }
            } else
            {
                Debug.Log("One or many field(s) are missing");
                return;
            }
        });
    }

    void Start()
    {

    }

    void Update()
    {

    }
}
