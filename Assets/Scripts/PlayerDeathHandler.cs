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

        switch(aDeathReason)
        {
            case DeathReason.Collision:
                myAnimator.Play("Dead");                
                GameManager.ourInstance.myAudioManager.PlaySFXClip("player_death");
                break;
            case DeathReason.Laser:
                GameManager.ourInstance.myAudioManager.PlaySFXClip("lazer_kill");
                break;
        }

        GameManager.ourInstance.myAudioManager.PlaySFXClip("player_loss");
        GameManager.ourInstance.myAudioManager.PlayVoiceClip("stage_defeat");

        StartCoroutine(DelayedDeath(aDelay));
    }

    private IEnumerator DelayedDeath(float aDelay)
    {
        yield return new WaitForSeconds(aDelay);

        if (StageManager.ourInstance.myCurrentCheckpoint != null)
        {
            // Restore plane controls
            myRigidbody.useGravity = false;
            myPlaneController.myCurrentVelocity = 0f;
            myRigidbody.velocity = Vector3.zero;
            myRigidbody.angularVelocity = Vector3.zero;

            // Restore model
            myAnimator.Play("Flying");
            
            StageManager.ourInstance.RestartFromCheckpoint();
            
            // Invoke countdown again
            Camera.main.transform.parent.gameObject.GetComponentInChildren<Countdown>().restart = true; // Resume movement is handled by Countdown callback

        }
        else
        {
            UI.EndScreenMenu.ourInstance.DisplayEndScreen(false);
        }
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
