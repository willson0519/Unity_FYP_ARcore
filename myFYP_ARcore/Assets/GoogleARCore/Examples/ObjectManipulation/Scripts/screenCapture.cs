using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class screenCapture : MonoBehaviour
{
    [SerializeField]

    public void TakeAShot()
    {
        StartCoroutine("CaptureIt");
    }

    IEnumerator CaptureIt()
    {
        string timeStamp = System.DateTime.Now.ToString("dd-MM-yyy-HH-mm-ss");
        string fileName = "ScreenShot" + timeStamp + ".png";
        string pathToSave = "/storage/emulated/0/Android/data/com.fyp.kitchen/screenshots";
        ScreenCapture.CaptureScreenshot(pathToSave);
        yield return new WaitForEndOfFrame();
       
    }
}
