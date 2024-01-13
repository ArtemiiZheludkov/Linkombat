using TMPro;
using UnityEngine;

namespace PuzzlePaint
{
    public class TileCube : MonoBehaviour
    {
        public int Row { get; private set; }
        public int Column { get; private set; }
        public bool IsPainted { get; private set; }
        public int PointNumber { get; private set; }
        
        public TileType MyType { get; private set; }
        
        [SerializeField] private Renderer _renderer;
        [SerializeField] private TextMeshPro _numberText;
        
        private MaterialPropertyBlock _materialBlock;
        private Color _standardColor;
        private Color _fillColor;
        
        public void Init(int row, int column, TileType type, Color standardColor, Color fillColor)
        {
            MyType = type;
            Row = row;
            Column = column;
            _standardColor = standardColor;
            _fillColor = fillColor;
            
            if (_renderer == null)
                _renderer = GetComponentInChildren<Renderer>();
            
            _materialBlock = new MaterialPropertyBlock();

            if (_numberText == null)
                _numberText = GetComponentInChildren<TextMeshPro>();
            
            _numberText.gameObject.SetActive(false);

            Clear();
            
            if (MyType == TileType.Finish)
                RotateText();
        }

        public void Paint()
        {
            _materialBlock.SetColor("_Color", _fillColor);
            _renderer.SetPropertyBlock(_materialBlock);
            IsPainted = true;
        }

        public void SetNumber(int number)
        {
            PointNumber = number;
            _numberText.text = PointNumber.ToString();
            _numberText.gameObject.SetActive(true);
        }

        public void Clear()
        { 
            _materialBlock.SetColor("_Color", _standardColor);
            _renderer.SetPropertyBlock(_materialBlock);
            IsPainted = false;

            if (MyType == TileType.Block)
                IsPainted = true;
        }

        private void RotateText()
        {
            Vector3 origRot = _numberText.transform.localEulerAngles;
            
            if (transform.position.x > 0f)
                origRot.y += 15f;
            else if (transform.position.x < 0f)
                origRot.y -= 15f;
            else
                return;
            
            _numberText.transform.localEulerAngles = origRot;
        }
    }
}