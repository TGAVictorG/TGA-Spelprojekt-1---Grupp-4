using UnityEngine;

public class DiscoBall : MonoBehaviour
{
    [SerializeField]
    private float myRotationSpeed = 20.0f;

    [SerializeField]
    private float myLossScreenDelay = 2.0f;

    [SerializeField]
    private float myLaserRadius = 1.2f;

    [SerializeField]
    private float myOvershoot = 0.2f;

    [SerializeField]
    private Transform[] myLaserTransforms;

    private Rigidbody myRigidbody;
    private int myLayerMask;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.ourInstance.myAudioManager.PlaySFXClip("lazer_kill");
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

    private void UpdateLaserScale()
    {
        Vector3 globalScale = transform.lossyScale;
        for (int i = 0; i < myLaserTransforms.Length; ++i)
        {
            Transform laserTransform = myLaserTransforms[i];

            float length = 50.0f;

            Vector3 origin = laserTransform.position - laserTransform.forward;
            Ray ray = new Ray(origin, laserTransform.forward);

#if UNITY_EDITOR
            Debug.DrawRay(ray.origin, ray.direction, Color.red);
#endif

            if (Physics.Raycast(ray, out RaycastHit hitInfo, Mathf.Infinity, myLayerMask))
            {
                length = (hitInfo.distance - 1.0f + myOvershoot) / (2.0f * globalScale.x);
            }

            laserTransform.localScale = new Vector3(myLaserRadius, myLaserRadius, length);
        }
    }

    private void FixedUpdate()
    {
        myRigidbody.rotation *= Quaternion.AngleAxis(myRotationSpeed * Time.deltaTime, Vector3.up);
        UpdateLaserScale();
    }

    private void Awake()
    {
        myRigidbody = GetComponent<Rigidbody>();
        myLayerMask = ~(1 << LayerMask.NameToLayer("Ignore Raycast"));

        UpdateLaserScale();
    }
}
