using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void OnTriggerEnter(Collider other)
    {
        //print(other.gameObject.name + "has entered the cube");
       // Destroy(gameObject);
    }

    private void OnTriggerStay(Collider other)
    {
        // print(other.gameObject.name + "is still in the cube");
        Vector3 currentscale = transform.localScale;
        
        if(currentscale.x >= 2f)
        {
            Destroy(gameObject);
        }
        else
        {
            transform.localScale = new Vector3(currentscale.x * 1.01f,
            currentscale.y * 1.01f, currentscale.z * 1.01f);
        }

    }

    private void OnTriggerExit(Collider other)
    {
        print(other.gameObject.name + "has left the cube");
    }
}
