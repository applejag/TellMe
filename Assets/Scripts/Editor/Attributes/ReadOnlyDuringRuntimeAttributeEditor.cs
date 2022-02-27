using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(ReadOnlyDuringRuntimeAttribute))]
public class ReadOnlyDuringRuntimeAttributeEditor : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginDisabledGroup(EditorApplication.isPlaying);
        EditorGUI.PropertyField(position, property, label);
        EditorGUI.EndDisabledGroup();
    }
}
