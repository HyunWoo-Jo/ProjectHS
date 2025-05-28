using UnityEngine;
using Data;
namespace GamePlay
{
    public interface IUpgradeCommandHandler
    {
        void Execute(UpgradePayload payload);
    }
}
