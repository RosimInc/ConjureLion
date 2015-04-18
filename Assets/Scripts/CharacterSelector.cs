using UnityEngine;
using System.Collections;
using XInputDotNetPure; // Required in C#

public class CharacterSelector : MonoBehaviour
{
    private enum Characters { None, Souffli, Aspi }
    public GameObject StartButton;

    public PlayerMovement Player1;
    public PlayerMovement Player2;

    public Beam SouffliBeam;
    public Beam AspiBeam;

    public BrokenSouffli BrokenSouffli;
    public BrokenAspi BrokenAspi;

    public GameObject SouffliAButton1;
    public GameObject SouffliAButton2;
    public GameObject AspiAButton1;
    public GameObject AspiAButton2;

    public Aspi AspiCharacter;
    public Souffli SouffliCharacter;
    
    private bool _canStartGame = false;

    private Characters _player1Character;
    private Characters _player2Character;

    private bool _souffliHasBeenPicked = false;
    private bool _aspiHasBeenPicked = false;

    private bool _gameIsStarting = false;

    private Vector3 _fullRotationVector = new Vector3(0f, 0f, 360f);

    void Update()
    {
        //TODO: Set a state system instead of doing everything in the update, so that events trigger only when the player moves between the character zones

        //TODO: Have a separate script for the Aspi selector and the Souffli selector, that would inherit from this script

        if (Player1.Progress <= 0.1f && _player2Character != Characters.Souffli && !_souffliHasBeenPicked)
        {
            _player1Character = Characters.Souffli;
        }
        else if (Player1.Progress >= 0.97f && _player2Character != Characters.Aspi && !_aspiHasBeenPicked)
        {
            _player1Character = Characters.Aspi;
        }
        else
        {
            _player1Character = Characters.None;
        }


        if (Player2.Progress <= 0.1f && _player1Character != Characters.Souffli && !_souffliHasBeenPicked)
        {
            _player2Character = Characters.Souffli;
        }
        else if (Player2.Progress >= 0.97f && _player1Character != Characters.Aspi && !_aspiHasBeenPicked)
        {
            _player2Character = Characters.Aspi;
        }
        else
        {
            _player2Character = Characters.None;
        }

        if (_player1Character == Characters.Souffli)
        {
            SouffliBeam.StartObject = Player1.transform;
            SouffliBeam.Activate(true);

            SouffliAButton2.SetActive(false);
            SouffliAButton1.SetActive(true);
        }
        else if (_player2Character == Characters.Souffli)
        {
            SouffliBeam.StartObject = Player2.transform;
            SouffliBeam.Activate(true);

            SouffliAButton1.SetActive(false);
            SouffliAButton2.SetActive(true);
        }
        else
        {
            SouffliBeam.Activate(false);
            SouffliAButton1.SetActive(false);
            SouffliAButton2.SetActive(false);
        }

        if (_player1Character == Characters.Aspi)
        {
            AspiBeam.StartObject = Player1.transform;
            AspiBeam.Activate(true);
            
            AspiAButton2.SetActive(false);
            AspiAButton1.SetActive(true);
        }
        else if (_player2Character == Characters.Aspi)
        {
            AspiBeam.StartObject = Player2.transform;
            AspiBeam.Activate(true);

            AspiAButton1.SetActive(false);
            AspiAButton2.SetActive(true);
        }
        else
        {
            AspiBeam.Activate(false);
            AspiAButton1.SetActive(false);
            AspiAButton2.SetActive(false);
        }

        if (InputManager.Instance.GetInputAccept(1))
	    {
            switch (_player1Character)
            {
                case Characters.Souffli:
                    StartCoroutine(Player1ChooseSouffli(0.3f));
                    break;
                case Characters.Aspi:
                    StartCoroutine(Player1ChooseAspi(0.3f));
                    break;
            }
	    }

        if (InputManager.Instance.GetInputAccept(2))
        {
            switch (_player2Character)
            {
                case Characters.Souffli:
                    StartCoroutine(Player2ChooseSouffli(0.3f));
                    break;
                case Characters.Aspi:
                    StartCoroutine(Player2ChooseAspi(0.3f));
                    break;
            }
        }

        ShowOrHideStartButton();


        if (_canStartGame && !_gameIsStarting && (InputManager.Instance.GetInputPauseMenu(1) || InputManager.Instance.GetInputPauseMenu(2)))
        {
            /*
            if (_player1Character == Characters.Souffli)
            {
                GameManager.Instance.SetSouffliPlayerNumber(1);
                GameManager.Instance.SetAspiPlayerNumber(2);
            }
            else
            {
                GameManager.Instance.SetSouffliPlayerNumber(2);
                GameManager.Instance.SetAspiPlayerNumber(1);
            }*/

            _gameIsStarting = true;

            MusicManager.Instance.PlayGoalLevel();
            GameManager.Instance.LoadNextLevel();
        }
    }

