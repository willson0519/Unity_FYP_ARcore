using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CallingColor1 : MonoBehaviour
{
    public textureChange1 _textureChange1;
    // Start is called before the first frame update
    public void SwapTexture()
    {
        _textureChange1.SwapTexture();
    }
}
