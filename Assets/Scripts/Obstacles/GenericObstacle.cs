using UnityEngine;

public class GenericObstacle : MonoBehaviour
{

    ShakeableTransform myCameraShake;

    [SerializeField] private bool myStunnedOnImpact = true;
    [SerializeField] private float myTimeBeforeDeathScreen = 3;
    private float mydeadTime;

    private void Awake()
    {

        mydeadTime = myTimeBeforeDeathScreen;

        //NOTE: Will be implemented today
        myCameraShake = Camera.main.GetComponent<ShakeableTransform>();
    }

    private void Update()
    {
        if (mydeadTime < myTimeBeforeDeathScreen)
        {
            mydeadTime -= Time.deltaTime;
        }
        if (mydeadTime < 0)
        {
            UI.EndScreenMenu.ourInstance.DisplayEndScreen(false);
            mydeadTime = myTimeBeforeDeathScreen;

            if (myStunnedOnImpact)
            {
                GetComponent<PlaneController>().enabled = true;
                GetComponent<Rigidbody>().useGravity = false;
            }
        }
    }

    public void OnCollisionEnter(Collision aCollision)
    {
        Debug.Assert(aCollision.contactCount > 0, "Collision without contact points!");

        ContactPoint contactPoint = aCollision.GetContact(0);

        float angle = Vector3.Angle(transform.forward, -contactPoint.normal);

        if (angle < 45.0f)
        {
            StageManager.ourInstance.OnPlayerDied();

            GetComponentInChildren<Animator>().Play("Dead");
            mydeadTime -= Time.deltaTime;

            if (myStunnedOnImpact)
            {
                GetComponent<PlaneController>().enabled = false;
                GetComponent<Rigidbody>().useGravity = true;

                if (myCameraShake != null)
                {
                    myCameraShake.ShakeCamera();

                }
            }
        }
    }
}
