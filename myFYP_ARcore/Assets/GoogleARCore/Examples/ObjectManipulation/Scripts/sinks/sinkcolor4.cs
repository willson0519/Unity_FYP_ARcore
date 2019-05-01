using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sinkcolor4 : MonoBehaviour
{
    public Texture[] textures;
    public int currentTexture;
    public int blue;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    public void sinkBody4()
    {

        currentTexture++;
        currentTexture %= textures.Length;
        GetComponent<Renderer>().material.mainTexture = textures[blue];
    }
}
