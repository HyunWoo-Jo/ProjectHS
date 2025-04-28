using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
namespace Data
{
    public enum SceneName {
        PlayScene,
        MainLobbyScene,
        LoadScene,
    }

    public interface ILoadManager {
        void LoadScene(SceneName nextScene, float delay);
        float GetLoadingRation();
        SceneName GetPreSceneName();
        SceneName GetNextSceneName();
    }

    /// <summary>
    /// �� �ε带 �����ϴ� Ŭ����
    /// </summary>
    public class LoadManager : MonoBehaviour, ILoadManager {
        private AsyncOperation _operation;
        public SceneName PreScene { get; private set; } = SceneName.MainLobbyScene;
        public SceneName NextScene { get; private set; } = SceneName.MainLobbyScene;


        public float GetLoadingRation() {
            return _operation != null ? _operation.progress : 0;
        }
        /// <summary>
        /// �� �ε� 
        /// </summary>
        /// <param name="nextScene"></param>
        /// <param name="delay"></param>
        public void LoadScene(SceneName nextScene, float delay) {
            NextScene = nextScene; // ���� �� �̸� ����
            _operation = SceneManager.LoadSceneAsync(SceneName.LoadScene.ToString()); // LoadScene���� �̵�        
            _operation.completed += (op) => { // �ε��� �Ϸ�Ǹ� NextScene�� �ε��ϴ� �ڷ�ƾ ����
                StartCoroutine(LoadNextSceneCoroutine());
            };
            if (delay > 0) { // �����̰� ������ 
                _operation.allowSceneActivation = false; // �ڵ� �ε� x
                StartCoroutine(DelayAllowLoadCoroutine(_operation, delay)); // �������� �ε尡 ����Ǵ� �ڷ�ƾ ����
            } else {
                _operation.allowSceneActivation = true;
            }
            


        }
        /// <summary>
        /// �ð��� ������ �ڵ��ε带 ���ִ� �Լ�
        /// </summary>
        /// <param name="operation"></param>
        /// <param name="delay"></param>
        /// <returns></returns>
        private IEnumerator DelayAllowLoadCoroutine(AsyncOperation operation, float delay) {
            yield return new WaitForSeconds(delay);
            if (operation != null) operation.allowSceneActivation = true;
        }

        /// <summary>
        /// �ε� ������ ȣ��Ǵ� �Լ� 
        /// </summary>
        /// <returns></returns>
        private IEnumerator LoadNextSceneCoroutine() {
            while (true) {
                if (_operation != null && _operation.allowSceneActivation) {
                    float minLoadTime = 2f; // �ּ� �ε� �ð� �ε������� ��� �ð� 
                    _operation = SceneManager.LoadSceneAsync(NextScene.ToString()); // ������ �ε�
                    _operation.allowSceneActivation = false; // �ڵ��ε� x
                    StartCoroutine(DelayAllowLoadCoroutine(_operation, minLoadTime));
                    
                    break;
                }
                yield return null;
            }
        }

        public SceneName GetNextSceneName() {
            return NextScene;
        }

        public SceneName GetPreSceneName() {
            return PreScene;
        }
    }
}
