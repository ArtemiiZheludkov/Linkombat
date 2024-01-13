using UnityEngine;
using UnityEngine.UI;

namespace PuzzlePaint
{
    public class WinPanel : UIPanel
    {
        [SerializeField] private Button _nextLevel;
        
        private Game _game;

        private void OnDisable()
        {
            _nextLevel.onClick.RemoveAllListeners();
        }
        
        public override void Activate()
        {
            if (_game == null)
                _game = Game.Instance;
            
            gameObject.SetActive(true);

            _nextLevel.onClick.AddListener(OnNextClicked);
        }

        public override void UpdatePanel()
        {
        }

        private void OnNextClicked()
        {
            _game.NextLevel();
        }
    }
}