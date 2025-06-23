using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using static PlasticPipe.PlasticProtocol.Messages.Serialization.ItemHandlerMessagesSerialization;
namespace UI
{
    public class InitSceneUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _text;
        private string stopCheckPointStr = ".";
        private string _tempText;
        private const int _MaxPointCount = 3;
        private float _pointUpdateTime = 1f; // ���� ���̵Ǵ� �ð�
        private float _curTime = 0f; // ����ð�

        private void SetText(){
            _text.text = _tempText + stopCheckPointStr; 
        }

        public void UpdateText(string text) {
            _tempText = text;
            SetText();
        }

        public async void UpdateTextFromThread(string text) {
            _tempText = text;
            await UniTask.SwitchToMainThread();
            SetText();
        }
        
        private void Update() {
            // ������ ���質 Ȯ���ϱ����� Text�� �̵��� ����
            _curTime += Time.deltaTime;
            if(_curTime >= _pointUpdateTime) { // �ð� Ȯ��
                _curTime -= _pointUpdateTime;
                if (stopCheckPointStr.Length < _MaxPointCount) stopCheckPointStr += "."; // . �߰�
                else stopCheckPointStr = "."; // �ʱ�ȭ
                SetText();
            }
        }
    }
}
