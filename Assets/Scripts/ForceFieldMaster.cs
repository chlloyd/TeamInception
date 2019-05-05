using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceFieldMaster : MonoBehaviour
{    
    static public ForceFieldMaster instance;

    [SerializeField]
    private Texture[] fieldTextures;

    [SerializeField]
    private RippleAffector[] affectors;

    public void SetTicketCount(int count) {
        if(count > fieldTextures.Length) count = fieldTextures.Length;
        if(count < 0) count = 0;
        for(int i = 0; i < affectors.Length; i++) affectors[i].rippleMat.SetTexture(0, fieldTextures[count]);
    }

    // Start is called before the first frame update
    void Start()
    {
        if(instance == null) {
            instance = this;
        } else Destroy(this);
    }
}
