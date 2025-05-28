using Data;
using UnityEngine;
using Zenject;
namespace GamePlay
{
    public class UpgradeCommand 
    {
        private IUpgradeCommandHandler _handler;
        private UpgradePayload _payload;

        public UpgradeCommand(IUpgradeCommandHandler handler, UpgradePayload payload) {
            _handler = handler;
            _payload = payload;
        }

        public UpgradeCommand Execute() {
            _handler.Execute(_payload);
            return this;
        }
    }
}
