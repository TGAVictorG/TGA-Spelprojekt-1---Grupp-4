using UnityEngine;

public class GenericObstacle : MonoBehaviour
{

    public void OnCollisionEnter(Collision aCollision)
    {
        Debug.Assert(aCollision.contactCount > 0, "Collision without contact points!");

        ContactPoint contactPoint = aCollision.GetContact(0);



        float angle = Vector3.Angle(transform.forward, -contactPoint.normal);

        //transform.Rotate(Vector3.Cross(transform.forward, contactPoint.normal), angle);

        print(angle);

        if (angle < 45.0f)
        {
            UI.EndScreenMenu.ourInstance.DisplayEndScreen(false);
        }
    }
}
