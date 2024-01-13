using System;
using System.Collections.Generic;
using UnityEngine;

namespace PuzzlePaint
{
    public class Player : MonoBehaviour
    {
        [SerializeField] private BrushDetector _detector;
        [SerializeField] private PlayerModel _model;
        [SerializeField] private float _offsetY;

        private Queue<TileCube> _path;
        private TileCube _lastTile;

        public void Init(Transform startTile, Camera mainCamera, Action<TileCube> tryPaint)
        {
            enabled = true;
            gameObject.SetActive(true);

            _path = new Queue<TileCube>();
            _model.Init(startTile);
            
            _detector.transform.position = new Vector3(startTile.position.x, _offsetY, startTile.position.z);
            _detector.Init(mainCamera, tryPaint, OnDetectorMouseUp);
        }
        
        public void UpdateDetectorPosition(TileCube tile)
        {
            _detector.transform.position = new Vector3(
                tile.transform.position.x, _offsetY, tile.transform.position.z);

            if (_lastTile != tile)
                _path.Enqueue(tile);
        }

        public void Stop()
        {
            _model.Stop();
            _detector.Stop();
        }
        
        public void MoveToFinish(Action<int> finished)
        {
            _detector.Stop();
            _model.FinishCallBack(finished);
            OnDetectorMouseUp();
        }

        private void OnDetectorMouseUp()
        {
            _model.Run(_path);
            _path.Clear();
        }
    }
}