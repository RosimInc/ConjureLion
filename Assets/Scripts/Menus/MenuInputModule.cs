using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;
using UnityEngine.Events;

namespace MenusHandler
{
    [RequireComponent(typeof(EventSystem))]
    public class MenuInputModule : BaseInputModule
    {
        public float Delay = 0.2f;
        public float DeadZone = 0.5f;

        public UnityEvent BackButtonEvent;

        private EventSystem _eventSystem;

        private bool _canNavigate = true;

        private GameObject _previousTargettedObject;

        private bool _acceptButtonPressed = false;
        private bool _backButtonPressed = false;
        private bool _menuDownPressed = false;
        private bool _menuUpPressed = false;
        private bool _menuRightPressed = false;
        private bool _menuLeftPressed = false;

        protected override void Start()
        {
            base.Start();

            _eventSystem = GetComponent<EventSystem>();
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
                ExecuteEvents.Execute(_eventSystem.currentSelectedGameObject.gameObject, new BaseEventData(eventSystem), ExecuteEvents.submitHandler);
            }
            else if (_backButtonPressed)
            {
                BackButtonEvent.Invoke();
            }
            else if (_canNavigate)
            {
                if (_menuRightPressed)
                {
                    SelectRightButton();
                }
                else if (_menuLeftPressed)
                {
                    SelectLeftButton();
                }
                else if (_menuDownPressed)
                {
                    SelectDownButton();
                }
                else if (_menuUpPressed)
                {
                    SelectUpButton();
                }
            }
        }

        public void SetFirstButton(Selectable button)
        {
            _eventSystem.firstSelectedGameObject = button.gameObject;
        }

        public void SelectFirstButton()
        {
            if (_eventSystem.firstSelectedGameObject == null) return;

            if (_eventSystem.currentSelectedGameObject != _eventSystem.firstSelectedGameObject)
            {
                ExecuteEvents.Execute(_eventSystem.currentSelectedGameObject, new BaseEventData(eventSystem), ExecuteEvents.deselectHandler);
            }

            ExecuteEvents.Execute(_eventSystem.firstSelectedGameObject, new BaseEventData(eventSystem), ExecuteEvents.selectHandler);
            _eventSystem.SetSelectedGameObject(_eventSystem.firstSelectedGameObject);

            //ExecuteEvents.Execute(_eventSystem.firstSelectedGameObject, new BaseEventData(eventSystem), ExecuteEvents.selectHandler);
        }

        private void SelectLeftButton()
        {
            Selectable toBeSelected = _eventSystem.currentSelectedGameObject.GetComponent<Selectable>().FindSelectableOnLeft();

            if (toBeSelected != null)
            {
                _eventSystem.SetSelectedGameObject(toBeSelected.gameObject);
                StartCoroutine("PauseNavigation");
            }
        }

        private void SelectRightButton()
        {
            Selectable toBeSelected = _eventSystem.currentSelectedGameObject.GetComponent<Selectable>().FindSelectableOnRight();

            if (toBeSelected != null)
            {
                _eventSystem.SetSelectedGameObject(toBeSelected.gameObject);
                StartCoroutine("PauseNavigation");
            }
        }

        private void SelectUpButton()
        {
            Selectable toBeSelected = _eventSystem.currentSelectedGameObject.GetComponent<Selectable>().FindSelectableOnUp();

            if (toBeSelected != null)
            {
                _eventSystem.SetSelectedGameObject(toBeSelected.gameObject);
                StartCoroutine("PauseNavigation");
            }
        }

        private void SelectDownButton()
        {
            Selectable toBeSelected = _eventSystem.currentSelectedGameObject.GetComponent<Selectable>().FindSelectableOnDown();

            if (toBeSelected != null)
            {
                _eventSystem.SetSelectedGameObject(toBeSelected.gameObject);
                StartCoroutine("PauseNavigation");
            }
        }

        // Adds a delay before we can switch options again
        private IEnumerator PauseNavigation()
        {
            _canNavigate = false;

            float elapsedTime = 0f;

            // Since the game might be paused here, we can't do "yield return new WaitForSeconds(0.2f);"
            while (elapsedTime < Delay)
            {
                elapsedTime += Time.unscaledDeltaTime;

                yield return null;
            }

            _canNavigate = true;
        }

        public void SetInputValues(bool acceptButton, bool backButton, float horizontalAxis, float verticalAxis)
        {
            _acceptButtonPressed = acceptButton;
            _backButtonPressed = backButton;
            _menuDownPressed = verticalAxis < -DeadZone;
            _menuUpPressed = verticalAxis > DeadZone;
            _menuRightPressed = horizontalAxis > DeadZone;
            _menuLeftPressed = horizontalAxis < -DeadZone;
        }
    }
}