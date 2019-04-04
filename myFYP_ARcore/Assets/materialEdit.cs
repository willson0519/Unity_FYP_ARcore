using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class materialEdit : MonoBehaviour
{
    MeshRenderer meshRenderer;
    public Texture tableTop;
    public Texture original;
    // Start is called before the first frame update
    void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
    }

    public void ChangeTexture()
    {
        meshRenderer.material.SetTexture("_MainTex", tableTop);
    }
    public void OriTexture()
    {
        meshRenderer.material.SetTexture("_MainTex", original);
    }
}
