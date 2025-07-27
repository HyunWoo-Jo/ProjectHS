using Contracts;
using CustomUtility;
using R3;
using UnityEngine;

namespace Domain
{
    // 선택된 업그레이드를 가지는 모델
    public class SelectedUpgradeModel {
        private ReactiveProperty<IUpgradeData>[] _upgradeDatasObservable ={
            new (),
            new (),
            new ()
        }; // 3칸 생성

        private ReactiveProperty<int> _rerollCountObservable = new (0);

        public int RerollCount {
            get { return _rerollCountObservable.Value; }
            private set {  _rerollCountObservable.Value = value;}
        }

        
        public ReadOnlyReactiveProperty<IUpgradeData> GetRO_UpgradeData(int index) => _upgradeDatasObservable[index];

        public int SlotLength => _upgradeDatasObservable.Length;
        
        public ReadOnlyReactiveProperty<int> GetRO_RerollCount => _rerollCountObservable;

        public void SetUpgradeData(int index, IUpgradeData upgradeData) {
            _upgradeDatasObservable[index].Value = upgradeData;
        }


        public IUpgradeData GetUpgradeData(int index) {
            if (index >= _upgradeDatasObservable.Length) return null;
            return _upgradeDatasObservable[index].Value;
        }
        


        public void AddRerollCount(int count) {
            RerollCount += count;
        }

        public void UseRerollCount() {
            RerollCount--;
        }

        public void Notify() {
            foreach (var upgradeData in _upgradeDatasObservable) {
                upgradeData.ForceNotify();
            }
            _rerollCountObservable.ForceNotify();
        }
    }
}
