using System.Collections;
using UnityEngine;

public class PlaneController : MonoBehaviour
{
    [Header("(Skulle rekommenera att säkna myPitchMultiplier ifall båda är igång)")]
    [Space(-10)]
    [Header("(Man kan ha båda systemen samtidigt, nya petar nosen mot taket, gamla pitchar baserat på rotation)")]
    [Header("DEBUG")]
    [SerializeField] private bool myUseOldPitchSystem = false;
    [SerializeField] private bool myUseNewPitchSystem = false;
    [SerializeField] private bool myEnableSpaceSpeedBoost;
    [SerializeField] private bool myEnableUnlimitedFuel;

    [Header("USER INPUT")]
    [Header("--------------------------------------------------------------------------------------------------------")]
    //[Space()]
    [SerializeField] private float myPitchMultiplier = 1f;
    [SerializeField] private bool myPitchInvertState = false;
    [SerializeField] private float myRollMultiplier = 1f;
    [SerializeField] [Tooltip("How hard the plane turns when rotating with A/D")] private float myRollTurnMultiplier = 0.01f;
    [SerializeField] private bool myRollInvertState = false;
    [SerializeField] private float myMaxRoll = 65;
    [SerializeField] private float myMaxPitch = 75;

    [Header("SPEED & DRAG SETTINGS")]
    [SerializeField] private float myStartingVelocity = 3;
    [SerializeField] private float mySpeedMultiplier = 1f;
    [SerializeField] private float myVelocityCap = 5f;
    [SerializeField] private float myDragFactor = 0.2f;
    [SerializeField] private float myMinimunVelocity = 1f;

    [SerializeField] private float mySpeedBoostVelocityAdd = 4f;
    [SerializeField] private AnimationCurve mySpeedBoostAccelerationCurve;
    [SerializeField] private AnimationCurve mySpeedBoostFalloffCurve;

    [SerializeField] private AnimationCurve myVelocityByAngleCurve;
    [SerializeField] private AnimationCurve myDragByAngleCurve;

    [Header("and auto turns if banked")]
    [Space(-10)]
    [Header("such as how plane pitches towards the ground at lower speeds")]
    [Space(-10)]
    [Header("AUTO CORRECTION")]
    [SerializeField] [Tooltip("How much the plane automatically pitches towards the ground")] private float myAutoPitchFactor = 1;
    [SerializeField] [Tooltip("How hard the plane corrects the roll of the plane")] private float myRollCorrectionFactor = 1;
    [SerializeField] private AnimationCurve myRollCorrectionByVelocityCurve;
    [SerializeField] private AnimationCurve myAutoPitchByVelocityCurve;
    [SerializeField] private AnimationCurve myAutoPitchByRollCurve;
    [SerializeField] private float myVelocityPitchMultiplier;
    [SerializeField] private float myRollPitchMultiplier;
    [SerializeField] private float myNoFuelWeightIncrease;
    [SerializeField] private bool myPerfectColitionBox;


    [SerializeField] private float myNoFuelSteeringFactor = 0.2f;
    [SerializeField] private float myNoFuelDragFactor = 10f;
    [SerializeField] private float myNoFuelFallFactor = 1f;

    private Fuel myFuel;
    private Rigidbody myRigidbody;
    private SpeedLineController mySpeedLineController;
    private SpeedBoost mySpeedBoost;
    private float myCurrentVelocity;
    private float myCurrentAngleOfAttack;
    private float myNoFuelTime = 0.0f;

    private float mySpeedBoostCounter = 0.0f;

    private void Start()
    {
        if (!(myFuel = GetComponent<Fuel>()))
        {
            Debug.LogWarning("No Fuel-component attached!");
        }
        if (!(myRigidbody = GetComponent<Rigidbody>()))
        {
            Debug.LogWarning("No Rigidbody-component attached!");
        }
        if (!(mySpeedLineController = GetComponent<SpeedLineController>()))
        {
            Debug.LogWarning("No SpeedLineController-component attached!");
        }
        if (!(mySpeedBoost = GetComponent<SpeedBoost>()))
        {
            Debug.LogWarning("No SpeedBoost-component attached!");
        }
        else
        {
            mySpeedBoost.myOnSpeedBoost += SpeedBoost;
        }

        if (myPerfectColitionBox)
        {
            transform.localScale = new Vector3(1f, 1f, 1f);
            transform.GetChild(0).localScale = new Vector3(1f, 1f, 1f);
        }
        else
        {
            transform.localScale = new Vector3(0.65f, 1f, 1f);
            transform.GetChild(0).localScale = new Vector3(1.4774f, 1.4774f, 1.4774f);

        }

        myCurrentVelocity = myStartingVelocity;
    }

