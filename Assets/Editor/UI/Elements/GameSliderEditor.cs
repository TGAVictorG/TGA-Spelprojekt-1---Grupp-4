using UnityEditor;
using UnityEditor.UI;

[CustomEditor(typeof(GameSlider))]
public class GameSliderEditor : SliderEditor
{
    private SerializedProperty myFillArea;
    private SerializedProperty myInsetLeft;
    private SerializedProperty myInsetRight;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        serializedObject.Update();

        EditorGUILayout.PropertyField(myFillArea);
        EditorGUILayout.PropertyField(myInsetLeft);
        EditorGUILayout.PropertyField(myInsetRight);

        serializedObject.ApplyModifiedProperties();
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        myFillArea = serializedObject.FindProperty("myFillArea");
        myInsetLeft = serializedObject.FindProperty("myInsetLeft");
        myInsetRight = serializedObject.FindProperty("myInsetRight");
    }
}
