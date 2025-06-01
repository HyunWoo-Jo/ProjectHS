using UnityEngine;
using CustomUtility;
namespace Data
{
    public class WaveStatusModel
    {
        public ObservableValue<int> waveLevelObservable { get; } = new(0); // �������� ����
        public ObservableValue<float> waveTimeObservable { get; } = new(0); // ���̺� Ÿ��
    }
}
