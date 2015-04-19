using UnityEngine;
using System.Collections;
using XInputDotNetPure; // Required in C#

public class CharacterSelector : MonoBehaviour
{
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

    private enum PlayerState { SelectingNothing, SelectingSouffli, SelectingAspi, PickedSouffli, PickedAspi }
    private PlayerState _player1State;
    private PlayerState _player2State;

    private bool _gameIsReady = false;
    private bool _gameIsStarting = false;

    private float _previousPlayer1Progress;
    private float _previousPlayer2Progress;

    private Vector3 _fullRotationVector = new Vector3(0f, 0f, 360f);
    private const float SOUFFLI_TRESHOLD = 0.1f;
    private const float ASPI_TRESHOLD = 0.9f;

    void Update()
    {
        if (_gameIsReady)
        {
            if (!_gameIsStarting && (InputManager.Instance.GetInputPauseMenu(1) || InputManager.Instance.GetInputPauseMenu(2)))
            {
                _gameIsStarting = true;

                MusicManager.Instance.PlayGoalLevel();
                GameManager.Instance.LoadNextLevel();
            }
        }
        else
        {
            // Player1 state
            if (_player1State != PlayerState.PickedSouffli && _player1State != PlayerState.PickedAspi)
            {
                if (_player2State != PlayerState.SelectingSouffli && Player1.Progress <= SOUFFLI_TRESHOLD && _previousPlayer1Progress > SOUFFLI_TRESHOLD)
                {
                    SetPlayer1State(PlayerState.SelectingSouffli);
                }
                else if (_player2State != PlayerState.SelectingAspi && Player1.Progress >= ASPI_TRESHOLD && _previousPlayer1Progress < ASPI_TRESHOLD)
                {
                    SetPlayer1State(PlayerState.SelectingAspi);
                }
                else if (_previousPlayer1Progress <= SOUFFLI_TRESHOLD && Player1.Progress > SOUFFLI_TRESHOLD
                    || _previousPlayer1Progress >= ASPI_TRESHOLD && Player1.Progress < ASPI_TRESHOLD)
                {
                    SetPlayer1State(PlayerState.SelectingNothing);
                }

                if (InputManager.Instance.GetInputAccept(1))
                {
                    switch (_player1State)
                    {
                        case PlayerState.SelectingSouffli:
                            SetPlayer1State(PlayerState.PickedSouffli);
                            break;
                        case PlayerState.SelectingAspi:
                            SetPlayer1State(PlayerState.PickedAspi);
                            break;
                    }
                }
            }

            // Player2 state
            if (_player2State != PlayerState.PickedSouffli && _player2State != PlayerState.PickedAspi)
            {
                if (_player1State != PlayerState.SelectingSouffli && Player2.Progress <= SOUFFLI_TRESHOLD && _previousPlayer2Progress > SOUFFLI_TRESHOLD)
                {
                    SetPlayer2State(PlayerState.SelectingSouffli);
                }
                else if (_player1State != PlayerState.SelectingAspi && Player2.Progress >= ASPI_TRESHOLD && _previousPlayer2Progress < ASPI_TRESHOLD)
                {
                    SetPlayer2State(PlayerState.SelectingAspi);
                }
                else if (_previousPlayer2Progress <= SOUFFLI_TRESHOLD && Player2.Progress > SOUFFLI_TRESHOLD
                    || _previousPlayer2Progress >= ASPI_TRESHOLD && Player2.Progress < ASPI_TRESHOLD)
                {
                    SetPlayer2State(PlayerState.SelectingNothing);
                }

                if (InputManager.Instance.GetInputAccept(2))
                {
                    switch (_player2State)
                    {
                        case PlayerState.SelectingSouffli:
                            SetPlayer2State(PlayerState.PickedSouffli);
                            break;
                        case PlayerState.SelectingAspi:
                            SetPlayer2State(PlayerState.PickedAspi);
                            break;
                    }
                }
            }

            _previousPlayer1Progress = Player1.Progress;
            _previousPlayer2Progress = Player2.Progress;
        }

        //TODO: Have a separate script for the Aspi selector and the Souffli selector, that would inherit from this script
    }

    private IEnumerator Player1ChooseSouffli(float duration)
    {
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

    private void SetPlayer1State(PlayerState newState)
    {
        switch (newState)
        {
            case PlayerState.SelectingNothing:
                SouffliBeam.Activate(false);
                SouffliAButton1.SetActive(false);

                AspiBeam.Activate(false);
                AspiAButton1.SetActive(false);
                break;
            case PlayerState.SelectingSouffli:
                SouffliBeam.StartObject = Player1.transform;

                SouffliBeam.Activate(true);
                SouffliAButton1.SetActive(true);
                break;
            case PlayerState.SelectingAspi:
                AspiBeam.StartObject = Player1.transform;

                AspiBeam.Activate(true);
                AspiAButton1.SetActive(true);
                break;
            case PlayerState.PickedSouffli:
                if (_player2State == PlayerState.PickedAspi)
                {
                    SetGameReadyState(true);
                }

                SouffliAButton1.SetActive(false);
                SouffliBeam.Activate(false);

                StartCoroutine(Player1ChooseSouffli(0.3f));
                break;
            case PlayerState.PickedAspi:
                if (_player2State == PlayerState.PickedSouffli)
                {
                    SetGameReadyState(true);
                }

                AspiAButton1.SetActive(false);
                AspiBeam.Activate(false);

                StartCoroutine(Player1ChooseAspi(0.3f));
                break;
        }

        _player1State = newState;
    }

    private void SetPlayer2State(PlayerState newState)
    {
        switch (newState)
        {
            case PlayerState.SelectingNothing:
                SouffliBeam.Activate(false);
                SouffliAButton2.SetActive(false);

                AspiBeam.Activate(false);
                AspiAButton2.SetActive(false);
                break;
            case PlayerState.SelectingSouffli:
                SouffliBeam.StartObject = Player2.transform;

                SouffliBeam.Activate(true);
                SouffliAButton2.SetActive(true);
                break;
            case PlayerState.SelectingAspi:
                AspiBeam.StartObject = Player2.transform;

                AspiBeam.Activate(true);
                AspiAButton2.SetActive(true);
                break;
            case PlayerState.PickedSouffli:
                if (_player1State == PlayerState.PickedAspi)
                {
                    SetGameReadyState(true);
                }

                SouffliAButton1.SetActive(false);
                SouffliBeam.Activate(false);

                StartCoroutine(Player2ChooseSouffli(0.3f));
                break;
            case PlayerState.PickedAspi:
                if (_player1State == PlayerState.PickedSouffli)
                {
                    SetGameReadyState(true);
                }

                AspiAButton2.SetActive(false);
                AspiBeam.Activate(false);

                StartCoroutine(Player2ChooseAspi(0.3f));
                break;
        }

        _player2State = newState;
    }

    private void SetGameReadyState(bool state)
    {
        StartButton.SetActive(state);
        _gameIsReady = state;
    }
}
