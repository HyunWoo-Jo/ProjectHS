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
    /// 씬 로드를 관리하는 클레스
    /// </summary>
    public class LoadManager : MonoBehaviour, ILoadManager {
        private AsyncOperation _operation;
        public SceneName PreScene { get; private set; } = SceneName.MainLobbyScene;
        public SceneName NextScene { get; private set; } = SceneName.MainLobbyScene;


        public float GetLoadingRation() {
            return _operation != null ? _operation.progress : 0;
        }
        /// <summary>
        /// 씬 로드 
        /// </summary>
        /// <param name="nextScene"></param>
        /// <param name="delay"></param>
        public void LoadScene(SceneName nextScene, float delay) {
            NextScene = nextScene; // 다음 씬 이름 저장
            _operation = SceneManager.LoadSceneAsync(SceneName.LoadScene.ToString()); // LoadScene으로 이동        
            _operation.completed += (op) => { // 로딩이 완료되면 NextScene을 로딩하는 코루틴 실행
                StartCoroutine(LoadNextSceneCoroutine());
            };
            if (delay > 0) { // 딜레이가 있으면 
                _operation.allowSceneActivation = false; // 자동 로드 x
                StartCoroutine(DelayAllowLoadCoroutine(_operation, delay)); // 딜레이후 로드가 실행되는 코루틴 실행
            } else {
                _operation.allowSceneActivation = true;
            }
            


        }
        /// <summary>
        /// 시간이 지나면 자동로드를 켜주는 함수
        /// </summary>
        /// <param name="operation"></param>
        /// <param name="delay"></param>
        /// <returns></returns>
        private IEnumerator DelayAllowLoadCoroutine(AsyncOperation operation, float delay) {
            yield return new WaitForSeconds(delay);
            if (operation != null) operation.allowSceneActivation = true;
        }

        /// <summary>
        /// 로딩 씬에서 호출되는 함수 
        /// </summary>
        /// <returns></returns>
        private IEnumerator LoadNextSceneCoroutine() {
            while (true) {
                if (_operation != null && _operation.allowSceneActivation) {
                    float minLoadTime = 2f; // 최소 로드 시간 로딩씬에서 대기 시간 
                    _operation = SceneManager.LoadSceneAsync(NextScene.ToString()); // 다음씬 로드
                    _operation.allowSceneActivation = false; // 자동로드 x
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
