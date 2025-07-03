using Data;
using ModestTree;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI
{
    /// <summary>
    /// �ε�� UI
    /// </summary>
    public class LoadSceneUI : MonoBehaviour
    {
        [Inject] private LoadManager _loadManager;
        [SerializeField] private Image _fillImage;
        [SerializeField] private TextMeshProUGUI _loadProgressText;

        [SerializeField] private float _fakeLoadTime = 2f; // �ּ� �ε� �ð�
        private float _startTime = 0; // ���۽ð�
        private float _fakeProgress = 0; // ��¥ �ε� 
        private void Awake() {
#if UNITY_EDITOR
            Assert.IsNotNull(_fillImage);
#endif
        }

        private void Update() {
            // _startTime�� 0�̸� ���� �ð����� �ʱ�ȭ
            if (_startTime <= 0f) _startTime = Time.time;

            // ���� �ε� �����
            float realProgress = _loadManager.GetLoadingRation();

            // ��� �ð� ���
            float elapsedTime = Time.time - _startTime;

            // �ּ� �ε� �ð����� ª�� ��� ��¥ �ε� ����
            bool isFakeLoading = elapsedTime < _fakeLoadTime; 

            // ����� ����
            float progress;

            if (isFakeLoading) {
                _fakeProgress = Mathf.MoveTowards(_fakeProgress, 1f, Time.deltaTime / _fakeLoadTime);
                progress = Mathf.Min(realProgress, _fakeProgress);
            } else {
                progress = realProgress;
            }

            // �ε尡 ������ ���� ������ 1�� ǥ��
            if(progress >= 0.89f) progress = 1f;

            // UI�� ����
            _fillImage.fillAmount = progress;
            _loadProgressText.text = $"{Mathf.RoundToInt(progress * 100f)}%";
        }


    }
}
