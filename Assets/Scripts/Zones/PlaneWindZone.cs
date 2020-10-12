using System.Collections;
using UnityEngine;

[RequireComponent(typeof(CapsuleCollider))]
public class PlaneWindZone : MonoBehaviour
{
    [Header("Non steering parameters")]
    [Tooltip("In degrees / second (used when myUseSteering == false)")]
    public float myRotationSpeed = 135.0f;
    public float myVelocityChangeSpeed = 20.0f;

    [Header("Steering parameters")]
    [Tooltip("Will use steering to get to the end position, but will not take myEndRotation into account")]
    public bool myUseSteering = false;
    [Tooltip("In degrees / second")]
    public float myVelocityRotationSpeed = 135.0f;
    public float myPlayerMaxTipAngle = 35.0f;

    [Header("Generic parameters")]
    public float myPlayerSpeed = 7.0f;

    public float myEndZoneSize = 1.0f;

    public AnimationCurve mySpeedMultiplierOverTime = new AnimationCurve(
        new Keyframe(0.0f, 0.3f),
        new Keyframe(0.25f, 1.0f),
        new Keyframe(3.0f, 0.3f)
        );

    public Vector3 myEndPosition = Vector3.zero;
    public Vector3 myEndRotation = Vector3.forward;

    private Collider myCollider;

    private IEnumerator DoWindTraversal(Transform aPlayerTransform)
    {
        PlaneController planeController = aPlayerTransform.GetComponent<PlaneController>();
        Rigidbody playerRigidbody = aPlayerTransform.GetComponent<Rigidbody>();
        planeController.enabled = false;

        Vector3 playerVelocity = playerRigidbody.velocity;
        float playerStartSpeed = Mathf.Max(playerVelocity.magnitude, 2.5f);

        CollisionDetectionMode savedCollisionDetectionMode = playerRigidbody.collisionDetectionMode;
        if (savedCollisionDetectionMode != CollisionDetectionMode.Discrete)
            playerRigidbody.collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;
        playerRigidbody.isKinematic = true;

        Vector3 endPosition = transform.TransformPoint(myEndPosition);
        Vector3 endRotation = transform.TransformDirection(myEndRotation);

        float windStartTime = Time.time;
        float targetSpeed = myPlayerSpeed > playerStartSpeed ? myPlayerSpeed : playerStartSpeed * 2.0f;

        do
        {
            Vector3 playerToTarget = endPosition - playerRigidbody.position;
            if (playerToTarget.sqrMagnitude <= myEndZoneSize * myEndZoneSize)
            {
                break;
            }

            if (myUseSteering)
            {
                float playerSpeed = mySpeedMultiplierOverTime.Evaluate(Time.time - windStartTime) * (targetSpeed - playerStartSpeed) + playerStartSpeed;

                playerVelocity = Vector3.RotateTowards(playerVelocity, playerToTarget, myVelocityRotationSpeed * Mathf.Deg2Rad * Time.deltaTime, 0.0f);

                Vector3 leftAxis = Vector3.Cross(playerVelocity, Vector3.up);
                Vector3 xzVelocity = new Vector3(playerVelocity.x, 0.0f, playerVelocity.z).normalized;

                float tipAngle = Vector3.SignedAngle(xzVelocity, playerVelocity, leftAxis);
                if (Mathf.Abs(tipAngle) > myPlayerMaxTipAngle)
                {
                    tipAngle = myPlayerMaxTipAngle * Mathf.Sign(tipAngle);
                }

                playerVelocity = Quaternion.AngleAxis(tipAngle, leftAxis) * xzVelocity;

#if UNITY_EDITOR
                Debug.DrawLine(playerRigidbody.position, playerRigidbody.position + playerVelocity, Color.red);
                Debug.DrawLine(playerRigidbody.position, playerRigidbody.position + leftAxis, Color.red);
#endif

                playerRigidbody.rotation = Quaternion.LookRotation(playerVelocity);
                playerRigidbody.position += playerVelocity * playerSpeed * Time.deltaTime;
            }
            else
            {
                playerVelocity = Vector3.MoveTowards(playerVelocity, playerToTarget.normalized * myPlayerSpeed, myVelocityChangeSpeed * mySpeedMultiplierOverTime.Evaluate(Time.time - windStartTime) * Time.deltaTime);

#if UNITY_EDITOR
                Debug.DrawLine(playerRigidbody.position, playerRigidbody.position + playerVelocity, Color.red);
#endif

                playerRigidbody.rotation = Quaternion.RotateTowards(playerRigidbody.rotation, Quaternion.LookRotation(endRotation), myRotationSpeed * Time.deltaTime);
                playerRigidbody.position += playerVelocity * Time.deltaTime;
            }

            yield return null;
        } while (true);

        playerRigidbody.isKinematic = false;
        playerRigidbody.collisionDetectionMode = savedCollisionDetectionMode;
        playerRigidbody.velocity = playerVelocity;

        planeController.enabled = true;
    }

    private void OnTriggerEnter(Collider anOther)
    {
        if (anOther.CompareTag("Player"))
        {
            myCollider.enabled = false;

            StartCoroutine(DoWindTraversal(anOther.transform));
        }
    }

    private void Start()
    {
        myCollider = GetComponent<Collider>();
    }

#if UNITY_EDITOR

    private void OnDrawGizmos()
    {
        Color color = Color.yellow;
        color.a = 0.5f;

        Gizmos.color = color;

        Gizmos.DrawCube(transform.position, transform.lossyScale);

        Gizmos.DrawLine(transform.position, transform.TransformPoint(myEndPosition));
        Gizmos.DrawWireSphere(transform.TransformPoint(myEndPosition), myEndZoneSize);
    }

#endif
}
