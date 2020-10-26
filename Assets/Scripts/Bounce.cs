using System.Collections;
using UnityEngine;

public class Bounce : MonoBehaviour
{
    public float rotateTime = .1f;
    public float ignoreCollisionTime = .35f;
    public Collider myCollider;

    private void OnCollisionEnter(Collision collision)
    {
        StartCoroutine(DoBounce(transform.forward, Vector3.Reflect(transform.forward, collision.contacts[0].normal), collision.collider));
    }

    IEnumerator DoBounce(Vector3 start, Vector3 end, Collider aCollider)
    {
        bool didPlayAudio = false;
        float t = 0;

        Physics.IgnoreCollision(myCollider, aCollider, true);
        float currentZ = transform.eulerAngles.z;

        while (t < 1)
        {
            if (t > 0.0f && !didPlayAudio)
            {
                if (!StageManager.ourInstance.myIsPlayerDead)
                {
                    GameManager.ourInstance.myAudioManager.PlaySFXClip("player_hit_wall");
                }

                didPlayAudio = true;
            }

            t += Time.deltaTime / rotateTime;

            if (t > 1)
            {
                t = 1;
            }
        
            transform.forward = Vector3.Lerp(start, end, t);
            Vector3 eulers = transform.eulerAngles;
            eulers.z = currentZ;
            transform.eulerAngles = eulers;

            yield return null;
        }

        yield return new WaitForSeconds(ignoreCollisionTime);
        Physics.IgnoreCollision(myCollider, aCollider, false);
    }

}
