using CustomUtility;
using R3;
using UnityEngine;

namespace Data
{
    // ���õ� ���׷��̵带 ������ ��
    public class SelectedUpgradeModel {
        public ReactiveProperty<UpgradeDataSO>[] upgradeDatasObservable ={
            new (),
            new (),
            new ()
        }; // 3ĭ ����

        public ReactiveProperty<int> rerollCountObservable = new (0);
    }
}
