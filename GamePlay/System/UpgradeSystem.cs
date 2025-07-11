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
        [SerializeField] private UpgradeDataSO[] _upgradeDataList; // 업그레이드 데이터 목록 Resources에서 읽어옴
        private int _remainingUpgradeSelections = 0;
        private void Awake() { // Load
            _upgradeDataList = Resources.LoadAll<UpgradeDataSO>("UpgradeData");

           
        }

        /// <summary>
        /// 랜덤한 업그레이드 UI를 생성
        /// </summary>
        public void QueueUpgradeRequest(int level) {
            if (level <= 0) return;
            _remainingUpgradeSelections++;
            if(_remainingUpgradeSelections == 1) {
                ShowRandomUpgradeSelection();
            }
        }
        /// <summary>
        /// 랜덤한 업그레이드 UI를 생성
        /// </summary>
        private void ShowRandomUpgradeSelection() {
            var randUpgleList = GetRandomUpgradeDataList(3);
            for (int i = 0; i < _selectedUpgradeModel.observableUpgradeDatas.Length; i++) {
                // 모델에 업그레이드 갱신
                _selectedUpgradeModel.observableUpgradeDatas[i].Value = randUpgleList.Count > i ? randUpgleList[i] : null;
            }
            _uiFactory.InstanceUI<UpgradeView>(40); // UI 생성
        }

        



        /// <summary>
        /// 중복없이 사용 가능한 업그레이드만 가지고오는데 count 개수보다 적을 수 있음
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        private List<UpgradeDataSO> GetRandomUpgradeDataList(int count) {
            var ableUpgradeList = _upgradeDataList.Where((data) => data.CheckUnlock()).OrderBy(_ => UnityEngine.Random.value).Take(count).ToList(); // 중복없이 사용가능한 것만 가지고옴
            return ableUpgradeList;
        }

        public void Reroll(int index) {
            var list = GetRandomUpgradeDataList(1);
            if (list.Count > 0) {
                _selectedUpgradeModel.observableUpgradeDatas[index].Value = list[0];
            }
            // model 업데이트
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
