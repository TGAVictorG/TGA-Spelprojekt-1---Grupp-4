using UnityEngine;

public class DiscoBall : MonoBehaviour
{
    [SerializeField]
    private float myRotationSpeed = 20.0f;

    [SerializeField]
    private float myLossScreenDelay = 2.0f;

    private Rigidbody myRigidbody;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerDeathHandler>().Kill(PlayerDeathHandler.DeathReason.Laser, myLossScreenDelay);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<PlayerDeathHandler>().Kill(PlayerDeathHandler.DeathReason.Laser, myLossScreenDelay);
        }
    }

    private void FixedUpdate()
    {
        myRigidbody.rotation *= Quaternion.AngleAxis(myRotationSpeed * Time.deltaTime, Vector3.up);
    }

    private void Awake()
    {
        myRigidbody = GetComponent<Rigidbody>();
    }
}
