using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MouseInputModule : PointerInputModule
{
    public Button[] Buttons;

    private int _buttonIndex = 0;

    public override void ActivateModule()
    {
        base.ActivateModule();
        
        /*
        foreach (Button button in Buttons)
        {
            button.OnPointerEnter += () => { Debug.Log("Pointer enter!"); };
        }*/
    }

    public override void DeactivateModule()
    {
        base.DeactivateModule();
    }

    public override void Process()
    {
        Debug.Log(EventSystem.current.IsPointerOverGameObject());

        if (InputManager.Instance.GetInputMenuAccept())
        {
            ExecuteEvents.Execute(Buttons[_buttonIndex].gameObject, new BaseEventData(eventSystem), ExecuteEvents.submitHandler);
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
    }

    private void SelectNextButton()
    {
        ExecuteEvents.Execute(Buttons[_buttonIndex].gameObject, new BaseEventData(eventSystem), ExecuteEvents.deselectHandler);

        _buttonIndex = _buttonIndex == Buttons.Length - 1 ? 0 : _buttonIndex + 1;

        ExecuteEvents.Execute(Buttons[_buttonIndex].gameObject, new BaseEventData(eventSystem), ExecuteEvents.selectHandler);
    }
}
