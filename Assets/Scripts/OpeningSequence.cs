using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

/*
 * OpeningSequence CLASS
 * 
 * Class that handles the dynamic
 * aspects of the cinema e.g. the
 * door that appears in the screen.
 *
 * -Kieran
 */

public class OpeningSequence : MonoBehaviour
{
    private VideoPlayer vp;

    void Start() {
        vp = gameObject.GetComponent<VideoPlayer>();
        //This is an EventHandler. Adding a
        //function pointer to it means that
        //when this event occurs, the listening
        //function (ShowDoor()) automatically runs.
        vp.loopPointReached += ShowDoor;
    }

    void ShowDoor(VideoPlayer vp) {
        //TODO: Display the door and its teleport point (?)
        Debug.Log("ShowDoor()");
    }
}
