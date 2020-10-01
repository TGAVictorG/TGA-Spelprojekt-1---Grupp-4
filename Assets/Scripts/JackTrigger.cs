using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JackTrigger : MonoBehaviour
{
    public JackInABox myJackInABox;


    private void Update()
    {
    }

    private void OnTriggerEnter(Collider other)
    {        
        if (other.CompareTag("Player"))
        {
            myJackInABox.Trigger();
        }
    }
}
