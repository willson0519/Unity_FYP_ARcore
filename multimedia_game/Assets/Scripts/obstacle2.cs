using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class obstacle2 : MonoBehaviour
{
    // Start is called before the first frame update
    public float objSpeed = 0.01f;
    int mymultiplier = 1;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 currentpos = transform.localPosition;
        transform.localPosition = new Vector3(currentpos.x , currentpos.y + (objSpeed * mymultiplier), currentpos.z);

    }


    private void OnTriggerEnter(Collider other)
    {
        mymultiplier = mymultiplier * -1;


    }
}
