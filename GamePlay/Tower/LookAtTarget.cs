using UnityEngine;
using Data;
using Unity.Mathematics;
namespace GamePlay
{
    // pivot�� Ÿ���� �ٶ󺸵��� �ϴ� Ŭ����
    public class LookAtTarget
    {
        private const float _RotationSpeed = 30f;
        private Transform _pivotTr;

        public LookAtTarget(Transform pivotTr) {
            _pivotTr = pivotTr;
        }

        public void AimLookAtEnemy(float3 targetPosition) {
            float3 dir = math.normalize(targetPosition - (float3)_pivotTr.position);
            dir.z = 0; // z�� ����
            if (math.lengthsq(dir) > 0.0001f) {
                dir = math.normalize(dir);

                quaternion targetRot = quaternion.LookRotationSafe(dir, math.up()); // �⺻ ȸ��
                quaternion newRot = math.slerp(_pivotTr.rotation, targetRot, Time.deltaTime * _RotationSpeed);
                float3 euler = math.degrees(math.EulerXYZ(targetRot));
                euler.y = euler.y > 0 ? 90 : -90; // ���� ����

                _pivotTr.eulerAngles = euler;
            }
        }
    }
}
