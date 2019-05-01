using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class handle_2 : MonoBehaviour
{
    public Texture[] textures;
    public int currentTexture;
    public int steel;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    public void handleColor2()
    {

        currentTexture++;
        currentTexture %= textures.Length;
        GetComponent<Renderer>().material.mainTexture = textures[steel];
    }
}
