using CustomUtility;
using UnityEngine;

namespace Data
{
    // 선택된 업그레이드를 가지는 모델
    public class SelectedUpgradeModel {
        public ObservableValue<UpgradeDataSO>[] observableUpgradeDatas ={
            new (),
            new (),
            new ()
        }; // 3칸 생성

        public ObservableValue<int> observableRerollCount = new (0);
    }
}
