﻿using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using InputHandling;

public class MenuInputModule : BaseInputModule
{
    public MenuButton[] Buttons;

    private int _buttonIndex = 0;

    private bool _canNavigate = true;

    private GameObject _previousTargettedObject;

    private bool _acceptButtonPressed = false;
    private bool _menuDownPressed = false;
    private bool _menuUpPressed = false;

    void Start()
    {
        InputManager.Instance.AddCallback(MenuInputModuleCallback);
    }

    public override void ActivateModule()
    {
        base.ActivateModule();
        
        SelectFirstButton();
        _canNavigate = true;
    }

    public override void Process()
    {
        if (_acceptButtonPressed)
        {
            ExecuteEvents.Execute(Buttons[_buttonIndex].gameObject, new BaseEventData(eventSystem), ExecuteEvents.submitHandler);
        }

        if (_canNavigate)
        {
            if (_menuDownPressed)
            {
                SelectNextButton();
            }
            else if (_menuUpPressed)
            {
                SelectPreviousButton();
            }
        }

        ProcessMouseSelect();
    }

    public void SelectFirstButton()
    {
        if (Buttons.Length == 0) return;

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

        StartCoroutine("PauseNavigation");
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

    private void ProcessMouseSelect()
    {
        /*
        MouseState mouseData = GetMousePointerEventData();
        
        ButtonState mouseState = mouseData.GetButtonState(PointerEventData.InputButton.Left);

        GameObject targettedObject = GetButtonParent(mouseState.eventData.buttonData.pointerCurrentRaycast.gameObject);

        if (targettedObject != _previousTargettedObject)
        {
            ExecuteEvents.ExecuteHierarchy(_previousTargettedObject, new BaseEventData(eventSystem), ExecuteEvents.deselectHandler);
        }
        
        ExecuteEvents.Execute(targettedObject, new BaseEventData(eventSystem), ExecuteEvents.selectHandler);
        
        _previousTargettedObject = targettedObject;*/
    }

    private GameObject GetButtonParent(GameObject child)
    {
        if (child == null) return null;

        Transform go = child.transform;

        do
        {
            if (go.GetComponent<Button>() != null)
            {
                return go.gameObject;
            }

            go = go.parent;
        } while (go != null);

        return null;
    }

    private void MenuInputModuleCallback(MappedInput mappedInput)
    {
        // TODO: Temporary, until I figure out a better way to handle multiple player inputs for the menus
        if (mappedInput.PlayerIndex == 1)
        {
            return;
        }

        _acceptButtonPressed = mappedInput.Actions[ActionsConstants.Actions.AcceptMenuOption];
        _menuDownPressed = mappedInput.Ranges[ActionsConstants.Ranges.ChangeMenuOption] < -0.5f;
        _menuUpPressed = mappedInput.Ranges[ActionsConstants.Ranges.ChangeMenuOption] > 0.5f;
    }
}
