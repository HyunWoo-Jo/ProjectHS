using UnityEngine;
using UnityEditor;
using System.IO;
using UnityEditor.VersionControl;
namespace CA.UI {
    /// <summary>
    /// MVP UI �ڵ� ���� �ڵ�
    /// </summary>
    public class VVM_Generator : EditorWindow {

        private string _vvmName;
        private string _viewContext;
        private string _viewModelContect;
        [MenuItem("Tools/Generator/VVM_UI")]
        public static void OpenWindow() {
            var window = GetWindow<VVM_Generator>("VVM_Generator");
            
            window.maxSize = new Vector2(400, 70);
            window.Show();
           

        }
        private void OnGUI() {
            GUILayout.BeginHorizontal();
            GUILayout.Label("name", EditorStyles.boldLabel, GUILayout.Width(40));
            _vvmName = EditorGUILayout.TextField(_vvmName);
            GUILayout.EndHorizontal();
            if (GUILayout.Button("Generate")) {
                SetContext();
                GenerateScript();
            }
        }
        /// <summary>
        /// ���� �ۼ�
        /// </summary>
        private void SetContext() {

            _viewContext = $@"
using UnityEngine;
using Zenject;
using System;
////////////////////////////////////////////////////////////////////////////////////
// Auto Generated Code
namespace UI
{{
    public class {_vvmName}View : MonoBehaviour
    {{
        [Inject] private {_vvmName}ViewModel _viewModel;

        private void Awake() {{
#if UNITY_EDITOR // Assertion
            RefAssert();
#endif
            // ��ư �ʱ�ȭ
            _viewModel.OnDataChanged += UpdateUI;

        }}

        private void OnDestroy() {{
            _viewModel.OnDataChanged -= UpdateUI;
            _viewModel = null; // ���� ����
        }}

#if UNITY_EDITOR
        // ����
        private void RefAssert() {{

        }}
#endif
        // UI ����
        private void UpdateUI() {{
            
        }}
////////////////////////////////////////////////////////////////////////////////////
        // your logic here

    }}
}}
";
            _viewModelContect = $@"
using Zenject;
using System;
namespace UI
{{
    public class {_vvmName}ViewModel 
    {{   
        public event Action OnDataChanged; // �����Ͱ� ����ɋ� ȣ��� �׼� (��Ȳ�� �°� �������� �����ؼ� ���)

        /// <summary>
        /// ������ ���� �˸�
        /// </summary>
        private void NotifyViewDataChanged() {{
            OnDataChanged?.Invoke();
        }}

    }}
}} 
";

        }
        /// <summary>
        /// ����
        /// </summary>
        private void GenerateScript() {
            string path = $"{Application.dataPath}/Scripts/UI/MVVM/";
     
            string viewPath = path + $"View/{_vvmName}View.cs";
            string viewModelPath = path + $"ViewModel/{_vvmName}ViewModel.cs";

            File.WriteAllText(viewPath, _viewContext);
            File.WriteAllText(viewModelPath, _viewModelContect);

            AssetDatabase.Refresh();
            Close();

        }

    }
}
