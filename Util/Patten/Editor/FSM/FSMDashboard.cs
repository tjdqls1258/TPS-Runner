using UnityEditor;
using UnityEngine;

// 아래 네임스페이스는 실제 PlayerStateMachine과 SO들이 있는 경로로 맞춰주세요.
// using Util_Pattern.Parkour; 

public class FSMDashboard : EditorWindow
{
    private Vector2 scrollPos;

    // 상단 메뉴바에 Tools > FSM Dashboard 메뉴를 추가합니다.
    [MenuItem("Tools/FSM Dashboard")]
    public static void ShowWindow()
    {
        // 윈도우 창 띄우기
        var window = GetWindow<FSMDashboard>("FSM Dashboard");
        window.minSize = new Vector2(300, 400);
    }

    private void OnGUI()
    {
        GUILayout.Space(10);
        GUIStyle titleStyle = new GUIStyle(EditorStyles.boldLabel)
        {
            fontSize = 16,
            alignment = TextAnchor.MiddleCenter
        };
        GUILayout.Label("🎮 FSM Control Dashboard", titleStyle);
        EditorGUILayout.Space(20);
        DrawLine();
        EditorGUILayout.Space(10);

        // 하단: 라이브 디버거
        DrawLiveDebugger();
    }


    // 라이브 디버거 (현재 상태 실시간 추적)
    private void DrawLiveDebugger()
    {
        GUILayout.Label("🔴 Live Debugger", EditorStyles.boldLabel);

        // 현재 하이어라키에서 선택된 게임오브젝트 가져오기
        GameObject selectedGO = Selection.activeGameObject;

        if (selectedGO == null)
        {
            EditorGUILayout.HelpBox("하이어라키에서 플레이어(StateMachine)를 선택하세요.", MessageType.Warning);
            return;
        }

        // 선택된 오브젝트에서 StateMachine 컴포넌트 찾기
        var stateMachine = selectedGO.GetComponent<PlayerStateMachine>();

        if (stateMachine == null)
        {
            EditorGUILayout.HelpBox($"선택된 '{selectedGO.name}' 에는 PlayerStateMachine이 없습니다.", MessageType.Warning);
            return;
        }

        // --- 여기서부터는 StateMachine이 선택되었을 때의 UI ---

        scrollPos = EditorGUILayout.BeginScrollView(scrollPos);

        EditorGUILayout.LabelField("Target:", selectedGO.name);

        // 에디터가 플레이 모드일 때만 실시간 상태 표시
        if (EditorApplication.isPlaying)
        {
            GUI.color = Color.green;
            GUILayout.BeginVertical("box");

            // stateMachine.currentState 필드가 public이거나 프로퍼티로 열려있어야 접근 가능합니다.
            string currentStateName = stateMachine.currentState != null ? stateMachine.currentState.name : "None";

            GUIStyle stateStyle = new GUIStyle(EditorStyles.boldLabel) { fontSize = 14 };
            GUILayout.Label($"Current State : {currentStateName}", stateStyle);

            GUILayout.EndVertical();
            GUI.color = Color.white;

            // 현재 상태가 가진 Action들 나열
            if (stateMachine.currentState != null)
            {
                GUILayout.Space(10);
                GUILayout.Label("Running Actions:", EditorStyles.boldLabel);
                foreach (var action in stateMachine.currentState.actions)
                {
                    if (action != null)
                        EditorGUILayout.LabelField(" ◾ " + action.name);
                }
            }
        }
        else
        {
            EditorGUILayout.HelpBox("게임을 실행(Play)하면 현재 상태가 여기에 실시간으로 표시됩니다.", MessageType.Info);
        }

        EditorGUILayout.EndScrollView();

        // 창이 실시간으로 갱신되도록 강제 (게임 플레이 중 프레임마다 창 업데이트)
        Repaint();
    }

    // 에셋 자동 생성 헬퍼 함수
    private void CreateAsset<T>(string defaultName) where T : ScriptableObject
    {
        T asset = ScriptableObject.CreateInstance<T>();

        // 현재 프로젝트 창에서 선택된 폴더 경로 가져오기
        string path = AssetDatabase.GetAssetPath(Selection.activeObject);
        if (string.IsNullOrEmpty(path))
        {
            path = "Assets";
        }
        else if (System.IO.Path.HasExtension(path))
        {
            path = path.Replace(System.IO.Path.GetFileName(AssetDatabase.GetAssetPath(Selection.activeObject)), "");
        }

        string assetPathAndName = AssetDatabase.GenerateUniqueAssetPath(path + "/" + defaultName + ".asset");

        AssetDatabase.CreateAsset(asset, assetPathAndName);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        // 생성된 에셋으로 포커스 이동
        EditorUtility.FocusProjectWindow();
        Selection.activeObject = asset;
    }

    // 시각적 구분을 위한 구분선 그리기
    private void DrawLine()
    {
        Rect rect = EditorGUILayout.GetControlRect(false, 1);
        EditorGUI.DrawRect(rect, new Color(0.5f, 0.5f, 0.5f, 1));
    }
}