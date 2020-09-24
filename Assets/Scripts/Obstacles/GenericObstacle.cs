using UnityEngine;

public class GenericObstacle : MonoBehaviour
{
    public void OnCollisionEnter(Collision aCollision)
    {
        Debug.Assert(aCollision.contactCount > 0, "Collision without contact points!");

        ContactPoint contactPoint = aCollision.GetContact(0);

        float angle = Vector3.Angle(aCollision.collider.transform.forward, contactPoint.normal);
        if (angle < 45.0f)
        {
            // TODO: Kill player
        }
    }
}
