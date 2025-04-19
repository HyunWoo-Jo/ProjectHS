using UnityEngine;
using Utility;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine.AddressableAssets;
using Cysharp.Threading.Tasks;
using UnityEngine.ResourceManagement.AsyncOperations;
using System.Linq;
using System.Threading;
using System;
namespace Data
{
    /// <summary>
    /// Addressable 컨트롤 클레스
    /// </summary>
    public class DataManager : MonoBehaviour { 

        private readonly Dictionary<string, AsyncOperationHandle> _handleDic = new(); // Address Key , Handle
        private readonly Dictionary<string, int> _countDic = new(); // Address Key, ref count


        private readonly Dictionary<string, AsyncOperationHandle> _labelHandleDic = new(); // Key: Addressable Label, Value: 리스트 핸들 (AsyncOperationHandle<IList<T>>)
        private readonly Dictionary<string, int> _labelCountDic = new(); // Key: Addressable Label, Value: 레이블 참조 카운트


        #region Addressable Loading

        /// <summary>
        /// Addressable 에셋을 비동기적으로 로드
        /// 이미 로드되었거나 로딩 중인 경우 참조 카운트를 증가.
        /// 로딩 실패 또는 작업 취소 시에는 예외를 발생.
        /// </summary>
        public async UniTask<T> LoadAssetAsync<T>(string key, CancellationToken cancellationToken = default) where T : UnityEngine.Object {
            if (string.IsNullOrEmpty(key)) {
                throw new ArgumentNullException(nameof(key), "Addressable Key는 null이거나 비어 있을 수 없습니다.");
            }

            // 핸들이 존재하는지 확인
            if (_handleDic.TryGetValue(key, out var existingHandle)) {
                _countDic[key]++; // 핸들이 존재하면 참조 카운트 증가

                try {
                    // 기존 핸들의 완료 대기
                    // 실패하면 에러 발생 부분
                    await existingHandle.ToUniTask(cancellationToken: cancellationToken);

                    // await가 성공적으로 완료되면 핸들 상태 확인
                    if (existingHandle.Status == AsyncOperationStatus.Succeeded) {
                        if (existingHandle.Result is T typedResult) {
                            return typedResult;
                        } else {
                            // 로드는 성공했으나 요청한 타입과 다른 경우
                            ReleaseAssetInternal(key); // 실패한 요청에 대한 참조 카운트 감소
                            throw new InvalidCastException($"[DataManager] 에셋 '{key}' 로드는 성공했으나 타입이 다릅니다. 요청 타입: {typeof(T)}, 실제 타입: {existingHandle.Result?.GetType()}.");
                        }
                    } else { // 에러 없이 실패할 경우
                        ReleaseAssetInternal(key); // 실패한 요청에 대한 참조 카운트 감소
                        throw existingHandle.OperationException ?? new Exception($"[DataManager] 기존 핸들 '{key}' 작업 대기 후 예기치 않은 실패. 상태: {existingHandle.Status}");
                    }
                } catch {
                    // await 중 예외 발생 (취소 또는 실패)
                    ReleaseAssetInternal(key); // 참조 카운트 감소
                    throw; // 에러 다시 전달
                }
            } else { // 핸들이 존재하지 않으면 새로 로드 시작
 
                AsyncOperationHandle<T> newHandle = default; // 핸들 변수 선언
                try {
                    // 새로운 로드 작업을 시작합니다.
                    newHandle = Addressables.LoadAssetAsync<T>(key);

                    // 새로 추가
                    _handleDic.Add(key, newHandle);
                    _countDic.Add(key, 1);

                    // 새 핸들의 완료를 대기
                    // 실패하면 에러 발생 부분
                    T result = await newHandle.ToUniTask(cancellationToken: cancellationToken);

                    // await가 성공적으로 완료되면 결과 반환
                    return result;
                } catch {
                    // 로드 중 예외 발생
                    if (newHandle.IsValid() && _handleDic.ContainsKey(key)) { // 핸들이 유효하고 딕셔너리에 아직 있다면

                        ReleaseAssetInternal(key, true); // 강제 제거
                    } else {
                        // 정리
                        _handleDic.Remove(key);
                        _countDic.Remove(key);
                    }
                    throw; // 에러 다시 전달
                }
            }
        }

