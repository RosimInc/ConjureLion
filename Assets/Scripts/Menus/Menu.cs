using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace MenusHandler
{
    [RequireComponent(typeof(MenuInputModule))]
    public abstract class Menu : MonoBehaviour
    {
        private MenuInputModule _inputModule;

        public MenuInputModule InputModule
        {
            get { return _inputModule; }
        }

        void Awake()
        {
            _inputModule = GetComponent<MenuInputModule>();
        }

        protected virtual void Start() { }

        public virtual void Open() { }

        public virtual void Close() { }
    }
}
