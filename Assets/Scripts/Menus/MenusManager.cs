using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MenusManager : MonoBehaviour
{
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
        _instance = this;

        _menus = new Dictionary<string, Menu>();
        _cachedMenus = new Dictionary<string, Menu>();

        foreach (Menu menu in Menus)
        {
            _menus[menu.name] = menu;
        }
    }

    public void ShowMenu(string name)
    {
        if (_currentMenu != null)
        {
            _currentMenu.Close();
        }

        if (!_cachedMenus.ContainsKey(name) || _cachedMenus[name] == null)
        {
            _cachedMenus[name] = Instantiate(_menus[name], Vector3.zero, Quaternion.identity) as Menu;
            DontDestroyOnLoad(_cachedMenus[name]);
            _cachedMenus[name].name = name;
        }

        _currentMenu = _cachedMenus[name];

        _currentMenu.Open();
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
        // When we load a new level, we close currently active menu if it's still open
        if (_currentMenu != null)
        {
            _currentMenu.Close();
        }
    }
}
