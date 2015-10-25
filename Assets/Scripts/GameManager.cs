using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using InputHandling;
using MenusHandler;

public class GameManager : MonoBehaviour
{
    public MonoBehaviour[] PermanentManagers;

    public Image LevelTransitionImage;

    private static GameManager _instance;

    private int SouffliPlayerNumber;
    private int AspiPlayerNumber;

    private int _levelIndex = 0;

    private const int FIRST_PLAYABLE_LEVEL_INDEX = 2;
    private const float FADE_DURATION = 1.5f;

    public static GameManager Instance
    {
        get
        {
            return _instance;
        }
    }

    private bool _isPaused;

    public bool IsPaused
    {
        get { return _isPaused; }
    }

    private bool _isInBackground;

    public bool IsInBackground
    {
        get { return _isInBackground; }
    }
    

    void Awake()
    {
        if (_instance)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);

            for (int i = 0; i < PermanentManagers.Length; i++)
            {
                MonoBehaviour permanentManager = PermanentManagers[i];

                string name = permanentManager.name;

                permanentManager = Instantiate(permanentManager) as MonoBehaviour;
                permanentManager.name = name;

                DontDestroyOnLoad(permanentManager.gameObject);
            }
        }

        Application.runInBackground = false;

        // If the manager got created in the main menu scene, we load the menu
        if (_levelIndex == 0)
        {
            MenusManager.Instance.ShowMenu("MainMenu");
            InputManager.Instance.PushContext(new MainMenuContext());
        }
    }

    void Start()
    {
        InputManager.Instance.AddCallback(0, GameManagerCallback);
    }

    public void SetSouffliPlayerNumber(int number)
    {
        SouffliPlayerNumber = number;
        GameObject.FindGameObjectWithTag("Souffli").GetComponent<Player>().Number = SouffliPlayerNumber;
    }

    public void SetAspiPlayerNumber(int number)
    {
        AspiPlayerNumber = number;
        GameObject.FindGameObjectWithTag("Aspi").GetComponent<Player>().Number = AspiPlayerNumber;
    }

    public void RestartLevel()
    {
        StartCoroutine(LoadLevel(_levelIndex));
    }

    public void LoadNextLevel()
    {
        if (_levelIndex < Application.levelCount - 1)
        {
            StartCoroutine(LoadLevel(_levelIndex + 1));
        }
        else
        {
            StartCoroutine(LoadLevel(0));
        }
    }

    public void LoadPreviousLevel()
    {
        if (_levelIndex > 0)
        {
            StartCoroutine(LoadLevel(_levelIndex - 1)); 
        }
    }

    public void LoadMainMenu()
    {
        StartCoroutine(LoadLevel(0));
    }

    private IEnumerator LoadLevel(int levelIndex)
    {
        // Ideally, we want to be able to dynamically instantiate Aspi and Souffli when we load a level, but for now we can't do it elegantly because of the rails

        yield return StartCoroutine(FadeIn());

        Application.LoadLevel(levelIndex);
    }

    void OnLevelWasLoaded(int levelIndex)
    {
        if (levelIndex == 0)
        {
            InputManager.Instance.PushContext(new MainMenuContext());
        }
        else if (levelIndex == FIRST_PLAYABLE_LEVEL_INDEX - 1)
        {
            InputManager.Instance.PushContext(new CharacterSelectionContext());
        }
        else
        {
            InputManager.Instance.PushContext(new GameplayContext());
        }
        

        // If we just loaded the main menu
        if (levelIndex == 0)
        {
            MenusManager.Instance.ShowMenu("MainMenu");
        }
        else if (levelIndex >= FIRST_PLAYABLE_LEVEL_INDEX)
        {
            GameObject.FindGameObjectWithTag("Souffli").GetComponent<Player>().Number = SouffliPlayerNumber;
            GameObject.FindGameObjectWithTag("Aspi").GetComponent<Player>().Number = AspiPlayerNumber;
        }

        _levelIndex = levelIndex;

        StartCoroutine(FadeOut());
    }

    private IEnumerator FadeIn()
    {
        float ratio = 0f;

        Color transitionColor = LevelTransitionImage.color;

        Color initialColor = new Color(transitionColor.r, transitionColor.g, transitionColor.b, 0f);
        Color finalColor = new Color(transitionColor.r, transitionColor.g, transitionColor.b, 1f);

        LevelTransitionImage.gameObject.SetActive(true);

        while (ratio < 1f)
        {
            ratio += Time.deltaTime / FADE_DURATION;

            LevelTransitionImage.color = Color.Lerp(initialColor, finalColor, ratio);

            yield return null;
        }
    }

    private IEnumerator FadeOut()
    {
        float ratio = 0f;

        Color transitionColor = LevelTransitionImage.color;

        Color initialColor = new Color(transitionColor.r, transitionColor.g, transitionColor.b, 1f);
        Color finalColor = new Color(transitionColor.r, transitionColor.g, transitionColor.b, 0f);

        LevelTransitionImage.gameObject.SetActive(true);

        while (ratio < 1f)
        {
            ratio += Time.deltaTime / FADE_DURATION;

            LevelTransitionImage.color = Color.Lerp(initialColor, finalColor, ratio);

            yield return null;
        }

        LevelTransitionImage.gameObject.SetActive(false);
    }

    public void Pause()
    {
        _isPaused = true;
        Time.timeScale = 0;
    }

    public void Resume()
    {
        _isPaused = false;
        Time.timeScale = 1;
    }

    void OnApplicationFocus(bool focusStatus)
    {
        _isInBackground = !focusStatus;
    }

    private void GameManagerCallback(MappedInput mappedInput)
    {
        bool acceptButtonPressed = mappedInput.Actions.Contains(InputConstants.ACCEPT_MENU_OPTION);
        bool backButonPressed = mappedInput.Actions.Contains(InputConstants.BACK_MENU_OPTION);
        float horizontalAxis = 0f;
        float verticalAxis = !mappedInput.Ranges.ContainsKey(InputConstants.CHANGE_MENU_OPTION_VERTICAL) ? 0f : mappedInput.Ranges[InputConstants.CHANGE_MENU_OPTION_VERTICAL];

        MenusManager.Instance.SetInputValues(acceptButtonPressed, backButonPressed, horizontalAxis, verticalAxis);

        if (!_isPaused && _levelIndex >= FIRST_PLAYABLE_LEVEL_INDEX && mappedInput.Actions.Contains(InputConstants.PAUSE))
        {
            MenusManager.Instance.ShowMenu("PauseMenu");
            InputManager.Instance.PushContext(new PauseMenuContext());
        }
    }
}
