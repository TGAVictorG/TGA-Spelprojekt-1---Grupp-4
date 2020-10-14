using UnityEngine;

public class CameraZoomZone : MonoBehaviour
{
    [SerializeField] private float myCameraMoveSpeedMultiplier = 1.5f;

    [SerializeField] private float myToTargetUpMultiplier = 0.5f;
    [SerializeField] private float myToTargetBackMultiplier = 0.5f;

    private CameraFollow myCameraFollow;

    private void Start()
    {
        myCameraFollow = Camera.main.GetComponent<CameraFollow>();
    }

    private void OnTriggerEnter(Collider anOther)
    {
        if (anOther.CompareTag("Player"))
        {
            myCameraFollow.myDistanceToTargetUp *= myToTargetUpMultiplier;
            myCameraFollow.myDistanceToTargetBack *= myToTargetBackMultiplier;

            myCameraFollow.myMoveSpeed *= myCameraMoveSpeedMultiplier;
        }
    }

    private void OnTriggerExit(Collider anOther)
    {
        if (anOther.CompareTag("Player"))
        {
            myCameraFollow.myMoveSpeed /= myCameraMoveSpeedMultiplier;

            myCameraFollow.myDistanceToTargetBack /= myToTargetBackMultiplier;
            myCameraFollow.myDistanceToTargetUp /= myToTargetUpMultiplier;
        }
    }
}
