using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace MenusHandler
{
    public class MenusManager : MonoBehaviour
    {
        public Action OnMenusOpened;
        public Action OnMenusClosed;

        public Menu[] Menus;

        private static MenusManager _instance;
        private Dictionary<string, Menu> _menus;
        private Dictionary<string, Menu> _cachedMenus;
        private Menu _currentMenu;

        public static MenusManager Instance
        {
            get { return _instance; }
        }

        void Awake()
        {
            if (Instance)
            {
                Destroy(gameObject);
            }
            else
            {
                _instance = this;

                _menus = new Dictionary<string, Menu>();
                _cachedMenus = new Dictionary<string, Menu>();

                foreach (Menu menu in Menus)
                {
                    _menus[menu.name] = menu;
                }
            }
        }

        public void ShowMenu(string name)
        {
            if (_currentMenu != null)
            {
                CloseCurrentMenu();
            }

            if (!_cachedMenus.ContainsKey(name) || _cachedMenus[name] == null)
            {
                _cachedMenus[name] = Instantiate(_menus[name], Vector3.zero, Quaternion.identity) as Menu;
                DontDestroyOnLoad(_cachedMenus[name]);
                _cachedMenus[name].name = name;
            }

            _currentMenu = _cachedMenus[name];

            OpenCurrentMenu();
        }

        public void RequestClose()
        {
            CloseCurrentMenu();
        }

        public void SetInputValues(bool acceptButton, bool backButton, float horizontalAxis, float verticalAxis)
        {
            if (_currentMenu != null)
            {
                _currentMenu.InputModule.SetInputValues(acceptButton, backButton, horizontalAxis, verticalAxis);
            }
        }

        void OnDestroy()
        {
            foreach (KeyValuePair<string, Menu> kvp in _cachedMenus)
            {
                if (kvp.Value != null)
                {
                    Destroy(kvp.Value.gameObject);
                }
            }
        }

        void OnLevelWasLoaded(int levelIndex)
        {
            // When we load a new level, we close the currently active menu if it's still open
            CloseCurrentMenu();
        }

        // Can change based on whether we simply hide the menus or destroy them
        private void OpenCurrentMenu()
        {
            _currentMenu.gameObject.SetActive(true);
            _currentMenu.Open();

            if (OnMenusOpened != null)
            {
                OnMenusOpened();
            }
        }

        // Can change based on whether we simply hide the menus or destroy them
        private void CloseCurrentMenu()
        {
            if (_currentMenu != null && _currentMenu.gameObject.activeSelf)
            {
                _currentMenu.Close();
                _currentMenu.gameObject.SetActive(false);

                if (OnMenusClosed != null)
                {
                    OnMenusClosed();
                }
            }
        }
    }
}