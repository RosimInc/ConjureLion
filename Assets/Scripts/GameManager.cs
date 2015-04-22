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

    private const float FADE_DURATION = 2.5f;

    public static GameManager Instance
    {
        get
        {
            return _instance;
        }
    }

    void Awake()
    {
        _instance = this;
        DontDestroyOnLoad(gameObject);

        for (int i = 0; i < PermanentManagers.Length; i++)
        {
            MonoBehaviour permanentManager = PermanentManagers[i];

            permanentManager = Instantiate(permanentManager) as MonoBehaviour;

            DontDestroyOnLoad(permanentManager.gameObject);
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
        LoadLevel();
    }

    public void LoadNextLevel()
    {
        if (_levelIndex < Application.levelCount - 1)
        {
            _levelIndex++;

            StartCoroutine(LoadLevel());
        }
    }

    public void LoadPreviousLevel()
    {
        if (_levelIndex > 0)
        {
            _levelIndex--;

            StartCoroutine(LoadLevel()); 
        }
    }

    private IEnumerator LoadLevel()
    {
        // Ideally, we want to be able to dynamically instantiate Aspi and Souffli when we load a level, but for now we can't do it elegantly because of the rails

        yield return StartCoroutine(FadeIn());

        Application.LoadLevel(_levelIndex);
    }

    void OnLevelWasLoaded(int levelIndex)
    {
        StartCoroutine(FadeOut());

        GameObject.FindGameObjectWithTag("Souffli").GetComponent<Player>().Number = SouffliPlayerNumber;
        GameObject.FindGameObjectWithTag("Aspi").GetComponent<Player>().Number = AspiPlayerNumber;
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
}
