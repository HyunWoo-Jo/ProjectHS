using CustomUtility;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;

namespace UI
{
    public class DamageLogUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _text;
        private ObjectPoolItem _poolItem;
        private float timer;
        private void Awake() {
#if UNITY_EDITOR
            Assert.IsNotNull(_text);
#endif
        }

        private void OnDisable() {
            StopAllCoroutines();
            Color color = _text.color;
            color.a = 1f;
            _text.color = color;
        }

        public void SetDamage(int damage) {
            _text.text = damage.ToString();
            StartCoroutine(DelayRepay(1.5f));
            StartCoroutine(AlphaAnimation(1f));
        }
        private IEnumerator DelayRepay(float duration) {
            if (_poolItem == null) {
                if(TryGetComponent<ObjectPoolItem>(out var poolItem)) {
                    _poolItem = poolItem; // 할당
                } else { // 할당 실패
                    Destroy(_poolItem.gameObject);
                }  
            } else {
                yield return new WaitForSeconds(duration);
                _poolItem.Repay();
            }
        }
        private IEnumerator AlphaAnimation(float duration) {
            float elapsed = 0f;
            Color color = _text.color;

            while (elapsed < duration) {
                elapsed += Time.deltaTime;
                float t = Mathf.Clamp01(elapsed / duration);
                color.a = 1f - t;
                _text.color = color;
                yield return null;
            }
            color.a = 0f;
            _text.color = color;
        }

    }
}
