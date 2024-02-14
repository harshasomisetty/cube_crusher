using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using Data;
using UnityEngine.UI;

public class AwardJson
{

    public string name;
    public string description;
    public string imageUrl;

}

public class EndMenu : MonoBehaviour
{

    public GameObject redeemUI, awardUI;
    public TextMeshProUGUI description;
    public Image awardImage;


    private NetworkService networkService;

    private void Start()
    {
        networkService = new NetworkService();
        redeemUI.SetActive(true);
        awardUI.SetActive(false);
        Debug.Log("Starting EndMenu instance: " + GetInstanceID() + " with redeemUI: " + redeemUI.name);
    }

    public void QuitGame()
    {

        string userEmail = PlayerPrefs.GetString("UserEmail", "");

        if (!string.IsNullOrEmpty(userEmail))
        {
            StartCoroutine(networkService.FinishGameRoutine(userEmail, OnFinishGameSuccess, error =>
            {
                Debug.LogError("Login error: " + error);
            }));
        }
        else
        {
            Debug.LogError("UserEmail not found, cannot finish game properly.");
        }


        SceneManager.LoadScene("Scenes/StartScreen");
    }


    public void AwardUser()
    {
        string userEmail = PlayerPrefs.GetString("UserEmail", "");

        if (!string.IsNullOrEmpty(userEmail))
        {
            StartCoroutine(networkService.AwardUser(userEmail, OnAwardUserSuccess, error =>
            {
                Debug.LogError("Login error: " + error);
            }));
        }
        else
        {
            Debug.LogError("UserEmail not found, cannot award user properly.");
        }
    }

    private void OnFinishGameSuccess()
    {
        Debug.Log("Game finished successfully");
    }

    public void OnAwardUserSuccess(string json)
    {

        Debug.Log("AwardJson: " + json);
        var awardData = JsonUtility.FromJson<AwardJson>(json);
        Debug.Log("AwardJson image: " + awardData.imageUrl);
        if (awardImage == null)
        {
            Debug.LogError("awardImage is not assigned in the inspector.");
            return;
        }
        redeemUI.SetActive(false);
        awardUI.SetActive(true);
        StartCoroutine(networkService.DownloadImage(awardData.imageUrl, awardImage));


        Debug.Log("User awarded successfully");
    }


}
