using Data;
using UI;
using UnityEngine;
using Zenject;
namespace Core
{
    // UI�� ���ε�
    public class ProjectUIInstaller : MonoInstaller
    {
        public override void InstallBindings() {

            Container.Bind<RarityColorStyleSO>().FromNewScriptableObjectResource("Style/RarityColorStyle").AsSingle();

            Container.BindInterfacesAndSelfTo<CrystalViewModel>().AsSingle().NonLazy(); // Crystal Bind
        }

    }
}
