﻿using Data;
using UnityEngine;
using GamePlay;
using Zenject;
using UI;
using Network;
using Contracts;
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
            Container.Bind<GameManager>().FromNewComponentOn(this.gameObject).AsSingle().NonLazy();
            // DataManager 바인딩
            Container.Bind<DataManager>().FromNewComponentOn(this.gameObject).AsSingle().NonLazy();
            // UIManager 바인딩
            Container.Bind<UIEvent>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<UIManager>().FromNewComponentOn(this.gameObject).AsSingle().NonLazy();
            // LoadManager 바인딩
            Container.BindInterfacesAndSelfTo<LoadManager>().FromNewComponentOn(this.gameObject).AsSingle().NonLazy();
            Container.Bind<ISceneTransitionService>().To<SceneTransitionService>().AsCached();

            // Network 바인딩
            Container.Bind<INetworkLogic>().To<FirebaseLogic>().AsSingle().NonLazy(); // Network Logic을 결정하는 바인드 // Firebase를 사용

            Container.BindInterfacesAndSelfTo<NetworkManager>().FromNewComponentOn(this.gameObject).AsSingle().NonLazy();
        }
    }
}
