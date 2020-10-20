using UnityEngine;

public class FanRotator : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Degrees / second")]
    private float myRotationSpeed = 360.0f;

    private void Update()
    {
        transform.localRotation *= Quaternion.AngleAxis(myRotationSpeed * Time.deltaTime, Vector3.forward);
    }
}
