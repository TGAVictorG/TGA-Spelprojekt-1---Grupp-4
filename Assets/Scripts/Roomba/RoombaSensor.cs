using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoombaSensor : MonoBehaviour
{
    public GameObject myRoomba;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject != myRoomba)
        {
            myRoomba.GetComponent<Roomba>().myIsLookingForANewPath = true;
           
        }
    }
}
