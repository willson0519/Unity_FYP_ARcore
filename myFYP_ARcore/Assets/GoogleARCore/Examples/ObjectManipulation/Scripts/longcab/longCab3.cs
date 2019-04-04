using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class longCab3 : MonoBehaviour
{
    public Texture[] textures;
    public int currentTexture;
    public int plastic;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    public void SwapLong3()
    {

        currentTexture++;
        currentTexture %= textures.Length;
        GetComponent<Renderer>().material.mainTexture = textures[plastic];
    }
}
