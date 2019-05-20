using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ticket : MonoBehaviour
{
    [SerializeField]
    private Transform attachPoint;

    void OnTriggerEnter(Collider col) {
        //If the player touched the ticket,
        if(col.tag == "Player") {
            //bind the ticket to the attachment point.
            transform.parent = attachPoint;
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;
            transform.localScale = Vector3.one;

            //Tell the game controller a ticket was picked up.
            GameController.instance.AddTicket();

            //Destroy this script afterwards:
            //it is unnecessary.
            Destroy(this);
        }
    }
}
