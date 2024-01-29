using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using TMPro;
using System;
using System.Collections;

public class ProfileUIManager : MonoBehaviour
{
    public TMP_InputField emailInputField;
    public GameObject loginUI;
    public TextMeshProUGUI profileNameText;
    public GameObject profileDataUI;
    public GameObject logoutButton;

    [System.Serializable]
    public class UserData
    {
        public string name;
        // Other fields that your JSON might contain
    }

    private void Start()
    {
        UpdateUI();
    }

    public void AttemptLogin()
    {
        string email = emailInputField.text;
        if (!string.IsNullOrEmpty(email))
        {
            StartCoroutine(LoginRoutine(email));
        }
    }

    private IEnumerator LoginRoutine(string email)
    {
        string url = "http://localhost:3000/v2/users/" + email;

        using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
        {
            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.ConnectionError || webRequest.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("Error: " + webRequest.error);
            }
            else
            {
                PlayerPrefs.SetString("UserEmail", email);
                ProcessResponse(webRequest.downloadHandler.text);
                UpdateUI();
            }
        }
    }

    private void ProcessResponse(string json)
    {
        Debug.Log("process " + json);
        // Parse the JSON response
        // You need to define a class that matches the structure of your JSON data
        // For example:
        // UserData userData = JsonUtility.FromJson<UserData>(json);
        // Update the UI elements based on userData
        // profileNameText.text = userData.name; // Example
    }



    public void Logout()
    {
        PlayerPrefs.DeleteKey("UserEmail");
        emailInputField.text = "";
        UpdateUI();
    }

    private void UpdateUI()
    {
        string userEmail = PlayerPrefs.GetString("UserEmail", "");

        if (!string.IsNullOrEmpty(userEmail))
        {
            loginUI.SetActive(false);
            profileDataUI.SetActive(true);
            logoutButton.SetActive(true);
            profileNameText.text = userEmail;
        }
        else
        {
            loginUI.SetActive(true);
            profileDataUI.SetActive(false);
            logoutButton.SetActive(false);
        }
    }
}
