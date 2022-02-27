using Unity.Netcode.Editor;
using UnityEditor;

[CustomEditor(typeof(PlayerScript))]
public class PlayerScriptEditor : NetworkBehaviourEditor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        var player = target as PlayerScript;
        EditorGUI.BeginDisabledGroup(true);
        EditorGUILayout.TextField("Player Name", player.playerName.Value.ToString());
        EditorGUI.EndDisabledGroup();
    }
}
