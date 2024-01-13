using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace PuzzlePaint
{
    public class PlayerModel : MonoBehaviour
    {
        [SerializeField] private float _speed;
        [SerializeField] private Animator _animator;
        [SerializeField] private TextMeshPro _pointText;

        private Action<int> _finished;

        private Queue<TileCube> _path;
        private TileCube _target;
        private bool _move;
        
        private int _points;

        private readonly HashAnimation _animations = new HashAnimation();

        public void Init(Transform tile)
        {
            _path = new Queue<TileCube>();

            SetIdleState(tile);

            _points = 1;
            _pointText.text = _points.ToString();
            SetTestRotation();
        }
        
        private void FixedUpdate()
        {
            if (_move == true)
            {
                transform.position = Vector3.MoveTowards(
                    transform.position, 
                    _target.transform.position,
                    _speed * Time.fixedDeltaTime);

                if (Vector3.Distance(transform.position, _target.transform.position) < 0.001f)
                {
                    _points += 1;
                    _pointText.text = _points.ToString();
                    SetTarget();
                }
            }
        }

        public void FinishCallBack(Action<int> finished)
        {
            _finished = finished;
        }

        public void Run(Queue<TileCube> path)
        {
            foreach (TileCube tile in path)
                _path.Enqueue(tile);

            if (_target == null)
                SetTarget();
        }

        public void Stop()
        {
            SetIdleState(transform);
        }

        private void SetIdleState(Transform tile)
        {
            _target = null;
            _finished = null;
            _move = false;
            _path.Clear();
            
            transform.position = tile.position;
            transform.LookAt(tile);
            _animator.CrossFade(_animations.Idle, 0.1f);
        }

        private void SetTarget()
        {
            if (_path.Count <= 0)
            {
                if (_target == null || _target.IsPainted == true)
                {
                    SetIdleState(gameObject.transform);
                    return;
                }
                
                if (_target.MyType == TileType.Finish)
                    _finished?.Invoke(_points);
                else
                    SetIdleState(_target.transform);
                
                return;
            }
            
            _move = true;
            _target = _path.Dequeue();
            transform.LookAt(_target.transform);
            SetTestRotation();
            _animator.CrossFade(_animations.Run, 0.1f);
        }

        private void SetTestRotation()
        {
            Vector3 origRot = _pointText.transform.localEulerAngles;
            origRot.y = -transform.localEulerAngles.y;
            _pointText.transform.localEulerAngles = origRot;
        }
    }
}