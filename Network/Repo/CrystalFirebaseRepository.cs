using System;
using UnityEngine;
using Zenject;
using Data;
using Cysharp.Threading.Tasks;
using System.Diagnostics;
namespace Network
{
    public class CrystalFirebaseRepository : ICrystalRepository {
        private CrystalModel _model;
        [Inject] private IUserNetworkService _userService;
       
        public CrystalFirebaseRepository() {
            _model = new CrystalModel();
        }

        
        public int GetValue() {
            return _model.valueObservable.Value;
        }

        public void AddChangedListener(Action<int> handler) {
            _model.valueObservable.OnValueChanged += handler;
        }

        public void RemoveChangedListener(Action<int> handler) {
            _model.valueObservable.OnValueChanged -= handler;
        }

        public void SetValue(int value) {
            _model.valueObservable.Value = value;
            _userService.SaveUseCrystalAsync(value);
        }

        public async UniTask LoadValue() {
            await _userService.GetUserCrystalAsync(SetValue);
        }

        public bool TrySpend(int price) {
            _model.valueObservable.Value -= price;
            return true;
        }

        public bool TryEarn(int value) {
            _model.valueObservable.Value += value;
            return true;
        }
    }
}
