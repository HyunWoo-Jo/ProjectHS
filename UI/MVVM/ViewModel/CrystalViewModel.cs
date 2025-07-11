using Data;
using Zenject;
using System;
using System.Diagnostics;
namespace UI
{
    public class CrystalViewModel : IInitializable, IDisposable
    {
        [Inject] private ICrystalRepository _repo; // model
        public event Action<int> OnDataChanged; // �����Ͱ� ����ɋ� ȣ��� �׼�
        public int Crystal => _repo.GetValue();

        /// <summary>
        /// ������ ���� �˸�
        /// </summary>
        private void NotifyViewDataChanged(int value) {
            OnDataChanged?.Invoke(value);
        }

        public void Update() {
            NotifyViewDataChanged(Crystal);
        }

        // zenject���� ����

        public void Initialize() {
            _repo.AddChangedListener(NotifyViewDataChanged);
        }

        // zenject���� ����
        public void Dispose() {
            _repo.RemoveChangedListener(NotifyViewDataChanged);
        }
    }
} 
