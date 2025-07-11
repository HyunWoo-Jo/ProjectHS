using UnityEngine;
using Data;
using Zenject;
using System.Collections.Generic;
using System.Linq;
using UI;
using System;
using Contracts;
namespace GamePlay
{
    public class UpgradeSystem : MonoBehaviour, IUpgradeService {
        [Inject] private IUIFactory _uiFactory;
        [Inject] private GameDataHub _gameDataHub;
        [Inject] private IGlobalUpgradeRepository _globalUpgradeRepository;
        [Inject] private SelectedUpgradeModel _selectedUpgradeModel;
        [SerializeField] private UpgradeDataSO[] _upgradeDataList; // ���׷��̵� ������ ��� Resources���� �о��
        private int _remainingUpgradeSelections = 0;
        private void Awake() { // Load
            _upgradeDataList = Resources.LoadAll<UpgradeDataSO>("UpgradeData");

           
        }

        /// <summary>
        /// ������ ���׷��̵� UI�� ����
        /// </summary>
        public void QueueUpgradeRequest(int level) {
            if (level <= 0) return;
            _remainingUpgradeSelections++;
            if(_remainingUpgradeSelections == 1) {
                ShowRandomUpgradeSelection();
            }
        }
        /// <summary>
        /// ������ ���׷��̵� UI�� ����
        /// </summary>
        private void ShowRandomUpgradeSelection() {
            var randUpgleList = GetRandomUpgradeDataList(3);
            for (int i = 0; i < _selectedUpgradeModel.observableUpgradeDatas.Length; i++) {
                // �𵨿� ���׷��̵� ����
                _selectedUpgradeModel.observableUpgradeDatas[i].Value = randUpgleList.Count > i ? randUpgleList[i] : null;
            }
            _uiFactory.InstanceUI<UpgradeView>(40); // UI ����
        }

        



        /// <summary>
        /// �ߺ����� ��� ������ ���׷��̵常 ��������µ� count �������� ���� �� ����
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        private List<UpgradeDataSO> GetRandomUpgradeDataList(int count) {
            var ableUpgradeList = _upgradeDataList.Where((data) => data.CheckUnlock()).OrderBy(_ => UnityEngine.Random.value).Take(count).ToList(); // �ߺ����� ��밡���� �͸� �������
            return ableUpgradeList;
        }

        public void Reroll(int index) {
            var list = GetRandomUpgradeDataList(1);
            if (list.Count > 0) {
                _selectedUpgradeModel.observableUpgradeDatas[index].Value = list[0];
            }
            // model ������Ʈ
            _selectedUpgradeModel.observableRerollCount.Value -= 1;
        }

        public void ApplyUpgrade(int index) {
            _selectedUpgradeModel.observableUpgradeDatas[index].Value.ApplyUpgrade();
            _remainingUpgradeSelections--;
            if(_remainingUpgradeSelections > 0) {
                ShowRandomUpgradeSelection();
            }
        }
    }
}
