using GamePlay;
using UnityEngine;
using Zenject;
using Domain;
namespace Core
{

    /// <summary>
    /// Normal 난이도에 맞는 정책을 Bind
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
