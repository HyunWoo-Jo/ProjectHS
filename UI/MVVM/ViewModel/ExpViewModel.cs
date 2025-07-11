
using Zenject;
using System;
using Data;
namespace UI
{
    public class ExpViewModel : IInitializable , IDisposable {
        [Inject] private ExpModel _model;
        public event Action<int> OnLevelChanged; 
        public event Action<float> OnExpChanged;

        private int Level =>_model.levelObservable.Value;
        public float CurExp => _model.CurExp;
        private float NextExp => _model.nextExpObservable.Value;

        public float GetExpRation() {
            return CurExp / NextExp;
        }
        // View 에서 로딩이 되면 호출 되는 
        public void Update() {
            UpdateLevel(Level);
            UpdateExp(CurExp);
        }

        private void UpdateLevel(int level) {
            OnLevelChanged?.Invoke(level);
        }
        private void UpdateExp(float exp) {
            OnExpChanged?.Invoke(exp);
        }

        // Zenject 에서 관리
        public void Initialize() {
            _model.levelObservable.OnValueChanged += UpdateLevel;
            _model.AddExpChangedListener(UpdateExp);
            _model.nextExpObservable.OnValueChanged += UpdateExp;
        }
        // Zenject 에서 관리
        public void Dispose() {
            _model.levelObservable.OnValueChanged -= UpdateLevel;
            _model.RemoveExpChangedListener(UpdateExp);
            _model.nextExpObservable.OnValueChanged -= UpdateExp;
        }
    }
} 
