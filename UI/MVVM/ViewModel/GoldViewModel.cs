
using Zenject;
using System;
using Data;
namespace UI
{
    public class GoldViewModel : IInitializable, IDisposable
    {   
        public event Action<int> OnDataChanged; // 데이터가 변경될떄 호출될 액션 (상황에 맞게 변수명을 변경해서 사용)
        [Inject] private GoldModel _goldModel;

        public int Gold => _goldModel.goldObservable.Value;
       
        /// <summary>
        /// 데이터 변경 알림
        /// </summary>
        public void Update() {
            OnDataChanged?.Invoke(Gold);
        }

        public void GoldChanged(int value) {
            OnDataChanged?.Invoke(value);
        }
      

        /// <summary>
        /// Jenject에서 관리
        /// </summary>
        public void Initialize() {
            _goldModel.goldObservable.OnValueChanged += GoldChanged;
        }
        /// <summary>
        /// Jenject에서 관리
        /// </summary>
        public void Dispose() {
            _goldModel.goldObservable.OnValueChanged -= GoldChanged;
        }
    }
} 
