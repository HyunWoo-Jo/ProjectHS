using UnityEngine;
using Zenject;
namespace UI
{

    /// <summary>
    /// 제거 될때 UI Event를 발생하는 클레스
    /// </summary>
    public class AutoReleaseUI : MonoBehaviour
    {
        [Inject] private UIEvent _uiEvent;

        private void OnDestroy() {
            Close();
        }

        public void Close() {
            _uiEvent.CloseUI(this.gameObject); // 제거 이벤트 발생
        }


    }
}
