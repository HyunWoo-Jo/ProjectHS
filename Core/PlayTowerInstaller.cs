using GamePlay;
using UnityEngine;
using Zenject;
namespace Core
{
    public class PlayTowerInstaller : MonoInstaller 
    {
        public override void InstallBindings() {
            Container.Bind<IAttackStrategy>().WithId("Projectile").To<ProjectileAttackStrategy>().AsCached();

           
        }
    }
}
