using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnapCam : MonoBehaviour
{

    protected const string MEDIA_STORE_IMAGE_MEDIA = "android.provider.MediaStore$Images$Media";
    protected static AndroidJavaObject m_Activity;

    protected static string SaveImageToGallery(Texture2D a_Texture, string a_Title, string a_Description)
    {
        using (AndroidJavaClass mediaClass = new AndroidJavaClass(MEDIA_STORE_IMAGE_MEDIA))
        {
            using (AndroidJavaObject contentResolver = Activity.Call<AndroidJavaObject>("getContentResolver"))
            {
                AndroidJavaObject image = Texture2DToAndroidBitmap(a_Texture);
                return mediaClass.CallStatic<string>("insertImage", contentResolver, image, a_Title, a_Description);
            }
        }
    }

    protected static AndroidJavaObject Texture2DToAndroidBitmap(Texture2D a_Texture)
    {
        byte[] encodedTexture = a_Texture.EncodeToPNG();
        using (AndroidJavaClass bitmapFactory = new AndroidJavaClass("android.graphics.BitmapFactory"))
        {
            return bitmapFactory.CallStatic<AndroidJavaObject>("decodeByteArray", encodedTexture, 0, encodedTexture.Length);
        }
    }

    public static AndroidJavaObject Activity
    {
        get
        {
            if (m_Activity == null)
            {
                var unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
                m_Activity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
            }
            return m_Activity;
        }
    }

    public void CaptureScreenshot()
    {
        StartCoroutine(CaptureScreenshotCoroutine(Screen.width, Screen.height));
    }

    private IEnumerator CaptureScreenshotCoroutine(int width, int height)
    {
        yield return new WaitForEndOfFrame();
        Texture2D tex = new Texture2D(width, height);
        tex.ReadPixels(new Rect(0, 0, width, height), 0, 0);
        tex.Apply();

        yield return tex;
        string path = "";
        Debug.Log("Picture has been saved at:\n" + path);
    }


}
