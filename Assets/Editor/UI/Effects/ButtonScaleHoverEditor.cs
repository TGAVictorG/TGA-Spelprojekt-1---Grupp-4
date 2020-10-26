using UnityEditor;

[CustomEditor(typeof(ButtonScaleHover))]
[CanEditMultipleObjects]
public class ButtonScaleHoverEditor : Editor
{
    private SerializedProperty myAudioTypeProperty;
    private SerializedProperty myClickAudioNameProperty;

    private SerializedProperty myHoverScaleProperty;
    private SerializedProperty myScaleSpeedProperty;
    private SerializedProperty myEnableSoundsProperty;

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        EditorGUILayout.PropertyField(myAudioTypeProperty);
        EditorGUILayout.PropertyField(myClickAudioNameProperty);

        EditorGUILayout.PropertyField(myHoverScaleProperty);
        EditorGUILayout.PropertyField(myScaleSpeedProperty);
        EditorGUILayout.PropertyField(myEnableSoundsProperty);
        serializedObject.ApplyModifiedProperties();
    }

    private void OnEnable()
    {
        myAudioTypeProperty = serializedObject.FindProperty("myAudioType");
        myClickAudioNameProperty = serializedObject.FindProperty("myClickAudioName");

        myHoverScaleProperty = serializedObject.FindProperty("myHoverScale");
        myScaleSpeedProperty = serializedObject.FindProperty("myScaleSpeed");
        myEnableSoundsProperty = serializedObject.FindProperty("myEnableSounds");
    }
}
