using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Location : MonoBehaviour
{
    [SerializeField] public LocationInformation locationInformation;
    [SerializeField] public Image avatarLocation;

    // Start is called before the first frame update
    private void Start()
    {
        var button = GetComponent<Button>();
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(delegate { DisplayInformation.infoDisplayHelper.DisplayLocationPopup(this); });

        GetComponent<Image>().sprite = locationInformation.locationSprite;
        GetComponentInChildren<TextMeshProUGUI>().text = locationInformation.locationName;

        GameManager.instance.locationsList.Add(this);
    }

    public void ShowAvatar(bool isShown)
    {
        if (isShown)
            avatarLocation.color = Color.white;
        else
            avatarLocation.color = Color.clear;
    }
}