    private IEnumerator Player1ChooseSouffli(float duration)
    {
        _souffliHasBeenPicked = true;

        Vector3 brokenSouffliInitialPos = BrokenSouffli.transform.position;
        Vector3 brokenSouffliInitialRot = BrokenSouffli.transform.eulerAngles;

        float ratio = 0f;

        while (ratio < 1f)
        {
            ratio += Time.deltaTime / duration;

            BrokenSouffli.transform.position = Vector3.Lerp(brokenSouffliInitialPos, Player1.transform.position, ratio);
            BrokenSouffli.transform.eulerAngles = Vector3.Lerp(brokenSouffliInitialRot, Vector3.zero, ratio);

            yield return null;
        }

        SouffliCharacter = Instantiate(SouffliCharacter) as Souffli;
        Destroy(SouffliCharacter.GetComponent<BallEffect>());

        SouffliCharacter.GetComponent<PlayerMovement>().spline = Player1.spline;
        SouffliCharacter.GetComponent<PlayerMovement>().Progress = Player1.Progress;

        // TODO: Destroy instead of hiding (need to rework the code first)
        BrokenSouffli.gameObject.SetActive(false);
        Player1.gameObject.SetActive(false);

        GameManager.Instance.SetSouffliPlayerNumber(1);
    }

    private IEnumerator Player2ChooseSouffli(float duration)
    {
        _souffliHasBeenPicked = true;

        Vector3 brokenSouffliInitialPos = BrokenSouffli.transform.position;
        Vector3 brokenSouffliInitialRot = BrokenSouffli.transform.eulerAngles;

        float ratio = 0f;

        while (ratio < 1f)
        {
            ratio += Time.deltaTime / duration;

            BrokenSouffli.transform.position = Vector3.Lerp(brokenSouffliInitialPos, Player2.transform.position, ratio);
            BrokenSouffli.transform.eulerAngles = Vector3.Lerp(brokenSouffliInitialRot, Vector3.zero, ratio);

            yield return null;
        }

        SouffliCharacter = Instantiate(SouffliCharacter) as Souffli;
        Destroy(SouffliCharacter.GetComponent<BallEffect>());

        SouffliCharacter.GetComponent<PlayerMovement>().spline = Player2.spline;
        SouffliCharacter.GetComponent<PlayerMovement>().Progress = Player2.Progress;

        // TODO: Destroy instead of hiding (need to rework the code first)
        BrokenSouffli.gameObject.SetActive(false);
        Player2.gameObject.SetActive(false);

        GameManager.Instance.SetSouffliPlayerNumber(2);
    }

