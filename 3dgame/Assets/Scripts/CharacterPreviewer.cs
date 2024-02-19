using UnityEngine;

public class CharacterPreviewer : MonoBehaviour
{
    public GameObject[] playerPrefabs;
    public GameObject characterContainer;

    void Start()
    {
        PreviewPlayer();
    }

    void PreviewPlayer()
    {
        int selectedPlayerID = PlayerPrefs.GetInt("SelectedCharacter", 0);
        GameObject playerToPreview = playerPrefabs[selectedPlayerID];

        GameObject previewedPlayer = Instantiate(playerToPreview, characterContainer.transform);

        previewedPlayer.transform.localPosition = new Vector3(0, 0, -100);
        previewedPlayer.transform.localRotation = Quaternion.identity;

    }
}
