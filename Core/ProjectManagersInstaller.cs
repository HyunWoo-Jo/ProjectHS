using Data;
using UnityEngine;
using GamePlay;
using Zenject;
using UI;
namespace Core
{
    /// <summary>
    /// Project�� �������� �������ִ� Ŭ����
    /// </summary>
    public class ProjectManagersInstaller : MonoInstaller
    {
        public override void InstallBindings() {
            // ������ ���ε� Manager�� ���ε���
            // GameManager ���ε�
            Container.Bind<GameManager>().FromNewComponentOn(this.gameObject).AsSingle().NonLazy();
            // DataManager ���ε�
            Container.Bind<DataManager>().FromNewComponentOn(this.gameObject).AsSingle().NonLazy();
            // UIManager ���ε�
            Container.BindInterfacesAndSelfTo<UIManager>().FromNewComponentOn(this.gameObject).AsSingle().NonLazy();
            // LoadManager ���ε�
            Container.BindInterfacesAndSelfTo<LoadManager>().FromNewComponentOn(this.gameObject).AsSingle().NonLazy();
        }
    }
}
