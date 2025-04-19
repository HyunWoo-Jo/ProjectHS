using UnityEngine;
using Data;
using Zenject;
using System.ComponentModel;
namespace Core
{
    // �ֿ� Repo�� ���ε� �ϴ� Ŭ����
    public class ProjectRepoInstaller : MonoInstaller
    {
        public override void InstallBindings() {
            // repo ����
            Container.Bind<IMoneyRepository>().To<MoneyRepository>().AsSingle().NonLazy();
        }


    }
}
