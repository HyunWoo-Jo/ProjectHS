using UnityEngine;
using CustomUtility;
using R3;
namespace Data
{
    public class WaveStatusModel
    {
        public ReactiveProperty<int> waveLevelObservable { get; } = new(0); // �������� ����
        public ReactiveProperty<float> waveTimeObservable { get; } = new(0); // ���̺� Ÿ��
    }
}
