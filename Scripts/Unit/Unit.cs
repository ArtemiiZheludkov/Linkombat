using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

namespace PuzzlePaint
{
    [RequireComponent(typeof(BoxCollider))]
    [RequireComponent(typeof(Rigidbody))]
    public class Unit : MonoBehaviour
    {
        public UnitType MyType { get; private set; }
        public bool IsAlive { get; private set; }

        [SerializeField] private float _speedStandard;
        [SerializeField] private float _speedDeviation;
        
        [Header("PREFAB COMPONENTS")]
        [SerializeField, Range(0, 9)] private int _maxAnimationOffset;
        [SerializeField] private Animator _animator;
        [SerializeField] private ParticleSystem _deadParticles;
        [SerializeField] private AudioSource _audioSource;

        private Action<Unit> _targetNull;
        private Action<UnitType> _died;
        
        private Unit _target;

        private readonly HashAnimation _animations = new HashAnimation();

        public void Init(UnitType myType, Unit target, Action<Unit> targetNull, Action<UnitType> died)
        {
            StopAllCoroutines();
            
            _targetNull = targetNull;
            _died = died;
            MyType = myType;

            _target = null;
            _speedStandard = Random.Range(_speedStandard - _speedDeviation, _speedStandard + _speedDeviation);
            transform.LookAt(target.transform);
            
            SetLiveStatus(true);
            PrepareToBattle();
        }
        
        private void FixedUpdate()
        {
            if (_target != null)
                MoveToTarget();
        }
        
        private void OnTriggerEnter(Collider other)
        {
            //physical layers for detecting
            
            if (other.TryGetComponent(out Unit unit) == true)
            {
                if (IsAlive == true && unit.IsAlive == true)
                {
                    Kill();
                    unit.Kill();
                }
            }
        }

        public void SetLiveStatus(bool isAlive)
        {
            IsAlive = isAlive;
            gameObject.SetActive(IsAlive);
        }
        
        public void StartBattle(Unit target)
        {
            if (IsAlive == false)
                return;
            
            SetNewTarget(target);
        }
        
        public void SetNewTarget(Unit target)
        {
            _target = target;
            transform.LookAt(_target.transform);
            
            float offset = 1f / Random.Range(1, _maxAnimationOffset);
            _animator.CrossFade(_animations.Run, 0.1f, 0, offset);
        }
        
        public void CelebrateVictory()
        {
            _target = null;
            float offset = 1f / Random.Range(1, _maxAnimationOffset);
            _animator.CrossFade(_animations.Dance, 0.1f, 0, offset);
        }
        
        public void Kill()
        {
            IsAlive = false;
            _target = null;
            GetComponent<BoxCollider>().enabled = false;
            StartCoroutine(DieAnimation());
        }

        private void PrepareToBattle()
        {
            if (IsAlive == false)
                return;
            
            int animHash = _animations.Fist;
            int randAnim = Random.Range(0, 10);

            if (randAnim > 5)
                animHash = _animations.Threatening;

            float offset = 1f / Random.Range(1, _maxAnimationOffset);
            _animator.CrossFade(animHash, 0.1f, 0, offset);
        }
        
        private void MoveToTarget()
        {
            float step = _speedStandard * Time.fixedDeltaTime;
            transform.position = Vector3.MoveTowards(transform.position, _target.transform.position, step);

            if (Vector3.Distance(transform.position, _target.transform.position) < 0.001f)
                TargetMissing();
        }

        private void TargetMissing()
        {
            _target = null;
            _targetNull?.Invoke(this);

            if (_target == null)
                _animator.CrossFade(_animations.Idle, 0.1f);
        }

        private IEnumerator DieAnimation()
        {
            _animator.CrossFade(_animations.Increase, 0.1f);

            float wait = 0f;
            AnimationClip[] clips = _animator.runtimeAnimatorController.animationClips;
            
            for (int i = 0; i < clips.Length; i++)
            {
                if(clips[i].name == "Increase")
                {
                    wait = clips[i].length;
                    break;
                }
            }

            yield return new WaitForSeconds(wait);
            
            _deadParticles.transform.parent = null;
            _deadParticles.gameObject.SetActive(true);
            _deadParticles.Play();
            //_audioSource.Play();
            _died?.Invoke(MyType);
            
            SetLiveStatus(false);
            StopAllCoroutines();
        }
    }
}