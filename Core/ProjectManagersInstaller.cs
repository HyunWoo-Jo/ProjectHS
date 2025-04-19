using Data;
using UnityEngine;
using GamePlay;
using Zenject;
namespace Core
{
    /// <summary>
    /// Project의 의존성을 주입해주는 클레스
    /// </summary>
    public class ProjectManagersInstaller : MonoInstaller
    {
        public override void InstallBindings() {
            // 데이터 바인딩 Manager를 바인딩함
            // GameManager 바인딩
            Container.Bind<GameManager>().FromNewComponentOnNewPrefab(this.gameObject).AsSingle().NonLazy();
            // DataManager 바인딩
            Container.Bind<DataManager>().FromNewComponentOnNewPrefab(this.gameObject).AsSingle().NonLazy();
        }
    }
}
