using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


public class RippleAffector : MonoBehaviour
{
    static public Vector4[] ripples = new Vector4[10];
    static public int nextIndex = 0;
    private float lastUpdateTime = 0;
    private Material rippleMat;

    // Start is called before the first frame update
    void Start()
    {
        rippleMat = gameObject.GetComponent<MeshRenderer>().material;
        for(int i = 0; i < ripples.Length; i++) ripples[i] = Vector4.zero;
    }

    // Update is called once per frame
    void Update()
    {
        rippleMat.SetFloat("_CurrentTime", Time.timeSinceLevelLoad);
    }

    void OnTriggerStay(Collider col) {
        if(col.tag == "Player") {
            if(Time.time - lastUpdateTime > 0.5f) {
            ripples[nextIndex] = new Vector4(col.transform.position.x, col.transform.position.y, col.transform.position.z, Time.timeSinceLevelLoad);
            nextIndex++;
            if(nextIndex >= 10) nextIndex = 0;
            lastUpdateTime = Time.time;

            rippleMat.SetVectorArray("_RippleOrigins", ripples);
        }
        }
    }
}
