using System.Collections.Generic;
using UnityEngine;

namespace Data
{
    public class UpgradePayload {
        public UpgradeType type;
        public float? value = null;    // ��� ��ġ
        public string abilityId = null;    // Ư�� �ɷ� �ĺ�
        public IReadOnlyDictionary<string, object> args = null; // ���� �Ķ����
    }
}
