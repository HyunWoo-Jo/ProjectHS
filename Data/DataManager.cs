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
    /// Addressable ��Ʈ�� Ŭ����
    /// </summary>
    public class DataManager : MonoBehaviour { 

        private readonly Dictionary<string, AsyncOperationHandle> _handleDic = new(); // Address Key , Handle
        private readonly Dictionary<string, int> _countDic = new(); // Address Key, ref count


        private readonly Dictionary<string, AsyncOperationHandle> _labelHandleDic = new(); // Key: Addressable Label, Value: ����Ʈ �ڵ� (AsyncOperationHandle<IList<T>>)
        private readonly Dictionary<string, int> _labelCountDic = new(); // Key: Addressable Label, Value: ���̺� ���� ī��Ʈ


        #region Addressable Loading

        /// <summary>
        /// Addressable ������ �񵿱������� �ε�
        /// �̹� �ε�Ǿ��ų� �ε� ���� ��� ���� ī��Ʈ�� ����.
        /// �ε� ���� �Ǵ� �۾� ��� �ÿ��� ���ܸ� �߻�.
        /// </summary>
        public async UniTask<T> LoadAssetAsync<T>(string key, CancellationToken cancellationToken = default) where T : UnityEngine.Object {
            if (string.IsNullOrEmpty(key)) {
                throw new ArgumentNullException(nameof(key), "Addressable Key�� null�̰ų� ��� ���� �� �����ϴ�.");
            }

            // �ڵ��� �����ϴ��� Ȯ��
            if (_handleDic.TryGetValue(key, out var existingHandle)) {
                _countDic[key]++; // �ڵ��� �����ϸ� ���� ī��Ʈ ����

                try {
                    // ���� �ڵ��� �Ϸ� ���
                    // �����ϸ� ���� �߻� �κ�
                    await existingHandle.ToUniTask(cancellationToken: cancellationToken);

                    // await�� ���������� �Ϸ�Ǹ� �ڵ� ���� Ȯ��
                    if (existingHandle.Status == AsyncOperationStatus.Succeeded) {
                        if (existingHandle.Result is T typedResult) {
                            return typedResult;
                        } else {
                            // �ε�� ���������� ��û�� Ÿ�԰� �ٸ� ���
                            ReleaseAssetInternal(key); // ������ ��û�� ���� ���� ī��Ʈ ����
                            throw new InvalidCastException($"[DataManager] ���� '{key}' �ε�� ���������� Ÿ���� �ٸ��ϴ�. ��û Ÿ��: {typeof(T)}, ���� Ÿ��: {existingHandle.Result?.GetType()}.");
                        }
                    } else { // ���� ���� ������ ���
                        ReleaseAssetInternal(key); // ������ ��û�� ���� ���� ī��Ʈ ����
                        throw existingHandle.OperationException ?? new Exception($"[DataManager] ���� �ڵ� '{key}' �۾� ��� �� ����ġ ���� ����. ����: {existingHandle.Status}");
                    }
                } catch {
                    // await �� ���� �߻� (��� �Ǵ� ����)
                    ReleaseAssetInternal(key); // ���� ī��Ʈ ����
                    throw; // ���� �ٽ� ����
                }
            } else { // �ڵ��� �������� ������ ���� �ε� ����
 
                AsyncOperationHandle<T> newHandle = default; // �ڵ� ���� ����
                try {
                    // ���ο� �ε� �۾��� �����մϴ�.
                    newHandle = Addressables.LoadAssetAsync<T>(key);

                    // ���� �߰�
                    _handleDic.Add(key, newHandle);
                    _countDic.Add(key, 1);

                    // �� �ڵ��� �ϷḦ ���
                    // �����ϸ� ���� �߻� �κ�
                    T result = await newHandle.ToUniTask(cancellationToken: cancellationToken);

                    // await�� ���������� �Ϸ�Ǹ� ��� ��ȯ
                    return result;
                } catch {
                    // �ε� �� ���� �߻�
                    if (newHandle.IsValid() && _handleDic.ContainsKey(key)) { // �ڵ��� ��ȿ�ϰ� ��ųʸ��� ���� �ִٸ�

                        ReleaseAssetInternal(key, true); // ���� ����
                    } else {
                        // ����
                        _handleDic.Remove(key);
                        _countDic.Remove(key);
                    }
                    throw; // ���� �ٽ� ����
                }
            }
        }

        public async UniTask<IList<T>> LoadAssetsByLabelAsync<T>(string label, CancellationToken cancellationToken = default) where T : UnityEngine.Object {
            if (string.IsNullOrEmpty(label)) {
                throw new ArgumentNullException(nameof(label), "Addressable Label�� null�̰ų� ��� ���� �� �����ϴ�.");
            }

            // ���̺� �ڵ��� �����ϴ��� Ȯ��
            if (_labelHandleDic.TryGetValue(label, out var existingHandle)) {
                _labelCountDic[label]++; // ���̺� �ڵ��� �����ϸ� ���� ī��Ʈ ����

                try {
                    // ���� ���̺� �ڵ��� �Ϸ� ���
                    await existingHandle.ToUniTask(cancellationToken: cancellationToken);

                    if (existingHandle.Status == AsyncOperationStatus.Succeeded) {
                        // ����� IList<T> Ÿ������ Ȯ�� (�߿�: LoadAssetsAsync �ڵ��� Result�� IList<T>��)
                        if (existingHandle.Result is IList<T> typedResult) {
                            return typedResult;
                        } else {
                            // �ε�� ���������� ����ġ ���� Ÿ��
                            ReleaseAssetsByLabel(label);
                            throw new InvalidCastException($"[DataManager] ���̺� '{label}' �ε�� ���������� ��� Ÿ���� IList<{typeof(T)}>�� �ƴմϴ�. ���� Ÿ��: {existingHandle.Result?.GetType()}.");
                        }
                    } else {
                        ReleaseAssetsByLabel(label); // ���� �� �� ��û�� ����, ī��Ʈ ����
                        throw existingHandle.OperationException ?? new Exception($"[DataManager] ���� ���̺� �ڵ� '{label}' �۾� ��� �� ����ġ ���� ����. ����: {existingHandle.Status}");
                    }
                } catch {
                    ReleaseAssetsByLabel(label); // ���� �߻� �� �� ��û�� ����, ī��Ʈ ����
                    throw; // ���� �ٽ� ����
                }
            } else { // ���̺� �ڵ��� �������� ������ ���� �ε� ����     
                AsyncOperationHandle<IList<T>> newHandle = default;
                try {
                    // �߿�: LoadAssetsAsync�� �� ��° �Ķ���ʹ� �� ���� �ε� �� �ݹ��̸�, ���⼭�� ������� �����Ƿ� null ����
                    newHandle = Addressables.LoadAssetsAsync<T>(label, null);

                    // ���̺� �ڵ�/ī��Ʈ �߰� (�ڵ� Ÿ���� AsyncOperationHandle�� �����ص� ����)
                    _labelHandleDic.Add(label, newHandle);
                    _labelCountDic.Add(label, 1);

                    // �� ���̺� �ڵ��� �Ϸ� ���
                    IList<T> result = await newHandle.ToUniTask(cancellationToken: cancellationToken);
                    return result; // ���� �� ��� ����Ʈ ��ȯ
                } catch {
                    // ���� �߻� �� ����
                    if (newHandle.IsValid() && _labelHandleDic.ContainsKey(label)) {
                        ReleaseAssetsByLabel(label, true); // ���� ����
                    } else {
                        _labelHandleDic.Remove(label);
                        _labelCountDic.Remove(label);
                    }
                    throw; // ���� �ٽ� ����
                }
            }
        }

        #endregion

        #region Release

        /// <summary>
        /// ������ Ű�� Addressable ���� ���� ī��Ʈ�� ����
        /// ���� ī��Ʈ�� 0�� �Ǹ� ���� ���� �޸� ������ ��û
        /// </summary>
        public void ReleaseAsset(string key) {
            if (string.IsNullOrEmpty(key)) {
                Debug.LogWarning("[DataManager] ReleaseAsset ȣ�� �� key�� null�̰ų� ��� �ֽ��ϴ�.");
                return;
            }
            ReleaseAssetInternal(key);
        }

        /// <summary>
        /// ���� ����: ���� ī��Ʈ ���� �� ���Ǻ� �ڵ� ����
        /// </summary>
        private void ReleaseAssetInternal(string key, bool forceRemove = false) {
            if (!_countDic.TryGetValue(key, out int count)) {
                _handleDic.Remove(key); // �ڵ鸸 �������� ��� ����
                return;
            }
            count--;

            if (count <= 0 || forceRemove) { // ���� ī���� 0 �Ǵ� ���� ����
                if (_handleDic.TryGetValue(key, out var handle)) {
                    if (handle.IsValid()) { 
                        Addressables.Release(handle); // ��� ���� ȣ��
                    }
                    _handleDic.Remove(key); // �ڵ� ��ųʸ����� ����
                }
                _countDic.Remove(key); // ī��Ʈ ��ųʸ����� ����
            } else {
                // ���� ī��Ʈ�� ���������� ������Ʈ
                _countDic[key] = count;
            }
        }
        /// <summary>
        /// ���̺� ���� ���� ī��Ʈ ���� �� ���Ǻ� �ڵ� ����
        /// </summary>
        public void ReleaseAssetsByLabel(string label, bool forceRemove = false) {
            if (!_labelCountDic.TryGetValue(label, out int count)) {
                _labelHandleDic.Remove(label); // ī��Ʈ ������ �ڵ鵵 ���� �õ�
                return;
            }
            count--;

            if (count <= 0 || forceRemove) {
                if (_labelHandleDic.TryGetValue(label, out var handle)) {
                    if (handle.IsValid()) {
                        // ���̺� �ڵ� ���� �� �ش� ���̺�� �ε�� ��� ������ ������
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
            // Key ��� ���� ����
            var keyEnumerator = _handleDic.GetEnumerator();
            while (keyEnumerator.MoveNext()) {
                if (keyEnumerator.Current.Value.IsValid()) {
                    Addressables.Release(keyEnumerator.Current.Value);
                }
            }
            _handleDic.Clear();
            _countDic.Clear();

            // Label ��� ���� ����
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
