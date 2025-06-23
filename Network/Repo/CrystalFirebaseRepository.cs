using System;
using UnityEngine;
using Zenject;
using Data;
using Cysharp.Threading.Tasks;
namespace Network
{
    public class CrystalFirebaseRepository : ICrystalRepository {
        private CrystalModel _model;
        [Inject] private IUserService _userService;
       
        public CrystalFirebaseRepository() {
            _model = new CrystalModel();
        }

        
        public int GetValue() {
            return _model.valueObservable.Value;
        }

        public void AddChangedHandler(Action<int> handler) {
            _model.valueObservable.OnValueChanged += handler;
        }

        public void RemoveChangedHandler(Action<int> handler) {
            _model.valueObservable.OnValueChanged -= handler;
        }

        public void SetValue(int value) {
            _model.valueObservable.Value = value;
            _userService.SaveUseCrystalAsync(value);
        }

        public async UniTask LoadValue() {
            await _userService.GetUserCrystalAsync(SetValue);
        }

    }
}
