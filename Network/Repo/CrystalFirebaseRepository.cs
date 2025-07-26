using System;
using UnityEngine;
using Zenject;
using Data;
using Cysharp.Threading.Tasks;
using System.Diagnostics;
using R3;
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

        // Bind 전용으로 Readonly 객체로 반환
        public ReadOnlyReactiveProperty<int> GetRO_ReactiveObservable() {
            return _model.valueObservable;
        }

        public void SetValue(int value) {
            _model.valueObservable.Value = value;
            _userService.SaveUseCrystalAsync(value);
        }

        public async UniTask LoadValue() {
            await _userService.GetUserCrystalAsync((value) => {
                _model.valueObservable.Value = (value);
            });
        }

        public bool TrySpend(int price) {
            SetValue(GetValue() - price);
            return true;
        }

        public bool TryEarn(int value) {
            SetValue(GetValue() + value);
            return true;
        }

        public void Notify() {
            _model.valueObservable.ForceNotify();
        }
    }
}
