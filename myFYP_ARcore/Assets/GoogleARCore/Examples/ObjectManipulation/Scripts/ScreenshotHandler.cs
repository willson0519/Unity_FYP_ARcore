using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenshotHandler : MonoBehaviour
{
    Camera snapCam;
    

    int resWidth = 256;
    int resHeight = 256;
    private object textureFormat;

    // Start is called before the first frame update
    void Awake()
    {
        snapCam = GetComponent<Camera>();
        if(snapCam.targetTexture == null)
        {
            snapCam.targetTexture = new RenderTexture(resWidth, resHeight, 24);

        }
        else
        {
            resWidth = snapCam.targetTexture.width;
            resHeight = snapCam.targetTexture.height;
        }
        snapCam.gameObject.SetActive(false);
    }

    // Update is called once per frame
    public void takeSnap()
    {
        CallTakeSnapShot();
       
    }

    public void CallTakeSnapShot()
    {
        snapCam.gameObject.SetActive(true);
    }

    void LateUpdate()
    {
        if (snapCam.gameObject.activeInHierarchy)
        {
            Texture2D snapshot = new Texture2D(resWidth, resHeight,TextureFormat.RGB24, false);
            snapCam.Render();
            RenderTexture.active = snapCam.targetTexture;
            snapshot.ReadPixels(new Rect(0, 0, resWidth, resHeight), 0, 0);
            byte[] bytes = snapshot.EncodeToPNG();
            string filename = SnapshotName();
            System.IO.File.WriteAllBytes("/storage/emulated/0/Android/data/com.fyp.kitchen/screenshots", bytes);
            snapCam.gameObject.SetActive(false);
        }
        string SnapshotName()
        {
            return string.Format("{0}/Snapshots/snap_{1}x{2}_{3}.png", 
                Application.dataPath, resWidth, resHeight, 
                 System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss"));

        }
    }
   

}
