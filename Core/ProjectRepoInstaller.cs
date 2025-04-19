using UnityEngine;
using Data;
using Zenject;
using System.ComponentModel;
namespace Core
{
    // 주요 Repo를 바인딩 하는 클레스
    public class ProjectRepoInstaller : MonoInstaller
    {
        public override void InstallBindings() {
            // repo 생성
            Container.Bind<IMoneyRepository>().To<MoneyRepository>().AsSingle().NonLazy();
        }


    }
}
