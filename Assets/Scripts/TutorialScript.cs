using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TutorialScript : MonoBehaviour
{
    [SerializeField] private Button previousButton;

    [SerializeField] private Button nextButton;

    [SerializeField] private Image currentImage;

    [SerializeField] private TextMeshProUGUI currentTutorialText;

    [SerializeField] private List<Sprite> tutorialImages;

    private List<Tutorial> tutorials;

    private int currentIndex = 0;
    class Tutorial
    {
        public Tutorial(string tutString, Sprite sprite)
        {
            this.tutString = tutString;
            this.sprite = sprite;
        }

        public string tutString;
        public Sprite sprite;
    }
    // Start is called before the first frame update
    void Start()
    {
        tutorials = new List<Tutorial>
        {
            new Tutorial("Start off the game by choosing a profile. You can shuffle through profiles to choose who you want to play as.", tutorialImages[0]),
            new Tutorial("Your character has hunger, health, happiness, energy and money stats. Goal of the game is to maintain all these statistics while managing your money. Have to play the game according to the character profile you have chosen.", tutorialImages[1]),
            new Tutorial("Each location has different choices that you can choose. Some locations are only open at certain times.", tutorialImages[2]),
            new Tutorial("Don't take too long to decide as there is a timer which will run out and there will be penalties! Travelling also uses up hunger and time, so spend your time wisely!", tutorialImages[3]),
            new Tutorial("Random events might appear as you play the game.", tutorialImages[4]),
            new Tutorial("There are two types of goals - daily and monthly. strive to accomplish the goals - daily for rewards, monthly for better star rating at the end of the month!", tutorialImages[5]),
            new Tutorial("After about a month (avg 30 days passed), your stats will be calculated and star ratings will be given to you.", tutorialImages[6]),
            new Tutorial("The game autosaves your profile after every decision, which can be accessed through the load game in main menu. Good luck and have fun!", tutorialImages[7]),
        };

        previousButton.gameObject.SetActive(false);
        
        currentTutorialText.text = tutorials[currentIndex].tutString;
        currentImage.sprite = tutorials[currentIndex].sprite;
    }

    public void PreviousPage()
    {
        currentIndex--;
        if (currentIndex == 0) previousButton.gameObject.SetActive(false);
        nextButton.gameObject.SetActive(true);

        currentTutorialText.text = tutorials[currentIndex].tutString;
        currentImage.sprite = tutorials[currentIndex].sprite;
    }
    
    public void NextPage()
    {
        currentIndex++;
        if (currentIndex == tutorials.Count - 1) nextButton.gameObject.SetActive(false);
        previousButton.gameObject.SetActive(true);

        currentTutorialText.text = tutorials[currentIndex].tutString;
        currentImage.sprite = tutorials[currentIndex].sprite;
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene(0);
    }
}
