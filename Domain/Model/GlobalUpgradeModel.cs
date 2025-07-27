using System.Collections.Generic;

using R3;
using Contracts;
using Zenject;
using Cysharp.Threading.Tasks;
using System.Diagnostics;
namespace Domain
{
    public class GlobalUpgradeModel 
    {
        [Inject] private IGlobalUpgradeRepository _repo;
        private Dictionary<string, ReactiveProperty<int>> _dataDictionary = new(); // 업그레이드 단계를 저장

        public ReadOnlyReactiveProperty<int> GetRO_UpgradeData(GlobalUpgradeType key) => _dataDictionary.ContainsKey(key.ToString()) ? _dataDictionary[key.ToString()] : (_dataDictionary[key.ToString()] = new ReactiveProperty<int>(0));



        public async UniTask AsyncLoadData() {
            await _repo.LoadTableAsync();
            var dic =  await _repo.LoadAllUpgradeLevelAsync();

            // Dictionary 생성
            foreach (var item in dic) {
                if (!_dataDictionary.TryAdd(item.Key, new ReactiveProperty<int>(item.Value))) {
                    _dataDictionary[item.Key].Value = item.Value;
                }
            }
        }

        public int GetLevel(GlobalUpgradeType key) {
            return _dataDictionary[key.ToString()].Value;
        }

        public UniTask<int> GetLevelAsync(GlobalUpgradeType key) {
            return _repo.GetLevelAsync(key);
        }

        public int GetPrice(GlobalUpgradeType key) {
            return _repo.GetPrice(key, GetLevel(key));
        }
        public int GetPrice(GlobalUpgradeType key, int level) {
            return _repo.GetPrice(key, level);
        }
        public int GetAbilityPower(GlobalUpgradeType key) {
            return _repo.GetAbilityValue(key, GetLevel(key));
        }

        public void SetLevel(GlobalUpgradeType key, int value) {
            _repo.SetLevel(key, value);
            _dataDictionary[key.ToString()].Value = value;
        }

    }
}
