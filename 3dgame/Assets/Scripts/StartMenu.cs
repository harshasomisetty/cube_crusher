using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System;


public class StartMenu : MonoBehaviour
{

    public TextMeshProUGUI loginButtonText;

    private Boolean log = false;

    private void Start()
    {
        loginButtonText.text = "Test";
        UpdateLoginButtonText();
    }

    public void StartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void LoginOrLogout()
    {
        Debug.Log("in login function " + log);
        if (IsLoggedIn())
        {
            // Example: PlayerPrefs.DeleteKey("LoginToken");
            // PlayerPrefs.DeleteKey("LoginToken");
            log = false;
            // Potentially redirect to the start screen or perform other logout logic
        }
        else
        {
            // log = true;
            SceneManager.LoadScene("Scenes/ProfileScreen");
        }
        UpdateLoginButtonText();
        Debug.Log("log var " + log);
    }

    public void UpdateLoginButtonText()
    {
        if (IsLoggedIn())
        {
            loginButtonText.text = "Logout";
        }
        else
        {
            loginButtonText.text = "Login";
        }
    }

    private bool IsLoggedIn()
    {
        // return PlayerPrefs.HasKey("LoginToken");
        return log;
    }
}
