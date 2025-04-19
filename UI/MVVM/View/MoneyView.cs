using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Assertions;
namespace UI
{
    public class MoneyView : MonoBehaviour
    {
        private MoneyViewModel _moneyViewModel;
        [SerializeField] private TextMeshPro _text;

        private void Awake() {
#if UNITY_EDITOR
            Assert.IsNotNull(_text);

#endif

            _moneyViewModel = new MoneyViewModel();
            _moneyViewModel.OnDataChanged += UpdateUI;
        }


        public void UpdateUI(long value) {
            _text.text = value.ToString();

        }

    }
}
