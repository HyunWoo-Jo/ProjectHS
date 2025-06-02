using UI;
using UnityEngine;
using Zenject;
namespace Core
{
    // UI를 바인드
    public class ProjectUIInstaller : MonoInstaller
    {
        public override void InstallBindings() {
            Container.BindInterfacesAndSelfTo<CrystalViewModel>().AsSingle().NonLazy(); // Crystal Bind
        }

    }
}
