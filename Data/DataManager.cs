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
    public class DataManager : Singleton<DataManager> // Assuming Singleton<T> is defined elsewhere
{

        private readonly Dictionary<string, AsyncOperationHandle> _handleDic = new(); // Address Key , Handle
        private readonly Dictionary<string, int> _countDic = new(); // Address Key, ref count

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
                } catch (Exception ex) {
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
                } catch (Exception ex) {
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
        /// ��� Addressable ������ ����
        /// �ַ� �� ��ȯ �����̳� ���� ���� �� ȣ��
        /// </summary>
        public void ReleaseAllAssets() {
            var enumerator = _handleDic.GetEnumerator();
            while (enumerator.MoveNext()) { // �ݺ�
                if (enumerator.Current.Value.IsValid()) // ��ȿ�� �ڵ鸸 ���� �õ�
                {
                    Addressables.Release(enumerator.Current.Value);
                }
            }
            // ��� ���� ��� �ʱ�ȭ
            _handleDic.Clear();
            _countDic.Clear();
        }

        #endregion

    }
}
