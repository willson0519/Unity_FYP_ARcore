using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public float rotationSpeed = 2f;


    // Update is called once per frame
    void Update()
    {

        float mxVal = Input.GetAxis("Mouse X");
        float myVal = Input.GetAxis("Mouse Y");

        transform.Rotate(-myVal * rotationSpeed, mxVal * rotationSpeed, 0);

        float fire1Val = Input.GetAxis("Fire1");

        if (fire1Val>0)
         {
            CheckForRaycastHit();
         }
        
    }
       
        void CheckForRaycastHit()
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.forward, out hit,10f))
            {
            //print("There is something in front of the screen.");
            //print(hit.collider.gameObject.name + "is in front of the screen.");
            //print(hit.collider.gameObject.name + "is destroyed.");
            //Destroy(hit.collider.gameObject);
            StartCoroutine(HitIndicator(hit.point));
        }

        }
    private IEnumerator HitIndicator(Vector3 hitposition)
    {
        GameObject hitmark = GameObject.CreatePrimitive(PrimitiveType.Sphere);

        hitmark.transform.localScale = new Vector3(0.1f, 0.1f, 0.11f);
        hitmark.transform.position = hitposition;
        yield return new WaitForSeconds(1);
    }

    }

