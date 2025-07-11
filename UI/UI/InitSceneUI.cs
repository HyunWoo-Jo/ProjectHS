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
        private float _pointUpdateTime = 1f; // 점이 갱싱되는 시간
        private float _curTime = 0f; // 현재시간

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
            // 게임이 멈췄나 확인하기위한 Text의 이동을 구현
            _curTime += Time.deltaTime;
            if(_curTime >= _pointUpdateTime) { // 시간 확인
                _curTime -= _pointUpdateTime;
                if (stopCheckPointStr.Length < _MaxPointCount) stopCheckPointStr += "."; // . 추가
                else stopCheckPointStr = "."; // 초기화
                SetText();
            }
        }
    }
}
