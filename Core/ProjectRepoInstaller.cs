using UnityEngine;
using Data;
using Zenject;
using System.ComponentModel;
using Network;
using Contracts;
using Infrastructure;
namespace Core
{
    // 주요 Repo, Data를 바인딩 하는 클레스
    public class ProjectRepoInstaller : MonoInstaller
    {
        public override void InstallBindings() {

            Container.Bind<ICrystalRepository>().To<CrystalFirebaseRepository>().AsSingle();
            Container.Bind<IGlobalUpgradeRepository>().To<GlobalUpgradeFirebaseRepository>().AsSingle();

            // Data Bind
            Container.Bind<GlobalUpgradeTableSO>().FromScriptableObjectResource("GlobalUpgradeTableSO").AsSingle().NonLazy();
        }


    }
}
