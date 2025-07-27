using UnityEngine;

namespace Contracts
{
    public interface ISceneTransitionService {
        public void LoadScene(SceneName sceneName);
    }
}
