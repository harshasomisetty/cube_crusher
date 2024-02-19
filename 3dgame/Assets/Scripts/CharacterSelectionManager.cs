using UnityEngine;
using UnityEngine.UI;

public class CharacterSelectionManager : MonoBehaviour
{
    public GameObject[] characters; // Assign your character GameObjects in the inspector
    public Image highlightBorder; // The UI element used to highlight selected character

    public void SelectCharacter(int characterIndex)
    {
        // Save the selected character index
        PlayerPrefs.SetInt("SelectedCharacter", characterIndex);

        // Assuming each character GameObject has a child UI element with RectTransform
        RectTransform characterUIRect = characters[characterIndex].GetComponentInChildren<RectTransform>();

        // Move the highlight border to the UI element of the selected character
        highlightBorder.rectTransform.position = characterUIRect.position;
        // highlightBorder.rectTransform.sizeDelta = characterUIRect.sizeDelta;

        Debug.Log("Character selected: " + characters[characterIndex].name);
    }

    void Start()
    {
        // Select the previously selected character, or default to the first one
        int savedIndex = PlayerPrefs.GetInt("SelectedCharacter", 0);
        SelectCharacter(savedIndex);
    }
}