        public async UniTask<IList<T>> LoadAssetsByLabelAsync<T>(string label, CancellationToken cancellationToken = default) where T : UnityEngine.Object {
            if (string.IsNullOrEmpty(label)) {
                throw new ArgumentNullException(nameof(label), "Addressable Label은 null이거나 비어 있을 수 없습니다.");
            }

            // 레이블 핸들이 존재하는지 확인
            if (_labelHandleDic.TryGetValue(label, out var existingHandle)) {
                _labelCountDic[label]++; // 레이블 핸들이 존재하면 참조 카운트 증가

                try {
                    // 기존 레이블 핸들의 완료 대기
                    await existingHandle.ToUniTask(cancellationToken: cancellationToken);

                    if (existingHandle.Status == AsyncOperationStatus.Succeeded) {
                        // 결과가 IList<T> 타입인지 확인 (중요: LoadAssetsAsync 핸들의 Result는 IList<T>임)
                        if (existingHandle.Result is IList<T> typedResult) {
                            return typedResult;
                        } else {
                            // 로드는 성공했으나 예상치 못한 타입
                            ReleaseAssetsByLabel(label);
                            throw new InvalidCastException($"[DataManager] 레이블 '{label}' 로드는 성공했으나 결과 타입이 IList<{typeof(T)}>가 아닙니다. 실제 타입: {existingHandle.Result?.GetType()}.");
                        }
                    } else {
                        ReleaseAssetsByLabel(label); // 실패 시 이 요청은 실패, 카운트 감소
                        throw existingHandle.OperationException ?? new Exception($"[DataManager] 기존 레이블 핸들 '{label}' 작업 대기 후 예기치 않은 실패. 상태: {existingHandle.Status}");
                    }
                } catch {
                    ReleaseAssetsByLabel(label); // 예외 발생 시 이 요청은 실패, 카운트 감소
                    throw; // 예외 다시 전달
                }
            } else { // 레이블 핸들이 존재하지 않으면 새로 로드 시작     
                AsyncOperationHandle<IList<T>> newHandle = default;
                try {
                    // 중요: LoadAssetsAsync의 두 번째 파라미터는 각 에셋 로드 시 콜백이며, 여기서는 사용하지 않으므로 null 전달
                    newHandle = Addressables.LoadAssetsAsync<T>(label, null);

                    // 레이블 핸들/카운트 추가 (핸들 타입은 AsyncOperationHandle로 저장해도 무방)
                    _labelHandleDic.Add(label, newHandle);
                    _labelCountDic.Add(label, 1);

                    // 새 레이블 핸들의 완료 대기
                    IList<T> result = await newHandle.ToUniTask(cancellationToken: cancellationToken);
                    return result; // 성공 시 결과 리스트 반환
                } catch {
                    // 예외 발생 시 정리
                    if (newHandle.IsValid() && _labelHandleDic.ContainsKey(label)) {
                        ReleaseAssetsByLabel(label, true); // 강제 제거
                    } else {
                        _labelHandleDic.Remove(label);
                        _labelCountDic.Remove(label);
                    }
                    throw; // 예외 다시 전달
                }
            }
        }

        #endregion

        #region Release

        /// <summary>
        /// 지정된 키의 Addressable 에셋 참조 카운트를 감소
        /// 참조 카운트가 0이 되면 실제 에셋 메모리 해제를 요청
        /// </summary>
        public void ReleaseAsset(string key) {
            if (string.IsNullOrEmpty(key)) {
                Debug.LogWarning("[DataManager] ReleaseAsset 호출 시 key가 null이거나 비어 있습니다.");
                return;
            }
            ReleaseAssetInternal(key);
        }

        /// <summary>
        /// 내부 로직: 참조 카운트 감소 및 조건부 핸들 해제
        /// </summary>
        private void ReleaseAssetInternal(string key, bool forceRemove = false) {
            if (!_countDic.TryGetValue(key, out int count)) {
                _handleDic.Remove(key); // 핸들만 남아있을 경우 제거
                return;
            }
            count--;

            if (count <= 0 || forceRemove) { // 참조 카운터 0 또는 강제 제거
                if (_handleDic.TryGetValue(key, out var handle)) {
                    if (handle.IsValid()) { 
                        Addressables.Release(handle); // 헨들 제거 호출
                    }
                    _handleDic.Remove(key); // 핸들 딕셔너리에서 제거
                }
                _countDic.Remove(key); // 카운트 딕셔너리에서 제거
            } else {
                // 참조 카운트가 남아있으면 업데이트
                _countDic[key] = count;
            }
        }
        /// <summary>
        /// 레이블 에셋 참조 카운트 감소 및 조건부 핸들 해제
        /// </summary>
        public void ReleaseAssetsByLabel(string label, bool forceRemove = false) {
            if (!_labelCountDic.TryGetValue(label, out int count)) {
                _labelHandleDic.Remove(label); // 카운트 없으면 핸들도 제거 시도
                return;
            }
            count--;

            if (count <= 0 || forceRemove) {
                if (_labelHandleDic.TryGetValue(label, out var handle)) {
                    if (handle.IsValid()) {
                        // 레이블 핸들 해제 시 해당 레이블로 로드된 모든 에셋이 해제됨
                        Addressables.Release(handle);
                    }
                    _labelHandleDic.Remove(label);
                }
                _labelCountDic.Remove(label);
            } else {
                _labelCountDic[label] = count;
            }
        }
        public void ReleaseAllAssets() {
            // Key 기반 에셋 해제
            var keyEnumerator = _handleDic.GetEnumerator();
            while (keyEnumerator.MoveNext()) {
                if (keyEnumerator.Current.Value.IsValid()) {
                    Addressables.Release(keyEnumerator.Current.Value);
                }
            }
            _handleDic.Clear();
            _countDic.Clear();

            // Label 기반 에셋 해제
            var labelEnumerator = _labelHandleDic.GetEnumerator();
            while (labelEnumerator.MoveNext()) {
                if (labelEnumerator.Current.Value.IsValid()) {
                    Addressables.Release(labelEnumerator.Current.Value);
                }
            }
            _labelHandleDic.Clear();
            _labelCountDic.Clear();

        }

        #endregion

    }
}
