using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reactivator : MonoBehaviour
{
    [SerializeField]
    private GameObject[] objs;

    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < objs.Length; i++) objs[i].SetActive(true);
    }
}
