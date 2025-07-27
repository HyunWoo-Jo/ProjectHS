using System;
using UnityEngine;
using Zenject;
using Cysharp.Threading.Tasks;
using System.Diagnostics;
using R3;
using Network;
using Contracts;
namespace Infrastructure {
    public class CrystalFirebaseRepository : ICrystalRepository {
        [Inject] private IUserNetworkService _userService;

        public async UniTask<int> GetAsyncValue() {
            int current = await _userService.GetUserCrystalAsync();
            return current;
        }

        public async UniTask<bool> AsyncTrySpend(int price) {
            try {
                int current = await _userService.GetUserCrystalAsync();

                if (current - price >= 0) {
                    await _userService.SaveUseCrystalAsync(current - price);
                    return true;
                }

                return false;
            } catch {
                return false;
            }
        }

        public async UniTask<bool> AsyncTryEarn(int value) {
            try {
                int current = await _userService.GetUserCrystalAsync();
                int newValue = current + value;

                await _userService.SaveUseCrystalAsync(newValue);
                return true;
            } catch {
                return false;
            }
        }
    }
}
