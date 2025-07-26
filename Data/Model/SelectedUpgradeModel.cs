using CustomUtility;
using R3;
using UnityEngine;

namespace Data
{
    // 선택된 업그레이드를 가지는 모델
    public class SelectedUpgradeModel {
        public ReactiveProperty<UpgradeDataSO>[] upgradeDatasObservable ={
            new (),
            new (),
            new ()
        }; // 3칸 생성

        public ReactiveProperty<int> rerollCountObservable = new (0);
    }
}
