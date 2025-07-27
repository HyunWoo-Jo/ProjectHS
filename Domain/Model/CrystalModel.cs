using UnityEngine;
using CustomUtility;
using R3;
using Zenject;
using Contracts;
using Cysharp.Threading.Tasks;
namespace Domain
{
    public class CrystalModel
    {
        [Inject] private ICrystalRepository _repo;

        private ReactiveProperty<int> _crystalObservable = new();

        public ReadOnlyReactiveProperty<int> RO_CrystalObservable => _crystalObservable;


        public void SetValue(int value) {
            _crystalObservable.Value = value;
        }

        public async UniTask<bool> TryEarn(int value) {
            bool sus = await _repo.AsyncTryEarn(value);
            if (sus) {
                await LoadData();
            }
            return sus;
        }
        public async UniTask<bool> TrySpend(int value) {
            bool sus = await _repo.AsyncTrySpend(value);
            if (sus) {
               await LoadData();
            }
            return sus;
        }

        public async UniTask<int> LoadData() {
            int result = await _repo.GetAsyncValue();
            _crystalObservable.Value = result;

            return result;
        }


        public void Notify() => _crystalObservable.ForceNotify();
    }
}
