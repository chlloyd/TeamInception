using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

/* GameController CLASS
 *
 * Singleton that oversees the game in general.
 * Keeps track of the number of tickets
 * obtained by the player.
 *
 * -Kieran
  */
public class GameController : MonoBehaviour
{    
    static public GameController instance;

    [SerializeField]
    private Transform player;

    [SerializeField]
    private Texture[] fieldTextures;

    [SerializeField]
    private RippleAffector[] affectors;

    [SerializeField]
    private TeleportArea platformTeleportArea;
    [SerializeField]
    private TeleportPoint trainFrontPoint;
    [SerializeField]
    private TeleportPoint trainBackPoint;

    [SerializeField]
    private UnityEngine.UI.Image fadePanel;

    [SerializeField]
    private Elevator[] elevators;

    private int ticketsHeld = 0;

    private bool fadingOut = false;

    public void AddTicket() {
        ticketsHeld++;
        if(ticketsHeld >= 10) {
            ticketsHeld = 10;
            
            //Disable the forcefields.
            for(int i = 0; i < affectors.Length; i++) {
                affectors[i].gameObject.SetActive(false);
            }

            //Enable the platform-level teleport system.
            platformTeleportArea.gameObject.SetActive(true);
            trainFrontPoint.gameObject.SetActive(true);
            trainBackPoint.gameObject.SetActive(true);

            for(int i = 0; i < elevators.Length; i++) {
                elevators[i].Activate();
            }
        } else {
            //Update the ticket display on the forcefields.
            for(int i = 0; i < affectors.Length; i++) affectors[i].rippleMat.SetTexture(0, fieldTextures[ticketsHeld]);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        //Enforce Singleton structure.
        if(instance == null) {
            instance = this;
        } else Destroy(this);
    }

    void Update() {
        //End the game if the player boards the train.
        if(!fadingOut) {
            if(Vector3.Distance(player.position, trainFrontPoint.transform.position) < 1
            || Vector3.Distance(player.position, trainBackPoint.transform.position) < 1) {
                StartCoroutine(CoroutineFadeOut());
            }
        }
    }

    private IEnumerator CoroutineFadeOut() {
        fadingOut = true;
        
        //Fade the screen to white.
        while(fadePanel.color.a < 1) {
            fadePanel.color = new Color(fadePanel.color.r, fadePanel.color.g, fadePanel.color.b, fadePanel.color.a + Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }

        yield return new WaitForSeconds(1);

        //End the game.
        Application.Quit();
    }
}
