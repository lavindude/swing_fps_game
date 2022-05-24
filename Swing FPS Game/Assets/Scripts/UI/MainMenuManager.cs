using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public Button pressAnyButton;
    public Button playButton;
    public Button optionsButton;
    public Button multiplayerButton;
    public Button tutorialsButton;
    public Button backButton;
    public Button level1Button;
    public Button level2Button;
    public Button level3Button;
    public Button level4Button;

    int menuState;

    Animator playAnim;
    Animator optionsAnim;
    Animator multiplayerAnim;
    Animator tutorialsAnim;
    Animator backAnim;
    Animator level1Anim;
    Animator level2Anim;
    Animator level3Anim;
    Animator level4Anim;

    // Start is called before the first frame update
    void Start()
    {
        playAnim = playButton.GetComponent<Animator>();
        optionsAnim = optionsButton.GetComponent<Animator>();
        multiplayerAnim = multiplayerButton.GetComponent<Animator>();
        tutorialsAnim = tutorialsButton.GetComponent<Animator>();
        backAnim = backButton.GetComponent<Animator>();
        level1Anim = level1Button.GetComponent<Animator>();
        level2Anim = level2Button.GetComponent<Animator>();
        level3Anim = level3Button.GetComponent<Animator>();
        level4Anim = level4Button.GetComponent<Animator>();

        menuState = 0;
        pressAnyButton.onClick.AddListener(SwitchToMenu);
        StartCoroutine(PressAnyButtonAnim());
    }

    // Update is called once per frame
    void Update()
    {
        if(menuState == 0 && Input.anyKeyDown)
        {
            SwitchToMenu();
        }

        if(menuState != 0)
        {
            pressAnyButton.gameObject.SetActive(false);
        }
    }

    void SwitchToMenu()
    {
        StartCoroutine(MainMenu());     
    }

    void SwitchToPlayMenu()
    {
        StartCoroutine(PlayMenu());
    }

    void SwitchToTutorials()
    {
        StartCoroutine(TutorialsMenu());
    }

    void BackPress()
    {
        if(menuState == 2)
        {
            multiplayerAnim.SetBool("Active", false);
            tutorialsAnim.SetBool("Active", false);
            backAnim.SetBool("Active", false);
            StartCoroutine(MainMenu());
        }
        else if(menuState == 3)
        {
            level1Anim.SetBool("Active", false);
            level2Anim.SetBool("Active", false);
            level3Anim.SetBool("Active", false);
            level4Anim.SetBool("Active", false);
            StartCoroutine(PlayMenu());
        }
    }

    void ToMultiplayer()
    {
        SceneManager.LoadScene("SampleScene");
    }

    void ToTutorial1()
    {
        SceneManager.LoadScene("Tutorial Level 1");
    }

    void ToTutorial2()
    {
        SceneManager.LoadScene("Tutorial Level 2");
    }
    
    void ToTutorial3()
    {
        SceneManager.LoadScene("Tutorial Level 3");
    }

    void ToTutorial4()
    {
        SceneManager.LoadScene("Tutorial Level 4");
    }

    IEnumerator PressAnyButtonAnim()
    {
        while(menuState == 0)
        {
            pressAnyButton.gameObject.SetActive(false);
            yield return new WaitForSeconds(0.75f);
            pressAnyButton.gameObject.SetActive(true);
            yield return new WaitForSeconds(0.8f);
        }
    }

    IEnumerator MainMenu()
    {
        menuState = 1;

        yield return new WaitForSeconds(0.25f);
        playAnim.SetBool("Active", true);
        optionsAnim.SetBool("Active", true);

        playButton.onClick.AddListener(SwitchToPlayMenu);
    }

    IEnumerator PlayMenu()
    {
        menuState = 2;

        playAnim.SetBool("Active", false);
        optionsAnim.SetBool("Active", false);
        yield return new WaitForSeconds(0.5f);
        multiplayerAnim.SetBool("Active", true);
        tutorialsAnim.SetBool("Active", true);
        backAnim.SetBool("Active", true);

        backButton.onClick.AddListener(BackPress);
        multiplayerButton.onClick.AddListener(ToMultiplayer);
        tutorialsButton.onClick.AddListener(SwitchToTutorials);
    }

    IEnumerator TutorialsMenu()
    {
        menuState = 3;

        multiplayerAnim.SetBool("Active", false);
        tutorialsAnim.SetBool("Active", false);
        yield return new WaitForSeconds(0.5f);
        level1Anim.SetBool("Active", true);
        level2Anim.SetBool("Active", true);
        level3Anim.SetBool("Active", true);
        level4Anim.SetBool("Active", true);

        level1Button.onClick.AddListener(ToTutorial1);
        level2Button.onClick.AddListener(ToTutorial2);
        level3Button.onClick.AddListener(ToTutorial3);
        level4Button.onClick.AddListener(ToTutorial4);
    }

}
