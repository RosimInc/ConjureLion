using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public Level[] Levels;

    private static GameManager _instance;

    private int SouffliPlayerNumber;
    private int AspiPlayerNumber;

    private int _levelIndex = -1;

    public static GameManager Instance
    {
        get { return _instance; }
    }

    void Awake()
    {
        _instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void SetSouffliPlayerNumber(int number)
    {
        SouffliPlayerNumber = number;
    }

    public void SetAspiPlayerNumber(int number)
    {
        AspiPlayerNumber = number;
    }

    public void RestartLevel()
    {
        LoadLevel();
    }

    public void LoadNextLevel()
    {
        if (_levelIndex < Levels.Length - 1)
        {
            _levelIndex++;

            LoadLevel();
        }
    }

    public void LoadPreviousLevel()
    {
        if (_levelIndex > 0)
        {
            _levelIndex--;

            LoadLevel();
        }
    }

    private void LoadLevel()
    {
        // Ideally, we want to be able to dynamically instantiate Aspi and Souffli when we load a level, but for now we can't do it elegantly because of the rails
        
        Levels[_levelIndex].Load();
    }

    void OnLevelWasLoaded(int levelIndex)
    {
        GameObject.FindGameObjectWithTag("Souffli").GetComponent<Player>().Number = SouffliPlayerNumber;
        GameObject.FindGameObjectWithTag("Aspi").GetComponent<Player>().Number = AspiPlayerNumber;
    }
}
