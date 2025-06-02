using UnityEngine;
using Data;
using Zenject;
using System.ComponentModel;
using Network;
namespace Core
{
    // �ֿ� Repo, Data�� ���ε� �ϴ� Ŭ����
    public class ProjectRepoInstaller : MonoInstaller
    {
        public override void InstallBindings() {
            // repo ����
            Container.Bind<ICrystalRepository>().To<CrystalFirebaseRepository>().AsSingle().NonLazy();

            Container.Bind<IUserAuthRepository>().To<UserAuthRepositoryFirebase>().AsSingle().NonLazy();
            Container.Bind<IGlobalUpgradeRepository>().To<GlobalUpgradeFirebaseRepository>().AsSingle().NonLazy();
        }


    }
}
