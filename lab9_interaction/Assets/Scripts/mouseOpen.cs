using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mouseOpen : MonoBehaviour
{
   public GameObject target;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseDown()
    {
        target.transform.Rotate(0f, 45f, 0f);
    }
}
