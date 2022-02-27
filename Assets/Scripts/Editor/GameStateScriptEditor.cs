using System;
using System.Linq;
using Unity.Netcode.Editor;
using UnityEditor;
using UnityEditorInternal;

[CustomEditor(typeof(GameStateScript))]
public class GameStateScriptEditor : NetworkBehaviourEditor
{
    private ReorderableList list;

    private void OnEnable()
    {
        list = new(Array.Empty<PlayerScript>(), typeof(PlayerScript),
            draggable: false,
            displayHeader: true,
            displayAddButton: false,
            displayRemoveButton: false);
        list.drawHeaderCallback = (rect) =>
        {
            EditorGUI.LabelField(rect, "Player Scripts");
        };
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        var script = (GameStateScript)target;

        EditorGUI.BeginDisabledGroup(true);
        list.list = script.PlayerScripts?.ToArray() ?? Array.Empty<PlayerScript>();
        list.DoLayoutList();
        EditorGUI.EndDisabledGroup();
    }
}
