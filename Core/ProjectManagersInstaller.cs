using Data;
using UnityEngine;
using GamePlay;
using Zenject;
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
            Container.Bind<GameManager>().FromNewComponentOnNewPrefab(this.gameObject).AsSingle().NonLazy();
            // DataManager ���ε�
            Container.Bind<DataManager>().FromNewComponentOnNewPrefab(this.gameObject).AsSingle().NonLazy();
        }
    }
}
