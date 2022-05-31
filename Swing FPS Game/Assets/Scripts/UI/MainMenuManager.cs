using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
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
    public Button toMultiplayerButton;
    public TMP_InputField playerName;
    public TextMeshProUGUI playerNameText;
    public TMP_InputField playerID;
    public TextMeshProUGUI playerIDText;
    public TMP_InputField lobbyID;
    public TextMeshProUGUI lobbyIDText;

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
    Animator toMultiplayerAnim;
    Animator playerNameAnim;
    Animator playerNameTextAnim;
    Animator playerIDAnim;
    Animator playerIDTextAnim;
    Animator lobbyIDAnim;
    Animator lobbyIDTextAnim;

    int nameExist;
    int pIDExist;
    int lIDExist;

    public static string pName;
    public static int pID;
    public static int lID;

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
        toMultiplayerAnim = toMultiplayerButton.GetComponent<Animator>();
        playerNameAnim = playerName.GetComponent<Animator>();
        playerNameTextAnim = playerNameText.GetComponent<Animator>();
        playerIDAnim = playerID.GetComponent<Animator>();
        playerIDTextAnim = playerIDText.GetComponent<Animator>();
        lobbyIDAnim = lobbyID.GetComponent<Animator>();
        lobbyIDTextAnim = lobbyIDText.GetComponent<Animator>();

        nameExist = 0;
        pIDExist = 0;
        lIDExist = 0;

        menuState = 0;
        pressAnyButton.onClick.AddListener(SwitchToMenu);
        playButton.onClick.AddListener(SwitchToPlayMenu);
        backButton.onClick.AddListener(BackPress);
        multiplayerButton.onClick.AddListener(SwitchToPreLobby);
        tutorialsButton.onClick.AddListener(SwitchToTutorials);
        level1Button.onClick.AddListener(ToTutorial1);
        level2Button.onClick.AddListener(ToTutorial2);
        level3Button.onClick.AddListener(ToTutorial3);
        level4Button.onClick.AddListener(ToTutorial4);
        toMultiplayerButton.onClick.AddListener(ToMultiplayer);
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
        playAnim.SetBool("Active", false);
        optionsAnim.SetBool("Active", false);
        StartCoroutine(PlayMenu());
    }

    void SwitchToTutorials()
    {
        StartCoroutine(TutorialsMenu());
    }

    void SwitchToPreLobby()
    {
        StartCoroutine(PreLobby());
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
        else if(menuState == 4)
        {
            playerNameAnim.SetBool("Active", false);
            playerNameTextAnim.SetBool("Active", false);
            playerIDAnim.SetBool("Active", false);
            playerIDTextAnim.SetBool("Active", false);
            lobbyIDAnim.SetBool("Active", false);
            lobbyIDTextAnim.SetBool("Active", false);

            StartCoroutine(PlayMenu());
        }
    }

    void ToMultiplayer()
    {
        Constants.playerId = pID;
        Constants.lobbyId = lID;

        int curId = 0;
        for (int i = 1; i <= Constants.maxPlayers; i++)
        {
            if (i != pID)
            {
                Constants.otherPlayerIds[curId] = i;
                curId++;
            }

        }

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

        yield return new WaitForSeconds(0.01f);
        playAnim.SetBool("Active", true);
        optionsAnim.SetBool("Active", true);
    }

    IEnumerator PlayMenu()
    {
        menuState = 2;

        yield return new WaitForSeconds(0.5f);
        multiplayerAnim.SetBool("Active", true);
        tutorialsAnim.SetBool("Active", true);
        backAnim.SetBool("Active", true);
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
    }

    IEnumerator PreLobby()
    {
        menuState = 4;

        multiplayerAnim.SetBool("Active", false);
        tutorialsAnim.SetBool("Active", false);
        yield return new WaitForSeconds(0.5f);
        playerNameAnim.SetBool("Active", true);
        playerNameTextAnim.SetBool("Active", true);
        playerIDAnim.SetBool("Active", true);
        playerIDTextAnim.SetBool("Active", true);
        lobbyIDAnim.SetBool("Active", true);
        lobbyIDTextAnim.SetBool("Active", true);
    }

    void EraseBack()
    {
        backAnim.SetBool("Fade", true);
        toMultiplayerAnim.SetBool("Active", true);
    }

    void EraseStart()
    {
        backAnim.SetBool("Fade", false);
        toMultiplayerAnim.SetBool("Active", false);
    }

        public void ReadPlayerName(TMP_InputField name)
    {
        Debug.Log(name.text);
        if(name.text.Length > 0)
        {
            Debug.Log("Entered");
            nameExist = 1;
            pName = name.text;
        }
        else
        {
            Debug.Log("Deleted");
            nameExist = 2;
        }
    }

    public void ReadPlayerID(TMP_InputField id)
    {
        Debug.Log(id.text);
        if(id.text.Length == 0)
        {
            Debug.Log("Deleted");
            pIDExist = 2;
        }
        else if(int.Parse(id.text) > 0)
        {
            Debug.Log("Entered");
            pIDExist = 1;
            pID = int.Parse(id.text);
        }
    }

    public void ReadLobbyID(TMP_InputField id)
    {
        Debug.Log(id.text);
        if(id.text.Length == 0)
        {
            Debug.Log("Deleted");
            lIDExist = 2;
        }
        else if(int.Parse(id.text) > 0)
        {
            Debug.Log("Entered");
            lIDExist = 1;
            lID = int.Parse(id.text);
        }
    }

    public void CheckIfReady(TMP_InputField placehold)
    {
        if(menuState == 4 )
        {
            if(nameExist == 1 && pIDExist == 1 && lIDExist == 1)
            {
                EraseBack();
                Debug.Log("HELLO");
            }
            else if(nameExist == 2 || pIDExist == 2 || lIDExist == 2)
            {
                EraseStart();
                Debug.Log("BYE");
            }
        }
    }
}
