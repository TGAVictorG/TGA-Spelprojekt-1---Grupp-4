using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

[CustomEditor(typeof(Managers.AudioManager))]
public class AudioManagerEditor : Editor
{
    private SerializedProperty myMusicMixer;
    private SerializedProperty mySFXMixer;
    private SerializedProperty myVoiceMixer;

    private SerializedProperty myAudio;

    private ReorderableList myReorderableList;

    private void OnEnable()
    {
        myMusicMixer = serializedObject.FindProperty("myMusicMixer");
        mySFXMixer = serializedObject.FindProperty("mySFXMixer");
        myVoiceMixer = serializedObject.FindProperty("myVoiceMixer");

        myAudio = serializedObject.FindProperty("myAudio");

        myReorderableList = new ReorderableList(serializedObject, myAudio, true, true, true, true);

        myReorderableList.drawHeaderCallback += aRect =>
        {
            EditorGUI.LabelField(aRect, myAudio.displayName);
        };

        myReorderableList.drawElementCallback += (Rect aRect, int anIndex, bool anIsActive, bool anIsFocused) =>
        {
            SerializedProperty element = myReorderableList.serializedProperty.GetArrayElementAtIndex(anIndex);

            SerializedProperty name = element.FindPropertyRelative("myAudioName");
            SerializedProperty audioClip = element.FindPropertyRelative("myAudioClip");

            CalculatePadded(aRect, 2, 6.0f, out Rect rect, out float step);

            EditorGUI.PropertyField(rect, name);

            rect.y += step;

            EditorGUI.PropertyField(rect, audioClip);
        };

        myReorderableList.elementHeight = EditorGUIUtility.singleLineHeight * 2.0f + 6.0f * 3.0f;
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.PropertyField(myMusicMixer);
        EditorGUILayout.PropertyField(mySFXMixer);
        EditorGUILayout.PropertyField(myVoiceMixer);

        EditorGUILayout.Space(24);

        myReorderableList.DoLayoutList();

        serializedObject.ApplyModifiedProperties();
    }

    private void CalculatePadded(Rect aTotalRect, int anElementCount, float someSpacing, out Rect anElementRect, out float aStepAmount)
    {
        float height = aTotalRect.height - someSpacing;

        float stepSize = height / anElementCount;

        Rect calculatedRect = aTotalRect;

        calculatedRect.y += someSpacing;
        calculatedRect.height = stepSize - someSpacing;

        anElementRect = calculatedRect;
        aStepAmount = stepSize;
    }
}