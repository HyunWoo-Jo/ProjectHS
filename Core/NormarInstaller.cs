using GamePlay;
using UnityEngine;
using Zenject;
namespace Core
{

    /// <summary>
    /// Normal ���̵��� �´� ��å�� Bind
    /// </summary>
    public class NormarInstaller : MonoInstaller{

        public override void InstallBindings() {
            // Policy
            Container.Bind<IGoldPolicy>().To<GoldPolicy>().AsCached();
            Container.Bind<IHpPolicy>().To<HpPolicy>().AsCached();
            Container.Bind<IExpPolicy>().To<ExpPolicy>().AsCached();
            Container.Bind<ITowerPricePolicy>().To<TowerPricePolicy>().AsCached();
            Container.Bind<IRewardPolicy>().To<RewardPolicy>().AsCached();
        }

    }
}
