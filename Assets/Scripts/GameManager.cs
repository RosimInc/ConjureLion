using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public MonoBehaviour[] PermanentManagers;

    public Image LevelTransitionImage;

    private static GameManager _instance;

    private int SouffliPlayerNumber;
    private int AspiPlayerNumber;

    private int _levelIndex = 0;

    private const int FIRST_PLAYABLE_LEVEL_INDEX = 1;
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
    }

    void Update()
    {
        if (!_isPaused && _levelIndex >= FIRST_PLAYABLE_LEVEL_INDEX && InputManager.Instance.GetInputPauseMenu())
        {
            MenusManager.Instance.ShowMenu("PauseMenu");
        }
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
        // Loads the character selection screen for now, but it's gonna load the main menu once we have one
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
        if (levelIndex >= FIRST_PLAYABLE_LEVEL_INDEX)
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
}
