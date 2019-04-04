using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tallTop3 : MonoBehaviour
{
    public Texture[] textures;
    public int currentTexture;
    public int red2;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    public void TopTall3()
    {

        currentTexture++;
        currentTexture %= textures.Length;
        GetComponent<Renderer>().material.mainTexture = textures[red2];
    }
}
