using UnityEngine;

public class PlaneController : MonoBehaviour
{
    [Header("For user input")]
    [SerializeField] private float myPitchMultiplier = 1f;
    [SerializeField] private bool myPitchInvertState = false;
    [SerializeField] private float myRollMultiplier = 1f;
    [SerializeField] private bool myRollInvertState = false;
    [Header("How much effect user input has on roll based on rotation difference from 0 (Perfectly level)")]
    [SerializeField] private AnimationCurve myRollEffectByRotationCurve;
    [SerializeField] private float myMaxRollInput = 65;
    [SerializeField] private float myMaxPitch = 75;

    [Header("Speed & drag settings")]
    [SerializeField] private float myStartingVelocity = 3;
    [SerializeField] private float mySpeedMultiplier = 1f;
    [SerializeField] private float myVelocityCap = 5f;
    [SerializeField] private float myDragFactor = 0.2f;

    [SerializeField] private float mySpeedBoostVelocityAdd = 4f;
    [SerializeField] private AnimationCurve mySpeedBoostAccelerationCurve;
    [SerializeField] private AnimationCurve mySpeedBoostFalloffCurve;

    [SerializeField] private AnimationCurve myVelocityByAngleCurve;
    [SerializeField] private AnimationCurve myDragByAngleCurve;

    [Header("and auto turns if banked")]
    [Space(-10)]
    [Header("such as how plane pitches towards the ground at lower speeds")]
    [Space(-10)]
    [Header("Curves and multipliers for plane auto control")]
    [SerializeField] private float myAutoPitchFactor = 1;
    [SerializeField] private float myRollCorrectionFactor = 1;
    [SerializeField] private AnimationCurve myRollCorrectionByVelocityCurve;
    [SerializeField] private AnimationCurve myAutoPitchByVelocityCurve;
    [SerializeField] private AnimationCurve myAutoPitchByRollCurve;
    [SerializeField] private float myVelocityPitchMultiplier;
    [SerializeField] private float myRollPitchMultiplier;

    [SerializeField] private float myNoFuelSteeringFactor = 0.2f;
    [SerializeField] private float myNoFuelDragFactor = 10f;

    private Fuel myFuel;
    private Rigidbody myRigidbody;
    private float myCurrentVelocity;
    private float myCurrentAngleOfAttack;

    private float mySpeedBoostCounter = 0.0f;

    private void Start()
    {
        if (!(myFuel = GetComponent<Fuel>()))
        {
            Debug.LogWarning("No Fuel-component attached!!");
        }
        if(!(myRigidbody = GetComponent<Rigidbody>()))
        {
            Debug.LogWarning("No Rigidbody-component attached!!");
        }

        myCurrentVelocity = myStartingVelocity;
    }

    private void Update()
    {
        if (Input.GetButtonDown("Jump"))
        {
            SpeedBoost();
        }
    }

    private void FixedUpdate()
    {
        AddTorque();
        AddAutoRotationAdjustment();
        CalculateAngleOfAttack();
        SetVelocity();
        ApplyDrag();
    }

    private void AddAutoRotationAdjustment()
    {
        //Turns plane nose towards ground
        Vector3 currentRotation = transform.eulerAngles;

        if ((currentRotation.x > 270 && currentRotation.x < 360) || (currentRotation.x > 0 && currentRotation.x < 90))
        {
            currentRotation.x += myAutoPitchFactor / myCurrentVelocity;
        }
        else
        {
            currentRotation.x -= myAutoPitchFactor / myCurrentVelocity;
        }

        transform.eulerAngles = currentRotation;

        //Auto banking, tries to level plane out
        if (!Input.GetButton("Horizontal"))
        {
            Vector3 targetRotation = currentRotation;
            targetRotation.z = currentRotation.z > 180 ? 360f : 0f;

            float t = myRollCorrectionFactor * myRollCorrectionByVelocityCurve.Evaluate(myCurrentVelocity / myVelocityCap);

            transform.eulerAngles = Vector3.Lerp(currentRotation, targetRotation, t);

        }

        //Auto pitch 
        float currentRoll = transform.eulerAngles.z > 180 ? 360f - transform.eulerAngles.z : transform.eulerAngles.z;

        float pitchValue = myAutoPitchByVelocityCurve.Evaluate(myCurrentVelocity / myVelocityCap) * myVelocityPitchMultiplier;
        pitchValue += myAutoPitchByRollCurve.Evaluate(currentRoll) * (myCurrentVelocity / myVelocityCap) * myRollPitchMultiplier;
        transform.Rotate(transform.right, -pitchValue, Space.World);
    }

    private void SetVelocity()
    {
        myCurrentVelocity += mySpeedMultiplier * myVelocityByAngleCurve.Evaluate(myCurrentAngleOfAttack);

        if (myCurrentVelocity > myVelocityCap)
        {
            myCurrentVelocity = myVelocityCap;
        }

        myRigidbody.velocity = transform.forward * myCurrentVelocity;
    }

    private void ApplyDrag()
    {
        float dragMultiplier = myFuel.myFuelIsEmpty ? myDragFactor * myNoFuelDragFactor : myDragFactor;

        myCurrentVelocity -= myDragByAngleCurve.Evaluate(myCurrentAngleOfAttack) * dragMultiplier * myCurrentVelocity * myCurrentVelocity;
        myCurrentVelocity = Mathf.Clamp(myCurrentVelocity, 0.1f, myVelocityCap);
    }

    private void CalculateAngleOfAttack()
    {
        Vector3 forwardDirection = transform.forward;
        Vector3 relativeWindDirection = forwardDirection;
        relativeWindDirection.y = 0;

        myCurrentAngleOfAttack = Vector3.Dot(Vector3.down, forwardDirection);
        myCurrentAngleOfAttack = 1 - myCurrentAngleOfAttack;
    }

    private void AddTorque()
    {
        AddPitch();
        AddRoll();
    }

    private void AddPitch()
    {
        float pitchValue = Input.GetAxis("Pitch");
        int invertValue = myPitchInvertState ? -1 : 1;
        float pitchFactor = myFuel.myFuelIsEmpty ? myPitchMultiplier * myNoFuelSteeringFactor : myPitchMultiplier;

        transform.Rotate(transform.right, pitchValue * pitchFactor * invertValue, Space.World);
    }

    private void AddRoll()
    {
        float transformZRotation = transform.eulerAngles.z;
        float currentRollDiffFromZero = transformZRotation > 180 ? 360f - transform.eulerAngles.z : transform.eulerAngles.z;
        float rollInput = Input.GetAxis("Roll");

        if (currentRollDiffFromZero > myMaxRollInput)
        {
            bool rollingRight = rollInput > 0 ? true : false;
            if ((rollingRight && transformZRotation > 180) || !rollingRight && transformZRotation < 180)
            {
                return;
            }
        }

        int invertValue = myRollInvertState ? -1 : 1;
        float rollFactor = myFuel.myFuelIsEmpty ? myRollMultiplier * myNoFuelSteeringFactor : myRollMultiplier;
        transform.Rotate(transform.forward, (rollInput * rollFactor * invertValue) * myRollEffectByRotationCurve.Evaluate(currentRollDiffFromZero / 180), Space.World);
    }

    public void SpeedBoost()
    {
        mySpeedBoostCounter = 0f;
        myCurrentVelocity += mySpeedBoostVelocityAdd;
    }
}
