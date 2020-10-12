using UnityEngine;

public class DiscoBall : MonoBehaviour
{
    [SerializeField]
    private float myRotationSpeed = 20.0f;

    private Rigidbody myRigidbody;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // TODO: Kill player
            Debug.Log("Kill player");
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // TODO: Kill player
            Debug.Log("Kill player");
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
