using Contracts;
using Domain;
using Zenject;
namespace GamePlay
{
    public class SellTowerService : ISellTowerService {
        [Inject] private ITowerSystem _towerSystem;
        [Inject] private GoldModel _goldModel;
        public bool TrySellTower() {
            // 타워 삭제에 성공하면 골드 추가
            if (!_towerSystem.TryRemoveTower(out int cost)) return false;
            _goldModel.TryEarnGold(cost);
            return true;
        }
    }
}
