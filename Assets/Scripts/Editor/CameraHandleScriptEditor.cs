using System.Linq;
using UnityEditor;

[CustomEditor(typeof(CameraHandleScript))]
[CanEditMultipleObjects]
public class CameraHandleScriptEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        var allSame = true;
        ulong? clientId = null;
        foreach (var id in targets.OfType<CameraHandleScript>().Select(s => s.RegisteredKey.clientId))
        {
            if (clientId.HasValue && id != clientId)
            {
                allSame = false;
                break;
            }
            clientId = id;
        }

        EditorGUI.BeginDisabledGroup(true);
        EditorGUI.showMixedValue = !allSame;
        EditorGUILayout.LongField("Client ID", unchecked((long)(clientId ?? 0uL)));
        EditorGUI.showMixedValue = false;
        EditorGUI.EndDisabledGroup();
    }
}
