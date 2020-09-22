using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] private float myBaseMovementSpeed = 1;


    [SerializeField] private float myPitchMultiplier = 1;
    [SerializeField] private bool myPitchInvertState = false;
    [SerializeField] private float myRollModifier = 1;
    [SerializeField] private bool myRollInvertState = false;
    [SerializeField] private float myYawModifier = 1;
    [SerializeField] private bool myYawInvertState = false;

    private Rigidbody myRigidbody;

    private void Start()
    {
        myRigidbody = GetComponent<Rigidbody>();
        myRigidbody.velocity = transform.forward * myBaseMovementSpeed;
    }

    private void FixedUpdate()
    {
        AddTorque();
        myRigidbody.velocity = transform.forward * myBaseMovementSpeed;
    }

    private void AddTorque()
    {
        AddPitch();
        AddRoll();
        AddYaw();
    }

    private void AddPitch()
    {
        float pitchValue = Input.GetAxis("Pitch");
        int invertValue = myPitchInvertState ? -1 : 1;
        transform.Rotate(transform.right, pitchValue * myPitchMultiplier * invertValue, Space.World);
    }

    private void AddRoll()
    {
        float rollValue = Input.GetAxis("Roll");
        int invertValue = myRollInvertState ? -1 : 1;
        transform.Rotate(transform.forward, rollValue * myRollModifier * invertValue, Space.World);
    }

    private void AddYaw()
    {
        float yawValue = Input.GetAxis("Yaw");
        int invertValue = myYawInvertState ? -1 : 1;
        transform.Rotate(transform.up, yawValue * myYawModifier * invertValue, Space.World);
    }
}
