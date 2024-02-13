using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using Data;

public class EndMenu : MonoBehaviour
{
    private NetworkService networkService;

    private void Start()
    {
        networkService = new NetworkService();
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


        // string gamesPlayedString = PlayerPrefs.GetString("GamesPlayed", "");

        // if (!string.IsNullOrEmpty(userEmail))
        // {
        //     // StartCoroutine(networkService.LoginRoutine(userEmail, OnFinishGameSuccess, OnFinishGameError));
        // }
        // else
        // {
        //     Debug.LogError("UserEmail not found, cannot finish game properly.");
        // }

        SceneManager.LoadScene("Scenes/StartScreen");

    }

    private void OnFinishGameSuccess()
    {
        Debug.Log("Game finished successfully");
    }


}
