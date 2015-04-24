using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class MenuInputModule : BaseInputModule
{
    public MenuButton[] Buttons;

    private int _buttonIndex = 0;

    private bool _canNavigate = true;

    public override void ActivateModule()
    {
        base.ActivateModule();

        SelectFirstButton();
        _canNavigate = true;
    }

    public override void Process()
    {
        if (InputManager.Instance.GetInputMenuAccept())
        {
            ExecuteEvents.Execute(Buttons[_buttonIndex].gameObject, new BaseEventData(eventSystem), ExecuteEvents.submitHandler);
        }

        if (_canNavigate)
        {
            if (InputManager.Instance.GetInputMenuDown())
            {
                SelectNextButton();
            }
            else if (InputManager.Instance.GetInputMenuUp())
            {
                SelectPreviousButton();
            }
        }
    }

    private void SelectFirstButton()
    {
        if (_buttonIndex != 0)
        {
            ExecuteEvents.Execute(Buttons[_buttonIndex].gameObject, new BaseEventData(eventSystem), ExecuteEvents.deselectHandler);

            _buttonIndex = 0;
        }

        ExecuteEvents.Execute(Buttons[_buttonIndex].gameObject, new BaseEventData(eventSystem), ExecuteEvents.selectHandler);
    }

    private void SelectPreviousButton()
    {
        ExecuteEvents.Execute(Buttons[_buttonIndex].gameObject, new BaseEventData(eventSystem), ExecuteEvents.deselectHandler);

        _buttonIndex = _buttonIndex == 0 ? Buttons.Length - 1 : _buttonIndex - 1;

        ExecuteEvents.Execute(Buttons[_buttonIndex].gameObject, new BaseEventData(eventSystem), ExecuteEvents.selectHandler);

        StartCoroutine(PauseNavigation());
    }

    private void SelectNextButton()
    {
        ExecuteEvents.Execute(Buttons[_buttonIndex].gameObject, new BaseEventData(eventSystem), ExecuteEvents.deselectHandler);

        _buttonIndex = _buttonIndex == Buttons.Length - 1 ? 0 : _buttonIndex + 1;

        ExecuteEvents.Execute(Buttons[_buttonIndex].gameObject, new BaseEventData(eventSystem), ExecuteEvents.selectHandler);

        StartCoroutine(PauseNavigation());
    }

    private IEnumerator PauseNavigation()
    {
        _canNavigate = false;

        float elapsedTime = 0f;

        // Since the game can be paused, we can't do "yield return new WaitForSeconds(0.2f);"
        while (elapsedTime < 0.2f)
        {
            elapsedTime += Time.unscaledDeltaTime;

            yield return null;
        }

        _canNavigate = true;
    }
}
