using UnityEngine;
using Zenject;
namespace UI
{

    /// <summary>
    /// ���� �ɶ� UI Event�� �߻��ϴ� Ŭ����
    /// </summary>
    public class AutoReleaseUI : MonoBehaviour
    {
        [Inject] private UIEvent _uiEvent;

        private void OnDestroy() {
            Close();
        }

        public void Close() {
            _uiEvent.CloseUI(this.gameObject); // ���� �̺�Ʈ �߻�
        }


    }
}
