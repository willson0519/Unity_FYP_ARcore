#if UNITY_EDITOR
using System.Collections;
using UnityEngine;
using UnityEditor;
using System.IO;

/// <summary>
/// On editor Screenshot Tool: capture images of your app from screen or camera in editor Play Mode.
/// </summary>
public class OnEditorScreenshot : MonoBehaviour
{
    [Header("[ HOW ] Attach this script on a GameObject in the scene,")]
    [Header("modify settings and start capture images.")]

    [Header("[ Save Settings ]")]
    public FilePathName.AppPath m_SaveDirectory = FilePathName.AppPath.PersistentDataPath;
    public SaveFormat m_SaveFormat = SaveFormat.PNG;
    public enum SaveFormat
    {
        JPG = 0,
        PNG,
    }
    public string m_OptionalSubFolder;

    [Header("[ Burst Mode Capture Settings ]")]
    [Tooltip("The number of images to capture")]
    [Range(2, 999)] public int m_BurstCount = 10;
    [Tooltip("The burst interval (in seconds)")]
    [Range(0.05f, 10f)] public float m_BurstInterval = 0.1f;

    [Space()]
    [Header("[ Screen Capture Settings ]")]
    [Tooltip("Capture fullscreen image. Ignore the screen position and size(width & height) values.")]
    public bool m_IsFullscreen = true;
    public Vector2 m_ScreenPosition = new Vector2(0, 0);
    public int m_Width = 360;
    public int m_Height = 360;

    [Space()]
    [Header("[ Camera Capture Settings ]")]
    [Tooltip("The camera for recording GIF. Drag camera on this variable or click the 'Find Camera' button to setup.")]
    public Camera m_SelectedCamera;
    public Camera[] m_AllCameras;
    [Range(0.1f, 4f)] public float m_ImageScale = 1.0f;
    private int _currCameraIndex = 0;

    /// <summary>
    /// 0: capture image from screen, 1: capture image from selected camera
    /// </summary>
    [HideInInspector] public int m_CaptureSourceIndex = 0;  

    [HideInInspector] public string m_BurstProgress = "0 / 0";
	[HideInInspector] public string m_BurstState = "Stopped";
	[HideInInspector] [TextArea(1, 2)] public string m_SavePath = "";


	public void FindCameras(OnEditorGifRecorderCustomEditor editorScript)
	{
		m_AllCameras = Camera.allCameras;
		editorScript.SetCameraOptions(m_AllCameras);

		if(m_AllCameras != null && m_AllCameras.Length > 0 && m_SelectedCamera == null)
		{
			m_SelectedCamera = m_AllCameras[0];
		}
	}

    public void SetCaptureSource(int index)
    {
        m_CaptureSourceIndex = index;
    }

	public void SetCamera(int index)
	{
		if(_currCameraIndex == index) return;
		_currCameraIndex = index;
		if(index < m_AllCameras.Length) m_SelectedCamera = m_AllCameras[index];
	}

    private FilePathName filePathName = new FilePathName();
	public void Capture()
	{
		if(!Application.isPlaying || !Application.isEditor)
		{
			Debug.LogWarning("This script is designed to work in the editor Play Mode.");
			return;
		}

        m_BurstState = "Stopped";

        if (m_CaptureSourceIndex == 0) // Capture from screen
        {
            if (m_IsFullscreen)
            {
                ScreenshotHelper.iCaptureScreen((texture) =>
                {
                    m_SavePath = filePathName.SaveTextureAs(texture, m_SaveDirectory, m_OptionalSubFolder, (m_SaveFormat == SaveFormat.JPG));
                    ScreenshotHelper.iClear();
                });
            }
            else
            {
                ScreenshotHelper.iCapture(m_ScreenPosition, new Vector2(m_Width, m_Height), (texture) =>
                {
                    m_SavePath = filePathName.SaveTextureAs(texture, m_SaveDirectory, m_OptionalSubFolder, (m_SaveFormat == SaveFormat.JPG));
                    ScreenshotHelper.iClear();
                });
            }
        }
        else // Capture from selected camera
        {
            ScreenshotHelper.iCaptureWithCamera(m_SelectedCamera, m_ImageScale, (texture) =>
            {
                m_SavePath = filePathName.SaveTextureAs(texture, m_SaveDirectory, m_OptionalSubFolder, (m_SaveFormat == SaveFormat.JPG));
                ScreenshotHelper.iClear();
            });
        }
	}

