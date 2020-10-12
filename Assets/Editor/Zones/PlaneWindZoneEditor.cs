using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PlaneWindZone))]
public class PlaneWindZoneEditor : Editor
{
    private void OnSceneGUI()
    {
        PlaneWindZone windZone = target as PlaneWindZone;

        EditorGUI.BeginChangeCheck();

        Vector3 position = windZone.transform.TransformPoint(windZone.myEndPosition);
        Quaternion rotation = Quaternion.LookRotation(windZone.transform.TransformDirection(windZone.myEndRotation));
        Handles.TransformHandle(ref position, ref rotation);

        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(windZone, "Update endpoint");

            windZone.myEndPosition = windZone.transform.InverseTransformPoint(position);
            windZone.myEndRotation = windZone.transform.InverseTransformDirection(rotation * Vector3.forward);
        }
    }
}
