using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class handle_1 : MonoBehaviour
{
    public Texture[] textures;
    public int currentTexture;
    public int black;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    public void handleColor1()
    {

        currentTexture++;
        currentTexture %= textures.Length;
        GetComponent<Renderer>().material.mainTexture = textures[black];
    }
}
