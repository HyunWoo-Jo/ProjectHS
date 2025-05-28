using Data;
using System.Collections.Generic;
using UnityEngine;

namespace GamePlay
{
    public class UpgradeCommandFactory
    {
        private Dictionary<UpgradeType, IUpgradeCommandHandler> _upgradeDictionary;

        public UpgradeCommandFactory() {
            // handler ���
            _upgradeDictionary = new Dictionary<UpgradeType, IUpgradeCommandHandler> {
                { UpgradeType.Power, new PowerUpHandler() },
            };
        }

        public void Create(UpgradePayload payload) {
            // ���� Create
            var handler = _upgradeDictionary[payload.type];
            var command = new UpgradeCommand(handler, payload).Execute();
        }

    }
}
