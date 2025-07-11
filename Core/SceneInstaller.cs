using UnityEngine;
using Zenject;
using Data;
using UI;
namespace Core
{
    /// <summary>
    /// Scene�� ���ö� ���ε� �ϴ� ����� ���ϴ� Ŭ����
    /// </summary>
    public class SceneInstaller : MonoInstaller
    {
        public override void InstallBindings() {

            // Tag�� ���� ���ϴ°��� ���ε�
            Container.Bind<IMainCanvasTag>().To<MainCanvasTag>().FromComponentInHierarchy().AsCached(); // ���� ĵ���� ���ε�
          
        }
    }
}
