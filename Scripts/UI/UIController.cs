using System;
using System.Collections.Generic;
using UnityEngine;

namespace PuzzlePaint
{
    public class UIController : MonoBehaviour
    {
        [SerializeField] private GamePanel _game;
        [SerializeField] private WinPanel _win;
        [SerializeField] private LosePanel _lose;

        private Dictionary<Type, UIPanel> _panels;

        public void Init()
        {
            _panels = new Dictionary<Type, UIPanel>();
            _panels.Add(typeof(GamePanel), _game);
            _panels.Add(typeof(WinPanel), _win);
            _panels.Add(typeof(LosePanel), _lose);
        }

        public void ActivatePanel<T>() where T : UIPanel
        {
            DeactivatePanels();
            _panels[typeof(T)].Activate();
        }

        public void UpdatePanel<T>() where T : UIPanel
        {
            _panels[typeof(T)].UpdatePanel();
        }

        private void DeactivatePanels()
        {
            foreach(UIPanel panel in gameObject.GetComponentsInChildren<UIPanel>()) 
            {
                panel.gameObject.SetActive(false);
            }
        }
    }
}