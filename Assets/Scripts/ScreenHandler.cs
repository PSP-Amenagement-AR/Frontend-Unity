using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Class for manage interface, windows and animations.
/// </summary>
public class ScreenHandler : MonoBehaviour
{
    /// Animator object for animation controller.
    public Animator animator;
    /// Menu button.
    public GameObject MenuButton;
    ///Profil button.
    public GameObject ProfileButton;

    /// Function executed when the script is started.
    /// Show the user interface when the application start.
    public void Awake()
    {
        this.ShowUi();
    }

    /// Hide the user interface.
    public void HideUi()
    {
        CloseMenu();
        MenuButton.SetActive(false);
        ProfileButton.SetActive(false);
    }

    /// Make appear the user itnerface.
    public void ShowUi()
    {
        CloseMenu();
        MenuButton.SetActive(true);
        ProfileButton.SetActive(false);
    }

    /// Change the state of an interface.
    /// @param varname The name of the interface
    /// @param b The boolean value for show or hide the interface
    public void changeStateOf(string varname, bool b)
    {
        if (animator != null)
        {
            animator.SetBool(varname, b);
        }
    }

    #region OpenScreen
    /// Open the menu.
    public void OpenMenu()
    {
        changeStateOf("openMenu", true);
    }
    /// Open the scan interface.
    public void OpenScan()
    {
        changeStateOf("openScan", true);
        CloseMenu();
    }
    /// Open the add item interface.
    public void OpenAdd()
    {
        changeStateOf("openAdd", true);
        CloseMenu();
        //CloseValidation();
    }
    /// Open the stage inventory interface.
    public void OpenStage()
    {
        changeStateOf("openStage", true);
        CloseMenu();
    }
    /// Open the login interface.
    public void OpenLogin()
    {
        changeStateOf("openLogin", true);
        CloseMenu();
    }
    /// Open the registration interface.
    public void OpenRegistration()
    {
        changeStateOf("openRegistration", true);
    }
    /// Open the validation interface.
    public void OpenValidation()
    {
        changeStateOf("openValidation", true);
    }
    /// Open the profil interface.
    public void OpenProfil()
    {
        changeStateOf("openProfil", true);
        CloseMenu();
    }
    /// Open the prefab manager interface.
    public void OpenPrefabManager()
    {
        changeStateOf("openPrefabManager", true);
    }
    #endregion

    #region CloseScreen
    /// Close the menu.
    public void CloseMenu()
    {
        changeStateOf("openMenu", false);
    }
    /// Close the scan interface.
    public void CloseScan()
    {
        changeStateOf("openScan", false);
    }
    /// Close the add item interface.
    public void CloseAdd()
    {
        changeStateOf("openAdd", false);
    }
    /// Close the stage interface.
    public void CloseStage()
    {
        changeStateOf("openStage", false);
    }
    /// Close the login interface.
    public void CloseLogin()
    {
        changeStateOf("openLogin", false);
    }
    /// Close the registration interface.
    public void CloseRegistration()
    {
        changeStateOf("openRegistration", false);
    }
    /// Close the validation interface.
    public void CloseValidation()
    {
        changeStateOf("openValidation", false);
    }
    /// Close the profil interface.
    public void CloseProfil()
    {
        changeStateOf("openProfil", false);
        CloseMenu();
    }
    /// Close the prefab manager interface.
    public void ClosePrefabManager()
    {
        changeStateOf("openPrefabManager", false);
    }
    #endregion
}
