using UnityEngine;
using System;
using DG.Tweening;
using System.Collections.Generic;
namespace UI
{

    public enum UIAnimationMode {
        Open,
        Close
    }

    /// <summary>
    /// 지원되는 애니메이션 타입
    /// </summary>
    public enum UIAnimationType {
        Scale,
        ScaleX,
        ScaleY,
        MoveX,
        MoveY,
        Fade
    }

    [Serializable]
    public struct UIAnimationOption {
        public UIAnimationType type;
        public float value;      // 값
        public float duration;   // 애니메이션 시간
        [Range(0, 5f)] public float delay;       // 개별 딜레이
        public Ease ease;        
    }

    [DisallowMultipleComponent]
    public sealed class UIAnimation : MonoBehaviour {
        [SerializeField] private UIAnimationMode _mode = UIAnimationMode.Open;
        [SerializeField] private List<UIAnimationOption> _options = new();
        [SerializeField] private bool _playOnEnable = true;

        private Sequence _sequence;
        private Vector3 _originPos;
        private Vector3 _originScale;
        private CanvasGroup _canvasGroup; // Fade 전용 없으면 자동 추가

        private event Action _onCompleted;

        // ---------- Life‑cycle ----------
        private void Awake() {
            _originPos = transform.localPosition;
            _originScale = transform.localScale;
            _canvasGroup = GetComponent<CanvasGroup>();
        }

        private void OnEnable() {
            if (_playOnEnable) Play();
        }

        private void OnDisable() {
            ResetTransform();
            KillTweens();
        }

        private void OnDestroy() {
            KillTweens();
        }

        /// <summary>
        /// 애니메이션을 즉시 실행
        /// </summary>
        public void Play(Action onComplete = null) {
            if (onComplete != null) _onCompleted += onComplete;
            BuildSequence();
            _sequence.Play();
        }

        public void AddOnComplete(Action handler) => _onCompleted += handler;

        /// <summary>
        /// 과정
        /// </summary>
        private void BuildSequence() {
            KillTweens(); // 실행중인 Tween 제거
            _sequence = DOTween.Sequence().SetLink(gameObject); // GameObject 소멸 시 자동 Kill
            foreach (var opt in _options) {
                var tween = CreateTween(opt);
                if (tween != null) _sequence.Insert(opt.delay, tween);
            }
            _sequence.OnComplete(() => _onCompleted?.Invoke());
        }

        /// <summary>
        /// Animation 상세 구현부
        /// </summary>
        private Tween CreateTween(in UIAnimationOption opt) {
            switch (opt.type) {
                case UIAnimationType.Scale:
                transform.localScale = _mode == UIAnimationMode.Open ? Vector3.zero : _originScale;
                return transform.DOScale(_mode == UIAnimationMode.Open ? _originScale : Vector3.zero, opt.duration)
                                 .SetEase(opt.ease);

                case UIAnimationType.ScaleX:
                transform.localScale = _mode == UIAnimationMode.Open ? new Vector3(0, _originScale.y, _originScale.z) : _originScale;
                return transform.DOScaleX(_mode == UIAnimationMode.Open ? _originScale.x : 0, opt.duration)
                                 .SetEase(opt.ease);

                case UIAnimationType.ScaleY:
                transform.localScale = _mode == UIAnimationMode.Open ? new Vector3(_originScale.x, 0, _originScale.z) : _originScale;
                return transform.DOScaleY(_mode == UIAnimationMode.Open ? _originScale.y : 0, opt.duration)
                                 .SetEase(opt.ease);

                case UIAnimationType.MoveX: {
                    float startX = _originPos.x + (_mode == UIAnimationMode.Open ? -opt.value : 0);
                    transform.localPosition = new Vector3(startX, _originPos.y, _originPos.z);
                    float endX = _mode == UIAnimationMode.Open ? _originPos.x : _originPos.x + opt.value;
                    return transform.DOLocalMoveX(endX, opt.duration).SetEase(opt.ease);
                }

                case UIAnimationType.MoveY: {
                    float startY = _originPos.y + (_mode == UIAnimationMode.Open ? -opt.value : 0);
                    transform.localPosition = new Vector3(_originPos.x, startY, _originPos.z);
                    float endY = _mode == UIAnimationMode.Open ? _originPos.y : _originPos.y + opt.value;
                    return transform.DOLocalMoveY(endY, opt.duration).SetEase(opt.ease);
                }

                case UIAnimationType.Fade: {
                    if (_canvasGroup == null) _canvasGroup = gameObject.AddComponent<CanvasGroup>();
                    _canvasGroup.alpha = _mode == UIAnimationMode.Open ? 0f : 1f;
                    return _canvasGroup.DOFade(_mode == UIAnimationMode.Open ? 1f : 0f, opt.duration).SetEase(opt.ease);
                }
            }
            return null;
        }

        private void KillTweens() {
            _sequence?.Kill();
            _sequence = null;
        }

        private void ResetTransform() {
            transform.localPosition = _originPos;
            transform.localScale = _originScale;
            if (_canvasGroup) _canvasGroup.alpha = 1f;
        }
    }
}
