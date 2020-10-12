using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class jumpingFishResetTrigger : MonoBehaviour
{

    public GameObject myJumpingFish;

    public float myJumpForce = 10;
    public float myMaxjumpHeight = 4;

    public Color myGizmoColor = Color.white;

    Rigidbody rb;

    private void Start()
    {
        rb = myJumpingFish.GetComponent<Rigidbody>();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = myGizmoColor;
        Gizmos.DrawWireSphere(new Vector3(transform.position.x, transform.position.y + myMaxjumpHeight + 0.5f, transform.position.z), 0.25f);
        Gizmos.DrawWireCube(transform.position, new Vector3(0.5f,0.5f,0.5f));
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject == myJumpingFish)
        {
            myJumpingFish.GetComponent<Rigidbody>().velocity = Vector3.zero;

            MakeFishJump();
        }
    }

    private void MakeFishJump()
    {
        rb.velocity = new Vector3(0, Mathf.Sqrt(-2.0f * Physics.gravity.y * myMaxjumpHeight));
    }
}