    private IEnumerator Player1ChooseAspi(float duration)
    {
        _aspiHasBeenPicked = true;

        Vector3 brokenAspiInitialPos = BrokenAspi.transform.position;
        Vector3 brokenAspiInitialRot = BrokenAspi.transform.eulerAngles;

        float ratio = 0f;

        while (ratio < 1f)
        {
            ratio += Time.deltaTime / duration;

            BrokenAspi.transform.position = Vector3.Lerp(brokenAspiInitialPos, Player1.transform.position, ratio);
            BrokenAspi.transform.eulerAngles = Vector3.Lerp(brokenAspiInitialRot, _fullRotationVector, ratio);

            yield return null;
        }

        AspiCharacter = Instantiate(AspiCharacter) as Aspi;
        Destroy(AspiCharacter.GetComponent<BallEffect>());

        AspiCharacter.GetComponent<PlayerMovement>().spline = Player1.spline;
        AspiCharacter.GetComponent<PlayerMovement>().Progress = Player1.Progress;

        // TODO: Destroy instead of hiding (need to rework the code first)
        BrokenAspi.gameObject.SetActive(false);
        Player1.gameObject.SetActive(false);

        GameManager.Instance.SetAspiPlayerNumber(1);
    }

    private IEnumerator Player2ChooseAspi(float duration)
    {
        _aspiHasBeenPicked = true;

        Vector3 brokenAspiInitialPos = BrokenAspi.transform.position;
        Vector3 brokenAspiInitialRot = BrokenAspi.transform.eulerAngles;

        float ratio = 0f;

        while (ratio < 1f)
        {
            ratio += Time.deltaTime / duration;

            BrokenAspi.transform.position = Vector3.Lerp(brokenAspiInitialPos, Player2.transform.position, ratio);
            BrokenAspi.transform.eulerAngles = Vector3.Lerp(brokenAspiInitialRot, _fullRotationVector, ratio);

            yield return null;
        }

        AspiCharacter = Instantiate(AspiCharacter) as Aspi;
        Destroy(AspiCharacter.GetComponent<BallEffect>());

        AspiCharacter.GetComponent<PlayerMovement>().spline = Player2.spline;
        AspiCharacter.GetComponent<PlayerMovement>().Progress = Player2.Progress;
        

        // TODO: Destroy instead of hiding (need to rework the code first)
        BrokenAspi.gameObject.SetActive(false);
        Player2.gameObject.SetActive(false);

        GameManager.Instance.SetAspiPlayerNumber(2);
    }

    private void ShowOrHideStartButton()
    {
        if (_souffliHasBeenPicked && _aspiHasBeenPicked)
        {
            StartButton.SetActive(true);
            _canStartGame = true;
        }
        else
        {
            StartButton.SetActive(false);
            _canStartGame = false;
        }
    }

    /*
    void Update()
    {
        float player1XAxis = InputManager.Instance.GetInputMovement(1).x;
        float player2XAxis = InputManager.Instance.GetInputMovement(2).x;

        if (player1XAxis == 1f)
        {
            Player1Cursor.MoveRight();
            ShowOrHideStartButton();
        }
        else if (player1XAxis == -1f)
        {
            Player1Cursor.MoveLeft();
            ShowOrHideStartButton();
        }

        if (player2XAxis == 1f)
        {
            Player2Cursor.MoveRight();
            ShowOrHideStartButton();
        }
        else if (player2XAxis == -1f)
        {
            Player2Cursor.MoveLeft();
            ShowOrHideStartButton();
        }

        if (_canStartGame && (InputManager.Instance.GetInputAccept(1) || InputManager.Instance.GetInputAccept(2)))
        {
            if (Player1Cursor.Position == SelectorCursor.CursorPosition.Left)
            {
                GameManager.Instance.SetSouffliPlayerNumber(1);
                GameManager.Instance.SetAspiPlayerNumber(2);
            }
            else
            {
                GameManager.Instance.SetSouffliPlayerNumber(2);
                GameManager.Instance.SetAspiPlayerNumber(1);
            }

            GameManager.Instance.LoadNextLevel();
        }
    }

    private void ShowOrHideStartButton()
    {
        if (Player1Cursor.Position == SelectorCursor.CursorPosition.Left && Player2Cursor.Position == SelectorCursor.CursorPosition.Right
            || Player1Cursor.Position == SelectorCursor.CursorPosition.Right && Player2Cursor.Position == SelectorCursor.CursorPosition.Left)
        {
            StartButton.SetActive(true);
            _canStartGame = true;
        }
        else
        {
            StartButton.SetActive(false);
            _canStartGame = false;
        }
    }*/
}
