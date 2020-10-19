using System.Collections;
using UnityEngine;

public class PlayerDeathHandler : MonoBehaviour
{
    public enum DeathReason
    {
        Collision,
        Laser
    }

    private PlaneController myPlaneController;
    private Rigidbody myRigidbody;
    private Animator myAnimator;

    private ShakeableTransform myCameraShake;

    public void Kill(DeathReason aDeathReason, float aDelay, bool aStunPlayer = true)
    {
        if (StageManager.ourInstance.myIsPlayerDead)
        {
            return;
        }

        StageManager.ourInstance.OnPlayerDied();

        if (aStunPlayer)
        {
            myPlaneController.enabled = false;
            myRigidbody.useGravity = true;

            myCameraShake.ShakeCamera();
        }

        if (aDeathReason == DeathReason.Collision)
        {
            myAnimator.Play("Dead");
        }

        StartCoroutine(DelayedDeath(aDelay));
    }

    private IEnumerator DelayedDeath(float aDelay)
    {
        yield return new WaitForSeconds(aDelay);

        UI.EndScreenMenu.ourInstance.DisplayEndScreen(false);
    }

    private void Awake()
    {
        myPlaneController = GetComponent<PlaneController>();
        myRigidbody = GetComponent<Rigidbody>();
        myAnimator = GetComponentInChildren<Animator>();
    }

    private void Start()
    {
        myCameraShake = Camera.main.GetComponent<ShakeableTransform>();
    }
}
