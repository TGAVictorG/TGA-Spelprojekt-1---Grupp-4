using System.Collections;
using UnityEngine;

[RequireComponent(typeof(CapsuleCollider))]
public class WindZone : MonoBehaviour
{
    [Tooltip("In degrees / second")]
    public float myVelocityRotationSpeed = 135.0f;
    [Tooltip("In degrees / second (used when myUseSteering == false)")]
    public float myRotationSpeed = 135.0f;
    public float myPlayerSpeed = 7.0f;

    public float myEndZoneSize = 1.0f;

    public float myPlayerMaxTipAngle = 35.0f;

    public AnimationCurve mySpeedMultiplierOverTime = AnimationCurve.EaseInOut(0.0f, 1.0f, 3.0f, 0.3f);

    public Vector3 myEndPosition = Vector3.zero;
    public Vector3 myEndRotation = Vector3.forward;

    [Tooltip("Will use steering to get to the end position, but will not take myEndRotation into account")]
    public bool myUseSteering = false;

    private Collider myCollider;

    private IEnumerator DoWindTraversal(Transform aPlayerTransform)
    {
        Movement playerMovementScript = aPlayerTransform.GetComponent<Movement>();
        Rigidbody playerRigidbody = aPlayerTransform.GetComponent<Rigidbody>();
        playerMovementScript.enabled = false;

        Vector3 playerVelocity = playerRigidbody.velocity;
        float playerStartSpeed = Mathf.Max(playerVelocity.magnitude, 2.5f);

        playerRigidbody.isKinematic = true;

        Vector3 endPosition = transform.TransformPoint(myEndPosition);
        Vector3 endRotation = transform.TransformDirection(myEndRotation);

        float windStartTime = Time.time;
        float targetSpeed = myPlayerSpeed > playerStartSpeed ? myPlayerSpeed : playerStartSpeed * 2.0f;
        float playerSpeed;

        do
        {
            Vector3 playerToTarget = endPosition - playerRigidbody.position;
            if (playerToTarget.sqrMagnitude <= myEndZoneSize * myEndZoneSize)
            {
                break;
            }

            playerSpeed = mySpeedMultiplierOverTime.Evaluate(Time.time - windStartTime) * (targetSpeed - playerStartSpeed) + playerStartSpeed;

            if (myUseSteering)
            {
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
                playerVelocity = Vector3.RotateTowards(playerVelocity, playerToTarget, myVelocityRotationSpeed * Mathf.Deg2Rad * Time.deltaTime, 0.0f).normalized;

#if UNITY_EDITOR
                Debug.DrawLine(playerRigidbody.position, playerRigidbody.position + playerVelocity, Color.red);
#endif

                playerRigidbody.rotation = Quaternion.RotateTowards(playerRigidbody.rotation, Quaternion.LookRotation(endRotation), myRotationSpeed * Time.deltaTime);
                playerRigidbody.position += playerVelocity * playerSpeed * Time.deltaTime;
            }

            yield return null;
        } while (true);

        playerRigidbody.isKinematic = false;
        playerRigidbody.velocity = playerVelocity;

        playerMovementScript.enabled = true;
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
