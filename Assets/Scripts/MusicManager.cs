using UnityEngine;
using System.Collections;

public class MusicManager : MonoBehaviour
{
    public AudioSource SouffliBreathIn;
    public AudioSource SouffliBreathLoop;
    public AudioSource AspiBreathIn;
    public AudioSource AspiBreathLoop;
    public AudioSource GoalLevel;
    public AudioSource GoalStart;
    public AudioSource RailLoop;

    private static MusicManager _instance;

    public static MusicManager Instance
    {
        get { return _instance; }
    }

    void Awake()
    {
        _instance = this;
    }

    public void PlaySouffliBreathIn()
    {
        SouffliBreathIn.Play();
    }

    public void PlaySouffliBreathLoop()
    {
        SouffliBreathLoop.Play();
    }

    public void StopSouffliBreathLoop()
    {
        SouffliBreathLoop.Stop();
    }

    public void PlayAspiBreathIn()
    {
        AspiBreathIn.Play();
    }

    public void PlayAspiBreathLoop()
    {
        AspiBreathLoop.Play();
    }

    public void StopAspiBreathLoop()
    {
        AspiBreathLoop.Stop();
    }

    public void PlayGoalLevel()
    {
        GoalLevel.Play();
    }

    public void PlayGoalStart()
    {
        GoalStart.Play();
    }

    public void PlayRailLoop()
    {
        RailLoop.Play();
    }

    public void StopRailLoop()
    {
        RailLoop.Stop();
    }

    public void StopSouffliBreathIn()
    {
        SouffliBreathIn.Stop();
    }

    public void StopAspiBreathIn()
    {
        AspiBreathIn.Stop();
    }

    void OnLevelWasLoaded(int levelIndex)
    {
        StopRailLoop();
        StopSouffliBreathLoop();
        StopAspiBreathLoop();
    }
}
