using UnityEngine;

public class GenericObstacle : MonoBehaviour
{


    [SerializeField] private bool myStunnedOnImpact = true;
    [SerializeField] private float myTimeBeforeDeathScreen = 3;
    ShakeableTransform myCameraShake;
    private float mydeadTime;
    private void Awake()
    {
        mydeadTime = myTimeBeforeDeathScreen;


        myCameraShake = Camera.main.GetComponent<ShakeableTransform>();
    }

    public void OnCollisionEnter(Collision aCollision)
    {



        Debug.Assert(aCollision.contactCount > 0, "Collision without contact points!");

        ContactPoint contactPoint = aCollision.GetContact(0);



        float angle = Vector3.Angle(transform.forward, -contactPoint.normal);

        //transform.Rotate(Vector3.Cross(transform.forward, contactPoint.normal), angle);

        print(angle);

        if (angle < 45.0f)
        {

            GetComponentInChildren<Animator>().Play("Dead");
            mydeadTime -= Time.deltaTime;
            if (myStunnedOnImpact)
            {

                GetComponent<PlaneController>().enabled = false;
                myCameraShake.ShakeCamera(1);

            }
        }

        //float angle = Vector3.Angle(aCollision.collider.transform.forward, contactPoint.normal);
        //if (angle < 45.0f)
        //{
        //    // TODO: Kill player
        //}


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
            }


        }
    }
}


