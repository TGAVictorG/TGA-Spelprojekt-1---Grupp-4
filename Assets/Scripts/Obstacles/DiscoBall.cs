using Managers;
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

    [Header("Audio")]
    [SerializeField]
    private AudioClip myLazerBuzzClip;

    [SerializeField]
    private float myLazerBuzzVolume = 0.45f;

    [SerializeField]
    private float myLazerBuzzMinDistance = 1.0f;

    [SerializeField]
    private float myLazerBuzzMaxDistance = 2.0f;

    private AudioSource[] myAudioSources;

    private Transform myPlayer;

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

    private void UpdateAudioSourcePosition(
        Vector3 aPointA,
        Vector3 aPointB,
        AudioSource anAudioSource)
    {
        Debug.Assert(myPlayer != null, "Updating AudioSources with myPlayer == null!");

        Vector3 startToPlayer = myPlayer.position - aPointA;
        Vector3 line = aPointB - aPointA;
        float lineLength = line.magnitude;
        line /= lineLength;

        float distanceAlongLine = Mathf.Clamp(Vector3.Dot(startToPlayer, line), 0.0f, lineLength);

        anAudioSource.transform.position = aPointA + distanceAlongLine * line;
    }

    private void UpdateLasers()
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

            UpdateAudioSourcePosition(laserTransform.position, laserTransform.position + laserTransform.forward * length * 2.0f * globalScale.x, myAudioSources[i]);
        }
    }

    private void GenerateAudioSources()
    {
        if (myLaserTransforms.Length == 0)
        {
#if UNITY_EDITOR
            Debug.LogWarning("No laser transforms!");
#endif
            return;
        }

        myAudioSources = new AudioSource[myLaserTransforms.Length];
        for (int i = 0; i < myAudioSources.Length; ++i)
        {
            GameObject audioGo = new GameObject("LazerBuzzAudioSource");
            audioGo.transform.SetParent(myLaserTransforms[i]);
            audioGo.transform.localPosition = Vector3.zero;

            AudioSource audioSource = audioGo.AddComponent<AudioSource>();

            // Setup props
            audioSource.spatialBlend = 1.0f;
            if (GameManager.ourInstance.myAudioManager != null)
            {
                audioSource.outputAudioMixerGroup = GameManager.ourInstance.myAudioManager.GetMixerFromAudioType(AudioManager.AudioType.SFX);
            }
            audioSource.loop = true;
            audioSource.playOnAwake = true;
            audioSource.clip = myLazerBuzzClip;
            audioSource.volume = myLazerBuzzVolume;
            audioSource.minDistance = myLazerBuzzMinDistance;
            audioSource.maxDistance = myLazerBuzzMaxDistance;

            audioSource.Play();

            myAudioSources[i] = audioSource;
        }
    }

    private void FixedUpdate()
    {
        myRigidbody.rotation *= Quaternion.AngleAxis(myRotationSpeed * Time.deltaTime, Vector3.up);
        UpdateLasers();
    }

    private void Start()
    {
        myPlayer = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Awake()
    {
        myRigidbody = GetComponent<Rigidbody>();
        myLayerMask = ~(1 << LayerMask.NameToLayer("Ignore Raycast"));

        GenerateAudioSources();
    }
}
