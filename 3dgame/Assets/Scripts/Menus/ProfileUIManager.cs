using Data;

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Unity.Properties;

public class DataCollection
{
    public string gamesPlayed;
    public string email;
}

public class ProfileUIManager : MonoBehaviour
{
    public TMP_InputField emailInputField;
    public GameObject loginUI, profileDataUI, logoutButton;
    public TextMeshProUGUI profileNameText, gamesPlayedText;

    private NetworkService networkService;
    private UserDataManager userDataManager;


    private void Start()
    {
        // Debug.Log("in start");
        networkService = new NetworkService();
        userDataManager = new UserDataManager();
        userDataManager.OnUserDataUpdated += UpdateUI;
        AutoLogin();
        UpdateUI();
    }

    private void OnDestroy()
    {
        userDataManager.OnUserDataUpdated -= UpdateUI;
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene("Scenes/StartScreen");
    }

    private void AutoLogin()
    {
        string email = userDataManager.GetUserData().email;
        if (!string.IsNullOrEmpty(email))
        {
            emailInputField.text = email;
            AttemptLogin();
        }
    }

    public void AttemptLogin()
    {
        string email = emailInputField.text;
        if (!string.IsNullOrEmpty(email))
        {
            StartCoroutine(networkService.LoginRoutine(email, userDataManager.ProcessLoginResponse, error =>
            {
                Debug.LogError("Login error: " + error);
            }));
        }
    }

    public void Logout()
    {
        userDataManager.LogoutUser();
        emailInputField.text = "";
    }

    private void UpdateUI()
    {
        var userData = userDataManager.GetUserData();


        bool isLoggedIn = userDataManager.IsLoggedIn();

        loginUI.SetActive(!isLoggedIn);
        profileDataUI.SetActive(isLoggedIn);
        logoutButton.SetActive(isLoggedIn);

        if (isLoggedIn)
        {
            profileNameText.text = "Email: " + userData.email;
            gamesPlayedText.text = "Games Played: " + userData.gamesPlayed;
        }
    }
}
