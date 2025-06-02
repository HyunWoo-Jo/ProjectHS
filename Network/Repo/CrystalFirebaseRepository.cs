using System;
using UnityEngine;
using Zenject;
using Data;
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

        public void AddChangeHandler(Action<int> handler) {
            _model.valueObservable.OnValueChanged += handler;
        }

        public void RemoveChangeHandler(Action<int> handler) {
            _model.valueObservable.OnValueChanged -= handler;
        }

        public void SetValue(int value) {
            _model.valueObservable.Value = value;
            _userService.SaveUseCrystalAsync(value);
        }

        public void LoadValue() {
            _userService.GetUserCrystalAsync(SetValue);
        }

    }
}
