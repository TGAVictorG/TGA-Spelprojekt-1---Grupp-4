using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(WindZone))]
public class WindZoneEditor : Editor
{
    private void OnSceneGUI()
    {
        WindZone windZone = target as WindZone;

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