    private void Update()
    {
        if (myEnableSpaceSpeedBoost && Input.GetButtonDown("Jump"))
        {
            SpeedBoost(mySpeedBoostVelocityAdd);
        }

        myFuel.myAllowFuelDepletion = !myEnableUnlimitedFuel;
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
        else if (myCurrentVelocity < myMinimunVelocity && myFuel.myFuelIsEmpty == false)
        {
            myCurrentVelocity = myMinimunVelocity;
        }

        myRigidbody.velocity = transform.forward * myCurrentVelocity;
    }

    private void ApplyDrag()
    {
        float dragMultiplier = myFuel.myFuelIsEmpty ? myDragFactor * myNoFuelDragFactor : myDragFactor;

        if (myFuel.myFuelIsEmpty)
        {
            myNoFuelTime += Time.deltaTime;
            AddPitch(false, myNoFuelWeightIncrease * myNoFuelTime * myNoFuelFallFactor);
        }
        else
        {
            myNoFuelTime = 0f;
        }

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

        float currentRoll = transform.eulerAngles.z > 180 ? transform.eulerAngles.z - 360f : transform.eulerAngles.z;
        transform.Rotate(Vector3.up, -currentRoll * myRollTurnMultiplier, Space.World);
    }

    private void AddPitch(bool anInputFromKeyboard = true, float aPitchInput = 0)
    {
        float transformXRotation = transform.eulerAngles.x;
        float currentAbsolutePitch = transform.eulerAngles.x > 180 ? 360 - transformXRotation : transformXRotation;

        float pitchInput = aPitchInput;
        if (anInputFromKeyboard)
        {
            pitchInput = Input.GetAxis("Pitch") * (myPitchInvertState ? -1 : 1);
        }


        if (currentAbsolutePitch > myMaxPitch)
        {
            bool pitchingUp = pitchInput < 0 ? true : false;

            if ((pitchingUp && transformXRotation > 180) || !pitchingUp && transformXRotation < 180)
            {
                return;
            }
        }

        if (myUseOldPitchSystem)
        {
            int invertValue = myPitchInvertState ? -1 : 1;
            float pitchFactor = myFuel.myFuelIsEmpty ? myPitchMultiplier * myNoFuelSteeringFactor : myPitchMultiplier;

            transform.Rotate(transform.right, pitchInput * pitchFactor * invertValue, Space.World);
        }
        if (myUseNewPitchSystem)
        {
            Vector3 currentRotation = transform.eulerAngles;

            if ((currentRotation.x > 270 && currentRotation.x < 360) || (currentRotation.x > 0 && currentRotation.x < 90))
            {
                currentRotation.x += pitchInput * myPitchMultiplier;
            }
            else
            {
                currentRotation.x -= pitchInput * myPitchMultiplier;
            }

            transform.eulerAngles = currentRotation;
        }
    }

    private void AddRoll()
    {
        float transformZRotation = transform.eulerAngles.z;
        float currentAbsoluteRoll = transformZRotation > 180 ? 360f - transformZRotation : transformZRotation;
        float rollInput = Input.GetAxis("Roll");


        if (currentAbsoluteRoll > myMaxRoll)
        {
            bool rollingRight = rollInput > 0 ? true : false;
            if ((rollingRight && transformZRotation > 180) || !rollingRight && transformZRotation < 180)
            {
                return;
            }
        }

        int invertValue = myRollInvertState ? -1 : 1;
        float rollFactor = myFuel.myFuelIsEmpty ? myRollMultiplier * myNoFuelSteeringFactor : myRollMultiplier;
        transform.Rotate(transform.forward, (rollInput * rollFactor * invertValue), Space.World);
    }

    public void SpeedBoost(float aBoostAmount)
    {
        mySpeedBoostCounter = 0f;
        myCurrentVelocity += aBoostAmount;

        mySpeedLineController.Activate();
    }
}
