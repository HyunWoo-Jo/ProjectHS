using Data;
using Zenject;
using System;
using System.Diagnostics;
using R3;
namespace UI
{
    public class CrystalViewModel 
    {
        [Inject] private ICrystalRepository _repo; // model
        public int Crystal => _repo.GetValue();

        public ReadOnlyReactiveProperty<int> RO_CrystalObservable => _repo.GetRO_ReactiveObservable();

        /// <summary>
        /// ������ ���� �˸�
        /// </summary>
        public void Notify() {
            _repo.Notify();
        }
    }
} 
