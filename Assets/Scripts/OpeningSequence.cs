using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using Valve.VR.InteractionSystem;

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

    [SerializeField]
    private GameObject cinema;

    [SerializeField]
    private Transform player;
    [SerializeField]
    private Transform door;
    [SerializeField]
    private Transform startPoint;
    
    [SerializeField]
    private UnityEngine.UI.Image fadePanel;

    [SerializeField]
    private TeleportPoint exitPoint;

    private bool fadingOut = false;

    void Start() {
        vp = gameObject.GetComponent<VideoPlayer>();
        //This is an EventHandler. Adding a
        //function pointer to it means that
        //when this event occurs, the listening
        //function (ShowDoor()) automatically runs.
        vp.loopPointReached += ShowDoor;
    }

    void Update() {
        if(Vector3.Distance(player.position, exitPoint.transform.position) < 2.0f && !fadingOut)
            StartCoroutine(CoroutineFadeTransition());
    }

    private void ShowDoor(VideoPlayer vp) {
        //Display the door and its teleport point.
        StartCoroutine(CoroutineShowDoor());
    }

    private IEnumerator CoroutineShowDoor() {
        while(door.position.y < 1.3f) {
            door.Translate(Vector3.up * Time.deltaTime * 1.4f);
            yield return new WaitForEndOfFrame();
        }
        if(door.position.y > 1.3f) door.position = new Vector3(door.position.x, 1.3f, door.position.z);

        //After the door is revealed,
        //activate the exit point.
        exitPoint.gameObject.SetActive(true);
    }

    private IEnumerator CoroutineFadeTransition() {
        fadingOut = true;

        //Fade the screen to white.
        while(fadePanel.color.a < 1) {
            fadePanel.color = new Color(fadePanel.color.r, fadePanel.color.g, fadePanel.color.b, fadePanel.color.a + Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }
        if(fadePanel.color.a > 1) fadePanel.color = new Color(fadePanel.color.r, fadePanel.color.g, fadePanel.color.b, 1);
        
        //Turn the lights back on.
        RenderSettings.ambientIntensity = 1;
        RenderSettings.reflectionIntensity = 1;

        //Teleport the player to the station start area.
        player.position = startPoint.position;
        player.rotation = startPoint.rotation;

        yield return new WaitForSeconds(1.5f);

        //Fade the screen back to normal.
        while(fadePanel.color.a > 0) {
            fadePanel.color = new Color(fadePanel.color.r, fadePanel.color.g, fadePanel.color.b, fadePanel.color.a - Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }
        if(fadePanel.color.a < 0) fadePanel.color = new Color(fadePanel.color.r, fadePanel.color.g, fadePanel.color.b, 0);

        //After the player is moved to the station,
        //the cinema is no longer needed.
        Destroy(cinema);
    }
}
