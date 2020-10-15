using UnityEditor;

[CustomEditor(typeof(ButtonScaleHover))]
[CanEditMultipleObjects]
public class ButtonScaleHoverEditor : Editor
{
    private SerializedProperty myHoverScaleProperty;
    private SerializedProperty myScaleSpeedProperty;

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        EditorGUILayout.PropertyField(myHoverScaleProperty);
        EditorGUILayout.PropertyField(myScaleSpeedProperty);
        serializedObject.ApplyModifiedProperties();
    }

    private void OnEnable()
    {
        myHoverScaleProperty = serializedObject.FindProperty("myHoverScale");
        myScaleSpeedProperty = serializedObject.FindProperty("myScaleSpeed");
    }
}
