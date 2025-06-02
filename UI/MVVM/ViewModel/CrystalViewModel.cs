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

        // zenject���� ����

        public void Initialize() {
            _repo.AddChangeHandler(NotifyViewDataChanged);
 
        }

        // zenject���� ����
        public void Dispose() {
            _repo.RemoveChangeHandler(NotifyViewDataChanged);
        }
    }
} 
