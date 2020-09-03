﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenHandler : MonoBehaviour
{
    public Animator animator;

    public GameObject MenuButton;
    public GameObject ProfileButton;


    private void Awake()
    {
        this.ShowUi();
    }

    public void HideUi()
    {
        CloseMenu();
        MenuButton.SetActive(false);
        ProfileButton.SetActive(false);
    }

    public void ShowUi()
    {
        CloseMenu();
        MenuButton.SetActive(true);
        ProfileButton.SetActive(true);
    }

    private void changeStateOf(string varname, bool b)
    {
        if (animator != null)
        {
            animator.SetBool(varname, b);
        }
    }

    #region OpenScreen
    public void OpenMenu()
    {
        changeStateOf("openMenu", true);
    }
    public void OpenScan()
    {
        changeStateOf("openScan", true);
        CloseMenu();
    }

    public void OpenAdd()
    {
        changeStateOf("openAdd", true);
        CloseMenu();
        //CloseValidation();
    }

    public void OpenStage()
    {
        changeStateOf("openStage", true);
        CloseMenu();
    }
    public void OpenLogin()
    {
        changeStateOf("openLogin", true);
        CloseMenu();
    }

    public void OpenValidation()
    {
        changeStateOf("openValidation", true);
    }

    public void OpenProfil()
    {
        Debug.Log("Open");
        changeStateOf("openProfil", true);
        CloseMenu();
    }
    #endregion

    #region CloseScreen
    public void CloseMenu()
    {
        changeStateOf("openMenu", false);
    }
    public void CloseScan()
    {
        changeStateOf("openScan", false);
    }

    public void CloseAdd()
    {
        changeStateOf("openAdd", false);
    }

    public void CloseStage()
    {
        changeStateOf("openStage", false);
    }
    public void CloseLogin()
    {
        changeStateOf("openLogin", false);
    }

    public void CloseValidation()
    {
        changeStateOf("openValidation", false);
    }

    public void CloseProfil()
    {
        changeStateOf("openProfil", false);
        CloseMenu();
    }
    #endregion
}
