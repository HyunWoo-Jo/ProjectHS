using UI;
using UnityEngine;
using Zenject;
namespace Core
{
    // UI�� ���ε�
    public class ProjectUIInstaller : MonoInstaller
    {
        public override void InstallBindings() {
            Container.Bind<MoneyViewModel>().AsSingle().NonLazy(); // Money Bind
        }

    }
}
