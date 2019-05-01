using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gunLaser2 : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    public float speed = 10f;

    private int delayFrames = 0;
    public int FireAfterFrame = 10;
    public float rotationalSpeed = 2f;
    public GameObject projectilePrefab;
    private GameObject projectile;

    // Update is called once per frame
    void Update()
    {
        float mxVal = Input.GetAxis("Mouse X");
        float myVal = Input.GetAxis("Mouse Y");
        transform.Rotate(-myVal * rotationalSpeed, mxVal * rotationalSpeed, 0f);

        float fireVal = Input.GetAxis("Fire1");

        if (fireVal == 1)
        {
            if (delayFrames >= FireAfterFrame)
            {
                projectile = Instantiate(projectilePrefab) as GameObject;
                projectile.transform.position = transform.TransformPoint(Vector3.forward * 3f);
                projectile.transform.rotation = transform.rotation;
                delayFrames = 0;
            }
            else
            {
                delayFrames++;
            }

        }

    }
}
