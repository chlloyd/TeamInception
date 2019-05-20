using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingAnimator : MonoBehaviour
{
    /*
     * BuildingAnimator CLASS
     * 
     * Class that procedurally performs the
     * "fall into place" animations for each
     * building part.
     *
     * -Kieran
     */
    
    //Speed coefficient for the 'falling'.
    private const float SPEED = 1.0f;
    //Position toward which the object is falling.
    private Vector3 targetPos = Vector3.zero;
    //Whether or not the object is currently falling.
    private bool active = false;

    // Start is called before the first frame update
    void Start()
    {
        gameObject.GetComponent<MeshRenderer>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(!active) return;

        Vector3 translation = (targetPos - transform.position) * Time.deltaTime * SPEED;
        //Check if the object's next movement will overshoot its target position.
        if(translation.sqrMagnitude < (targetPos - transform.position).sqrMagnitude) {
            //Will not overshoot its target position: continue moving.
            transform.position += translation;
        }
        else {
            //Is about to overshoot is target position:
            //move to the target position.
            transform.position = targetPos;
            //This script is no longer needed after reaching
            //the target position.
            Destroy(this);
        }
    }

    //Reveals the object, and
    //begins the falling animation.
    public void Activate() {
        if(active) return;

        active = true;
        transform.position = (transform.position + Vector3.up * transform.position.magnitude) * 100;
        gameObject.GetComponent<MeshRenderer>().enabled = true;
    }
}
