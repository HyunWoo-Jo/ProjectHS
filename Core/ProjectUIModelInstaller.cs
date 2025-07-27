using Data;
using Domain;
using UI;
using UnityEngine;
using Zenject;
namespace Core
{
    /// <summary>
    /// UI를 바인드
    /// </summary>
    public class ProjectUIInstaller : MonoInstaller
    {
        public override void InstallBindings() {

            Container.Bind<RarityColorStyleSO>().FromNewScriptableObjectResource("Style/RarityColorStyle").AsSingle();

            Container.Bind<CrystalViewModel>().AsSingle();
            Container.Bind<CrystalModel>().AsSingle();

            Container.Bind<GlobalUpgradeModel>().AsSingle();
        }

    }
}
