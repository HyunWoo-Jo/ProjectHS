using UnityEngine;
using CustomUtility;
using R3;
namespace Data
{
    public class WaveStatusModel
    {
        public ReactiveProperty<int> waveLevelObservable { get; } = new(0); // 스테이지 레벨
        public ReactiveProperty<float> waveTimeObservable { get; } = new(0); // 웨이브 타임
    }
}
