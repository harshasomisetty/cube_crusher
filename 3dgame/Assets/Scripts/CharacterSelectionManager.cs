using System;
using UnityEngine;
using UnityEngine.UI;
using Data;
using System.Collections.Generic;
using System.Linq;

public class CharacterJson
{
    public string[] characters;
}

public class CharacterSelectionManager : MonoBehaviour
{

    private NetworkService networkService;

    public GameObject[] characters;
    public Image highlightBorder;
    public Material disabledMaterial;

    List<int> unlockedCharacters = new List<int>(3);

    private const string HasFetchedKey = "HasFetchedCharacters";
    private const string UnlockedCharactersKey = "UnlockedCharacters";


    public void SelectCharacter(int characterIndex)
    {
        if (!unlockedCharacters.Contains(characterIndex))
        {
            Debug.Log("Character " + characterIndex + " is locked.");
            return;
        }

        RectTransform characterUIRect = characters[characterIndex].GetComponentInChildren<RectTransform>();
        highlightBorder.rectTransform.position = characterUIRect.position;
        PlayerPrefs.SetInt("SelectedCharacter", characterIndex);
        Debug.Log("Character selected: " + characters[characterIndex].name);
    }

    public void SetMaterialToDisabled()
    {
        for (int i = 0; i < characters.Length; i++)
        {
            GameObject character = characters[i];
            if (!unlockedCharacters.Contains(i))
            {
                // Disable the button or selection component here if necessary
                // For example, if your characters have a Button component attached
                // Button buttonComponent = character.GetComponent<Button>();
                // if (buttonComponent != null)
                // {
                //     buttonComponent.interactable = false;
                // }

                // Apply the disabled material
                Image imageComponent = character.GetComponent<Image>();
                if (imageComponent != null)
                {
                    imageComponent.material = disabledMaterial;
                }
            }
        }
    }

    private void onGetCharactersSuccess(string json)
    {
        var characterJson = JsonUtility.FromJson<CharacterJson>(json);

        unlockedCharacters.Clear();

        for (int i = 0; i < characterJson.characters.Length; i++)
        {
            for (int j = 0; j < characters.Length; j++)
            {
                if (characters[j].name == characterJson.characters[i])
                {
                    unlockedCharacters.Add(j);
                    break;
                }
            }
        }

        string data = string.Join(",", unlockedCharacters);
        PlayerPrefs.SetString(UnlockedCharactersKey, data);
        PlayerPrefs.Save();

        int savedIndex = PlayerPrefs.GetInt("SelectedCharacter", 0);
        SelectCharacter(savedIndex);
        SetMaterialToDisabled();
    }


    void Start()
    {
        LoadUnlockedCharacters();

        bool hasFetched = PlayerPrefs.GetInt(HasFetchedKey, 0) == 1;
        if (!hasFetched)
        {
            FetchUnlockedCharacters();
        }
        else
        {
            int savedIndex = PlayerPrefs.GetInt("SelectedCharacter", 0);
            SelectCharacter(savedIndex);
            SetMaterialToDisabled();
        }
    }

    public void FetchUnlockedCharacters()
    {
        string userEmail = PlayerPrefs.GetString("UserEmail", "");
        if (!string.IsNullOrEmpty(userEmail))
        {
            networkService = new NetworkService();

            StartCoroutine(networkService.GetUserCharacters(userEmail, onGetCharactersSuccess, error =>
            {
                Debug.LogError("GetUserCharacters error: " + error);
            }));

            PlayerPrefs.SetInt(HasFetchedKey, 1);
            PlayerPrefs.Save();
        }
        else
        {
            Debug.LogError("UserEmail not set in PlayerPrefs");
        }
    }

    public void ManualFetch()
    {
        PlayerPrefs.SetInt(HasFetchedKey, 0);
        FetchUnlockedCharacters();
    }

    private void LoadUnlockedCharacters()
    {
        string data = PlayerPrefs.GetString(UnlockedCharactersKey, "");
        if (!string.IsNullOrEmpty(data))
        {
            unlockedCharacters = data.Split(',').Select(int.Parse).ToList();
        }
        else
        {
            unlockedCharacters = new List<int>();
        }
    }


}
