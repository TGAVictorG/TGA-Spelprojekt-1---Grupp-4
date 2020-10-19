using UnityEngine;

public class GenericObstacle : MonoBehaviour
{
    [SerializeField] private bool myStunnedOnImpact = true;
    [SerializeField] private float myTimeBeforeDeathScreen = 3;

    private PlayerDeathHandler myDeathHandler;

    private void Awake()
    {
        myDeathHandler = GetComponent<PlayerDeathHandler>();
    }

    public void OnCollisionEnter(Collision aCollision)
    {
        Debug.Assert(aCollision.contactCount > 0, "Collision without contact points!");

        ContactPoint contactPoint = aCollision.GetContact(0);

        float angle = Vector3.Angle(transform.forward, -contactPoint.normal);

        if (angle < 45.0f)
        {
            myDeathHandler.Kill(PlayerDeathHandler.DeathReason.Collision, myTimeBeforeDeathScreen, myStunnedOnImpact);
        }
    }
}
