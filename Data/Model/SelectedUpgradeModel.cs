using CustomUtility;
using UnityEngine;

namespace Data
{
    // ���õ� ���׷��̵带 ������ ��
    public class SelectedUpgradeModel {
        public ObservableValue<UpgradeDataSO>[] observableUpgradeDatas ={
            new (),
            new (),
            new ()
        }; // 3ĭ ����

        public ObservableValue<int> observableRerollCount = new (0);
    }
}
