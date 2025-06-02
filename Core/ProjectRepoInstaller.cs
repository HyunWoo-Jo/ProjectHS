using UnityEngine;
using Data;
using Zenject;
using System.ComponentModel;
using Network;
namespace Core
{
    // 주요 Repo, Data를 바인딩 하는 클레스
    public class ProjectRepoInstaller : MonoInstaller
    {
        public override void InstallBindings() {
            // repo 생성
            Container.Bind<ICrystalRepository>().To<CrystalFirebaseRepository>().AsSingle().NonLazy();

            Container.Bind<IUserAuthRepository>().To<UserAuthRepositoryFirebase>().AsSingle().NonLazy();
            Container.Bind<IGlobalUpgradeRepository>().To<GlobalUpgradeFirebaseRepository>().AsSingle().NonLazy();
        }


    }
}
