using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpingFish : MonoBehaviour
{

    public float JumpForce = 10f;

    public float jumpHeight = 5f;

    Rigidbody myRigidbody;
    public Vector3 originalHeight;

    // Start is called before the first frame update
    void Start()
    {
        myRigidbody = GetComponent<Rigidbody>();

        originalHeight = transform.localPosition;

        Jump();

    }

    private void Update()
    {
        if(transform.position.y >= (originalHeight.y + jumpHeight))
        {
            myRigidbody.velocity = Vector3.zero;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(transform.up * JumpForce, 0.5f);
    }

    public void Jump()
    {
        myRigidbody.AddForce(transform.up * JumpForce, ForceMode.Impulse);
    }
}
