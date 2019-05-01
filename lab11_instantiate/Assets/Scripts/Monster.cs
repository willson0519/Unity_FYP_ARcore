using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }
    public int hitPoint = 10;
    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (hitPoint > 0)
        {
            hitPoint--;

            Vector3 currentscale = transform.localScale;
            transform.localScale = new Vector3(currentscale.x * 0.9f, currentscale.y * 0.9f, 
                currentscale.z * 0.9f);

        }
        else
        {
            Destroy(this.gameObject);
        }
    }
}
