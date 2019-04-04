using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class longTop3 : MonoBehaviour
{
    public Texture[] textures;
    public int currentTexture;
    public int red;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    public void LongTop3()
    {

        currentTexture++;
        currentTexture %= textures.Length;
        GetComponent<Renderer>().material.mainTexture = textures[red];
    }
}
