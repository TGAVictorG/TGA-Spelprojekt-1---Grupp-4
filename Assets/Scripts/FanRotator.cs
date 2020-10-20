using UnityEngine;

public class FanRotator : MonoBehaviour
{
    public enum Axis
    {
        X,
        Y,
        Z
    }

    [SerializeField]
    [Tooltip("Degrees / second")]
    private float myRotationSpeed = 360.0f;

    [SerializeField]
    private Axis myRotationAxis = Axis.Z;

    private Vector3 myRotationVector = Vector3.forward;

    private void UpdateRotationVector()
    {
        switch(myRotationAxis)
        {
            case Axis.X:
                myRotationVector = Vector3.right;
                break;
            case Axis.Y:
                myRotationVector = Vector3.up;
                break;
            case Axis.Z:
            default:
                myRotationVector = Vector3.forward;
                break;
        }
    }

    private void Update()
    {
        transform.localRotation *= Quaternion.AngleAxis(myRotationSpeed * Time.deltaTime, myRotationVector);
    }

    private void Awake()
    {
        UpdateRotationVector();
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        UpdateRotationVector();
    }
#endif
}
