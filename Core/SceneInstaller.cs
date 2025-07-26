using UnityEngine;
using Zenject;
using Data;
using UI;
namespace Core
{
    /// <summary>
    /// Scene에 들어올때 바인딩 하는 목록을 정하는 클레스
    /// </summary>
    public class SceneInstaller : MonoInstaller
    {
        public override void InstallBindings() {

            // Tag를 통해 원하는것을 바인딩
            Container.Bind<IMainCanvasTag>().To<MainCanvasTag>().FromComponentInHierarchy().AsCached(); // 메인 캔버스 바인딩
          
        }
    }
}
