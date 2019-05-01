using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakeScreenshot : MonoBehaviour
{

    public GameObject snackers;
    private void Start()
    {
        snackers.SetActive(false);
    }

    // Update is called once per frame
    public void screenShot()
    {
        StartCoroutine("CaptureIt");
        snackers.SetActive(true);
        StartCoroutine("waitForSec");

    }

    IEnumerator CaptureIt()
    {
        string timeStamp = System.DateTime.Now.ToString("dd-MM-yyy-HH-mm-ss");
        string fileName = "Screenshot" + timeStamp + ".png";
        string pathToSave = "/storage/emulated/0/Android/data/com.fyp.kitchen/screenshots";
        ScreenCapture.CaptureScreenshot(pathToSave);
        yield return new WaitForEndOfFrame();
        
    }

    IEnumerator waitForSec()
    {
        yield return new WaitForSeconds(3);
        Destroy(snackers);
    }

}
