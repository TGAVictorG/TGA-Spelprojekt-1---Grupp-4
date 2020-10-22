using UnityEditor;

[CustomEditor(typeof(ButtonScaleHover))]
[CanEditMultipleObjects]
public class ButtonScaleHoverEditor : Editor
{
    private SerializedProperty myHoverScaleProperty;
    private SerializedProperty myScaleSpeedProperty;
    private SerializedProperty myEnableSoundsProperty;

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        EditorGUILayout.PropertyField(myHoverScaleProperty);
        EditorGUILayout.PropertyField(myScaleSpeedProperty);
        EditorGUILayout.PropertyField(myEnableSoundsProperty);
        serializedObject.ApplyModifiedProperties();
    }

    private void OnEnable()
    {
        myHoverScaleProperty = serializedObject.FindProperty("myHoverScale");
        myScaleSpeedProperty = serializedObject.FindProperty("myScaleSpeed");
        myEnableSoundsProperty = serializedObject.FindProperty("myEnableSounds");
    }
}
