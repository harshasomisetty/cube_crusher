using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System;
using Data;


public class StartMenu : MonoBehaviour
{

    public TextMeshProUGUI loginButtonText;
    private UserDataManager userDataManager;


    private void Start()
    {
        userDataManager = new UserDataManager();

        if (userDataManager == null || loginButtonText == null)
        {
            Debug.LogError("UserDataManager component not found on the GameObject.");
            return;
        }

        UpdateLoginButtonText();
    }

    public void StartGame()
    {
        SceneManager.LoadScene("Scenes/Game");
    }

    public void LoginOrLogout()
    {
        UpdateLoginButtonText();
        SceneManager.LoadScene("Scenes/ProfileScreen");

    }

    public void UpdateLoginButtonText()
    {
        if (userDataManager == null || loginButtonText == null)
        {
            Debug.LogError("UserDataManager component not found on the GameObject.");
            return;
        }

        loginButtonText.text = userDataManager.IsLoggedIn() ? "Profile" : "Login";
    }

}
