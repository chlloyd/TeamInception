using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class Elevator : MonoBehaviour
{
    [SerializeField]
    private TeleportPoint telePoint;

    [SerializeField]
    private Transform player;

    private bool movingUpwards = true;
    private bool openingDoors = false;
    private bool playerInside = false;

    [SerializeField]
    private Vector3 localDoorOffset;
    private Vector3[] localDoorClosedPos;

    [SerializeField]
    private Transform[] doors;

    private Vector3 startPos;
    [SerializeField]
    private Vector3 endOffset;

    // Start is called before the first frame update
    void Start()
    {
        //Initialise start positions.
        localDoorClosedPos = new Vector3[doors.Length];
        for(int i = 0; i < doors.Length; i++) {
            localDoorClosedPos[i] = doors[i].localPosition;
        }

        startPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        //If the player is in the elevator,
        if(Vector3.Distance(player.position, telePoint.transform.position) < 2.0f) {
            if(!playerInside) {
                //and they weren't already in the elevator,
                //switch the direction of the elevator
                //and close the doors.
                playerInside = true;
                openingDoors = false;
                movingUpwards = !movingUpwards;
            }

            //Keep the player parented to the elevator.
            player.parent = transform;
        } else {
            if(player.parent == transform) player.parent = null;

            //Stay on the same floor as the player.
            if(player.position.y > -5) movingUpwards = true;
            else movingUpwards = false;
        }

        //Open/close the doors.
        if(openingDoors) {
            //Open doors.
            Vector3 translation = localDoorOffset * localDoorOffset.magnitude * Time.deltaTime;

            //First door.
            if(translation.magnitude > Vector3.Distance(doors[0].localPosition, localDoorClosedPos[0] + localDoorOffset)) {
                //Prevent the door from overshooting its target position.
                doors[0].localPosition = localDoorClosedPos[0] + localDoorOffset;
            } else {
                doors[0].localPosition += translation;
            }

            //Second door.
            if(translation.magnitude > Vector3.Distance(doors[1].localPosition, localDoorClosedPos[1] - localDoorOffset)) {
                //Prevent the door from overshooting its target position.
                doors[1].localPosition = localDoorClosedPos[1] - localDoorOffset;
            } else {
                doors[1].localPosition -= translation;
            }
        } else {
            //Close doors.
            Vector3 translation = -localDoorOffset * localDoorOffset.magnitude * Time.deltaTime;

            //First door.
            if(translation.magnitude > Vector3.Distance(doors[0].localPosition, localDoorClosedPos[0])) {
                //Prevent the door from overshooting its target position.
                doors[0].localPosition = localDoorClosedPos[0];
            } else {
                doors[0].localPosition += translation;
            }

            //Second door.
            if(translation.magnitude > Vector3.Distance(doors[1].localPosition, localDoorClosedPos[1])) {
                //Prevent the door from overshooting its target position.
                doors[1].localPosition = localDoorClosedPos[1];
            } else {
                doors[1].localPosition -= translation;
            }
        }

        //If the doors are closed,
        if(doors[0].localPosition == localDoorClosedPos[0]) {
            //move the elevator up or down.
            if(!movingUpwards) {
                //Move towards the end position.
                Vector3 translation = endOffset * Time.deltaTime / 2.0f;
                if(translation.magnitude > Vector3.Distance(transform.position, startPos + endOffset)) {
                    transform.position = startPos + endOffset;
                    openingDoors = true;
                } else {
                    transform.position += translation;
                }
            } else {
                //Move towards the start position.
                Vector3 translation = -endOffset * Time.deltaTime / 2.0f;
                if(translation.magnitude > Vector3.Distance(transform.position, startPos)) {
                    transform.position = startPos;
                    openingDoors = true;
                } else {
                    transform.position += translation;
                }
            }
        }
    }
    
    public void Activate() {
        telePoint.gameObject.SetActive(true);
        openingDoors = true;
    }
}
