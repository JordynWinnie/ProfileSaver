using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Location : MonoBehaviour
{
    [SerializeField] private LocationInformation locationInformation;

    // Start is called before the first frame update
    private void Start()
    {
        var button = GetComponent<Button>();
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(delegate { DisplayInformation.infoDisplayHelper.DisplayLocationPopup(locationInformation); });

        GetComponent<Image>().sprite = locationInformation.locationSprite;
        GetComponentInChildren<TextMeshProUGUI>().text = locationInformation.locationName;
    }
}