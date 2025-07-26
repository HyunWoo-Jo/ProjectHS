using UnityEngine;
using Zenject;
using Network;
using System.Collections;
using Data;
using Cysharp.Threading.Tasks;
using UI;
namespace GamePlay
{
    public class MainLobbySceneManager : MonoBehaviour
    {
        [Inject] private IUIFactory _uiFactory; 
        private void Awake() {
            
        }

    }
}
