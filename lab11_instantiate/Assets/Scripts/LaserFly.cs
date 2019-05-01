using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserFly : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }
    public float speed = 10f;
    // Update is called once per frame
    void Update()
    {
        transform.Translate(0, 0, speed * Time.deltaTime);

        if(transform.position.z >= 10f)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
       Destroy(this.gameObject);
        //Destroy(other.gameObject);
    }
}