    public void BurstCapture()
    {
        _isBurstOnGoing = true;
        StartCoroutine(_BurstCapture());
    }

    public void StopBurstCapture()
    {
        _isBurstOnGoing = false;
    }

    private bool _isBurstOnGoing = false;
    private IEnumerator _BurstCapture()
    {
        for (int i = 0; i < m_BurstCount; i++)
        {
            if (!_isBurstOnGoing)
            {
                m_BurstState = "Stopped";
                m_BurstProgress = (i + 1) + " / " + m_BurstCount + " (Manually Stopped)";
                yield break;
            }
            Capture();
            m_BurstState = "On Going..";
            m_BurstProgress = (i + 1) + " / " + m_BurstCount;
            yield return new WaitForSeconds(m_BurstInterval);
        }
        m_BurstState = "Stopped";
    }

}


[CustomEditor(typeof(OnEditorScreenshot))]
public class OnEditorGifRecorderCustomEditor : Editor
{
	private static string[] cameraOptions = new string[]{};
	private static int cameraSelection = 0;

    private static string[] captureSources = new string[] {"From Screen", "From Camera"};
    private static int captureSourceSelection = 0;

    public override void OnInspectorGUI()
	{
		DrawDefaultInspector();

		OnEditorScreenshot capturer = (OnEditorScreenshot)target;

		cameraSelection = GUILayout.SelectionGrid(cameraSelection, cameraOptions, 2);
		capturer.SetCamera(cameraSelection);

        GUILayout.Label("Find all Cameras in the scene:");
		if(GUILayout.Button("Find Cameras"))
		{
			capturer.FindCameras(this);
		}

        GUILayout.Label("\n\nSelect Source: capture image(s) from " + (capturer.m_CaptureSourceIndex == 0 ? "Screen" : "Camera"));
        captureSourceSelection = GUILayout.SelectionGrid(captureSourceSelection, captureSources, 2);
        capturer.SetCaptureSource(captureSourceSelection);

        GUILayout.Label("[ Start Capture Image ]");    // \nStart capture image(s) from selected camera or from screen:
        if (GUILayout.Button("Capture (Single)"))
        {
            capturer.Capture();
        }

        GUILayout.Label(" Burst Mode ( Count: " + capturer.m_BurstCount + ", Interval: " + capturer.m_BurstInterval + "s )");
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Start Capture (Burst)"))
        {
            capturer.BurstCapture();
        }
        if (GUILayout.Button("Stop Capture (Burst)"))
        {
            // Stop the burst mode capture.
            capturer.StopBurstCapture();
        }
        GUILayout.EndHorizontal();

        GUILayout.Label("Burst Progress: " + capturer.m_BurstProgress
            + "\nBurst State: " + capturer.m_BurstState
			+ "\n\nLast File Name: " + Path.GetFileName(capturer.m_SavePath) + "\n");

		if(GUILayout.Button("View Image"))
		{
			if(string.IsNullOrEmpty(capturer.m_SavePath)) return;
			Application.OpenURL(new FilePathName().EnsureLocalPath(capturer.m_SavePath));
		}

		if(GUILayout.Button("Reveal In Folder"))
		{
			if(string.IsNullOrEmpty(capturer.m_SavePath)) return;
			string fileName = Path.GetFileName(capturer.m_SavePath);
			string directoryPath = capturer.m_SavePath.Remove(capturer.m_SavePath.IndexOf(fileName));
			Application.OpenURL(new FilePathName().EnsureLocalPath(directoryPath));
		}

		if(GUILayout.Button("Copy Image Path"))
		{
			if(string.IsNullOrEmpty(capturer.m_SavePath)) return;

			TextEditor te = null;
			te = new TextEditor();
			te.text = capturer.m_SavePath;
			te.SelectAll();
			te.Copy();
		}

	}

	public void SetCameraOptions(Camera[] cameras)
	{
        //IMBX.ImageCrop.CropRenderTextureToTexture2D();
		cameraSelection = 0;
		cameraOptions = new string[cameras.Length];
		for(int i=0; i<cameras.Length; i++)
		{
			cameraOptions[i] = cameras[i].name;
		}
	}
}

#endif