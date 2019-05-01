/// <summary>
/// By SwanDEV 2017
/// </summary>

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System;

public class ScreenshotHelper : MonoBehaviour
{
	public UnityEvent m_MainOnCaptured;

    /// <summary>
	/// Deprecated : In the previous version this parameter is for the iCapture method only. Deprecates this parameter to prevent confuse.
	/// Instead of using this parameter, you can input the capture size in the iCapture method directly.
    /// </summary>
	public Vector2 m_CaptureSize = new Vector2(512, 512);

	private bool _isOnCapture = false;
	private Texture2D _texture2D = null;
	private RenderTexture _renderTexture = null;
	public Text m_DebugText;

    private static ScreenshotHelper _instance = null;
	public static ScreenshotHelper Instance
	{
		get{
			if(_instance == null)
			{
				_instance = new GameObject("[ScreenshotHelper]").AddComponent<ScreenshotHelper>();
			}
			return _instance;
		}
	}

    /// <summary>
    /// Clear the instance of ScreenshotHelper:
    /// Destroy the stored textures, remove callbacks, remove script from camera.
    /// (Clear when no longer need the captured texture)
    /// </summary>
    public void Clear(bool clearCallback = true)
	{
        if (clearCallback)
        {
            if (m_MainOnCaptured != null)
            {
                m_MainOnCaptured.RemoveAllListeners();
                m_MainOnCaptured = null;
            }
        }

        if (_texture2D != null)
        {
            Texture2D.Destroy(_texture2D);
        }

        if (_renderTexture != null)
        {
            RenderTexture.Destroy(_renderTexture);
        }

        // Remove CameraOnRender script from camera
		UnRegisterAllRenderCameras();

		//Destroy(this.gameObject);
	}

	private void Awake()
	{
		if(_instance == null) _instance = this;
	}

	private void _InitMainOnCaptured()
	{
		if(m_MainOnCaptured == null) m_MainOnCaptured = new UnityEvent();
		m_MainOnCaptured.RemoveAllListeners();
	}

	/// <summary>
	/// Set the main onCaptured callback for receiving all images from all capture methods.
	/// </summary>
	/// <param name="mainOnCaptured">The callback to be fired at each capture.</param>
	public void SetMainOnCapturedCallback(Action mainOnCaptured)
	{
		_InitMainOnCaptured();
		m_MainOnCaptured.AddListener(delegate {
			mainOnCaptured();
		});
	}

	/// <summary>
	/// Set the main onCaptured callback for receiving all images from all capture methods. Return the captured images as Texture2D.
	/// </summary>
	/// <param name="mainOnCaptured">The callback to be fired at each capture, return a Texture2D.</param>
	public void SetMainOnCapturedCallback(Action<Texture2D> mainOnCaptured)
	{
		_InitMainOnCaptured();
		m_MainOnCaptured.AddListener(delegate {
			mainOnCaptured(_texture2D);
		});
	}

	/// <summary>
	/// Set the main onCaptured callback for receiving all images from all capture methods. Return the captured images as Sprite.
	/// </summary>
	/// <param name="mainOnCaptured">The callback to be fired at each capture, return a Sprite.</param>
	public void SetMainOnCapturedCallback(Action<Sprite> mainOnCaptured)
	{
		_InitMainOnCaptured();
		m_MainOnCaptured.AddListener(delegate {
			mainOnCaptured(GetCurrentSprite());
		});
	}

	/// <summary>
	/// Set the main onCaptured callback for receiving all images from all capture methods. Return the captured images as RenderTexture.
	/// </summary>
	/// <param name="mainOnCaptured">The callback to be fired at each capture, return a RenderTexture.</param>
	public void SetMainOnCapturedCallback(Action<RenderTexture> mainOnCaptured)
	{
		_InitMainOnCaptured();
		m_MainOnCaptured.AddListener(delegate {
			mainOnCaptured(_renderTexture);
		});
	}

	/// <summary>
	/// Capture the whole screen.
	/// </summary>
	/// <param name="onCaptured">On captured.</param>
	public void CaptureScreen(Action<Texture2D> onCaptured = null)
	{
		StartCoroutine(_TakeFullscreen(onCaptured));
	}

	/// <summary>
	/// Capture a portion of the screen at a specific screen position.
	/// </summary>
	/// <param name="screenPosition">Screen position.</param>
	/// <param name="imageSize">The maximum image size for the screenshot.</param>
	/// <param name="onCaptured">On captured action, return the captured texture2D</param>
	public void Capture(Vector2 screenPosition, Vector2 imageSize, Action<Texture2D> onCaptured = null)
	{
		if(_isOnCapture){
			Debug.LogWarning("Screenshot being captured, please wait for at least 1 frame for starting another capture!");
			return;
		}
		_isOnCapture = true;
        
		//UpdateDebugText("Capture screenPosition: " + screenPosition + " | imageSize: " + imageSize);
		Rect rect = new Rect(screenPosition, imageSize);
		StartCoroutine(_ReadPixelWithRect(rect, onCaptured, true));
	}

    private Rect _ConstraintRectWithScreen(Rect rect)
    {
        int ScreenWidth = Screen.width;
        int ScreenHeight = Screen.height;

        //size correction
        if (rect.width > ScreenWidth) rect.size = new Vector2(ScreenWidth, rect.height);
        if (rect.height > ScreenHeight) rect.size = new Vector2(rect.width, ScreenHeight);

        Debug.Log("Capture() imageSize: " + rect.size.ToString() + " Screen W: " + ScreenWidth + " Screen H: " + ScreenHeight);

        //position correction
        if (rect.x + rect.width / 2 > ScreenWidth)
            rect.position = new Vector2(rect.x - (rect.x + rect.width / 2 - ScreenWidth), rect.y);
        if (rect.x - rect.width / 2 < 0)
            rect.position = new Vector2(rect.x + (rect.width / 2 - rect.x), rect.y);
        if (rect.y + rect.height / 2 > ScreenHeight)
            rect.position = new Vector2(rect.x, rect.y - (rect.y + rect.height / 2 - ScreenHeight));
        if (rect.y - rect.height / 2 < 0)
            rect.position = new Vector2(rect.x, rect.y + (rect.height / 2 - rect.y));

        UpdateDebugText("Capture position: " + rect.position + " | imageSize: " + rect.size);
        return rect;
    }

    /// <summary>
    /// Capture image with the view of the target camera. Return a Texture2D.
    /// </summary>
    /// <param name="camera">Target Camera.</param>
    /// <param name="onCaptured">On captured callback, return the captured Texture2D.</param>
	/// <param name="targetWidth">The width for the texture.</param>
	/// <param name="targetHeight">The height for the texture.</param>
    public void CaptureWithCamera(Camera camera, Action<Texture2D> onCaptured = null, int targetWidth = 0, int targetHeight = 0)
	{
		UpdateDebugText(camera.name + " rect: " + camera.pixelWidth + " x " + camera.pixelHeight);

		RegisterRenderCamera(camera);
		CameraOnRender camOnRender = camera.gameObject.GetComponent<CameraOnRender>();

		if(camOnRender != null)
		{
			camOnRender.SetOnCaptureCallback((Texture2D tex)=>{
				_texture2D = tex;
				if(m_MainOnCaptured != null) m_MainOnCaptured.Invoke();
				if(onCaptured != null) onCaptured(_texture2D);
			}, targetWidth, targetHeight);
		}
		else
		{
			Debug.LogWarning("Require this camera to be registered with method RegisterCaptureCamera!");
		}
	}

    /// <summary>
    /// Capture image with the view of the target camera. Return a Texture2D.
    /// </summary>
    /// <param name="camera">Target Camera.</param>
    /// <param name="scale">Apply this scale to capture image. (Scale the image size down to the minimum of 0.1X and up to 4X)</param>
    /// <param name="onCaptured">On captured callback, return the captured Texture2D.</param>
    public void CaptureWithCamera(Camera camera, float scale, Action<Texture2D> onCaptured = null)
    {
        UpdateDebugText(camera.name + " rect: " + camera.pixelWidth + " x " + camera.pixelHeight);

        RegisterRenderCamera(camera);
        CameraOnRender camOnRender = camera.gameObject.GetComponent<CameraOnRender>();

        if (camOnRender != null)
        {
            camOnRender.SetOnCaptureCallback((Texture2D tex) => {
                _texture2D = tex;
                if (m_MainOnCaptured != null) m_MainOnCaptured.Invoke();
                if (onCaptured != null) onCaptured(_texture2D);
            }, scale);
        }
        else
        {
            Debug.LogWarning("Require this camera to be registered with method RegisterCaptureCamera!");
        }
    }

    /// <summary>
    /// Capture image with the view of the target camera. Return a RenderTexture.
    /// </summary>
    /// <param name="camera">Target Camera.</param>
    /// <param name="onCaptured">On captured callback, return the captured RenderTexture.</param>
	/// <param name="targetWidth">The width for the texture.</param>
	/// <param name="targetHeight">The height for the texture.</param>
    /// <param name="blitToNewTexture">Create and return a new RenderTexture, so that will not be removed by the Clear method.</param>
    public void CaptureRenderTextureWithCamera(Camera camera, Action<RenderTexture> onCaptured = null, int targetWidth = 0, int targetHeight = 0, bool blitToNewTexture = true)
	{
		UpdateDebugText(camera.name + " rect: " + camera.pixelWidth + " x " + camera.pixelHeight);

		RegisterRenderCamera(camera);
		CameraOnRender camOnRender = camera.gameObject.GetComponent<CameraOnRender>();

		if(camOnRender != null)
		{
			camOnRender.SetOnCaptureCallback((RenderTexture rTex)=>{
				_renderTexture = rTex;
				if(m_MainOnCaptured != null) m_MainOnCaptured.Invoke();
				if(onCaptured != null) onCaptured(_renderTexture);
			}, targetWidth, targetHeight, blitToNewTexture);
		}
		else
		{
			Debug.LogWarning("Require this camera to be registered with method RegisterCaptureCamera!");
		}
	}

    /// <summary>
    /// Capture image with the view of the target camera. Return a RenderTexture.
    /// </summary>
    /// <param name="camera">Target Camera.</param>
    /// <param name="scale">Apply this scale to capture image. (Scale the image size down to the minimum of 0.1X and up to 4X)</param>
    /// <param name="onCaptured">On captured callback, return the captured RenderTexture.</param>
    /// <param name="blitToNewTexture">Create and return a new RenderTexture, so that will not be removed by the Clear method.</param>
    public void CaptureRenderTextureWithCamera(Camera camera, float scale, Action<RenderTexture> onCaptured = null, bool blitToNewTexture = true)
    {
        UpdateDebugText(camera.name + " rect: " + camera.pixelWidth + " x " + camera.pixelHeight);

        RegisterRenderCamera(camera);
        CameraOnRender camOnRender = camera.gameObject.GetComponent<CameraOnRender>();

        if (camOnRender != null)
        {
            camOnRender.SetOnCaptureCallback((RenderTexture rTex) => {
                _renderTexture = rTex;
                if (m_MainOnCaptured != null) m_MainOnCaptured.Invoke();
                if (onCaptured != null) onCaptured(_renderTexture);
            }, scale, blitToNewTexture);
        }
        else
        {
            Debug.LogWarning("Require this camera to be registered with method RegisterCaptureCamera!");
        }
    }

    /// <summary>
    /// Get the currently stored Texture2D (since the last capture).
    /// If you did not take any screenshot before, this will return a null.
    /// </summary>
    /// <returns>The currently stored Texture2D.</returns>
    public Texture2D GetCurrentTexture()
	{
		return _texture2D;
	}

	/// <summary>
	/// Return the sprite that converts from the current stored texture2D
	/// If you did not take any screenshot before, this will return a null.
	/// </summary>
	/// <returns>The sprite converts from _texture2D.</returns>
	public Sprite GetCurrentSprite()
	{
		return ToSprite(GetCurrentTexture());
	}

	/// <summary>
	/// Get the currently stored RenderTexture (since the last capture, that captured by Camera only).
	/// If you did not take any screenshot before, this will return a null.
	/// </summary>
	/// <returns>The currently stored RenderTexture.</returns>
	public RenderTexture GetCurrentRenderTexture()
	{
		return _renderTexture;
	}

	private void _ProceedReadPixels(Rect targetRect, Action<Texture2D> onCaptured)
	{
		//size correction for target rect
		if(targetRect.width > Screen.width) targetRect.width = Screen.width;
		if(targetRect.height > Screen.height) targetRect.height = Screen.height;

		_texture2D = new Texture2D((int)targetRect.width, (int)targetRect.height, TextureFormat.RGB24, false);
		Rect rect = new Rect(targetRect.position.x-targetRect.width/2, targetRect.position.y-targetRect.height/2, targetRect.width, targetRect.height);
		_texture2D.ReadPixels(rect, 0, 0);
		_texture2D.Apply();
		if(m_MainOnCaptured != null) m_MainOnCaptured.Invoke();
		if(onCaptured != null) onCaptured(_texture2D);

		_isOnCapture = false;

		UpdateDebugText("Capture screenPosition: (" + targetRect.position.x + ", " + targetRect.position.y + ") | imageSize: (" + targetRect.width + ", " + targetRect.height + ")");
	}

	private IEnumerator _TakeFullscreen(Action<Texture2D> onCaptured)
	{
		//ensure to Read Pixels inside drawing frame
		yield return new WaitForEndOfFrame();
		Rect targetRect = new Rect(Screen.width/2, Screen.height/2, Screen.width, Screen.height);
		_ProceedReadPixels(targetRect, onCaptured);
	}

	private IEnumerator _ReadPixelWithRect(Rect targetRect, Action<Texture2D> onCaptured, bool constraintTargetRectWithScreen = false)
	{
		//ensure to Read Pixels inside drawing frame
		yield return new WaitForEndOfFrame();
        if (constraintTargetRectWithScreen) targetRect = _ConstraintRectWithScreen(targetRect);
        _ProceedReadPixels(targetRect, onCaptured);
	}

	/// <summary>
	/// Attach a CameraOnRender script on the camera to capture image from camera.
	/// </summary>
	/// <param name="camera">Target Camera.</param>
	public void RegisterRenderCamera(Camera camera)
	{
		if(camera != null && camera.gameObject.GetComponent<CameraOnRender>() == null)
		{
			camera.gameObject.AddComponent<CameraOnRender>();
		}
	}

	/// <summary>
	/// Clear the instance of CameraOnRender and remove the script.
	/// </summary>
	/// <param name="camera">Target Camera.</param>
	public void UnRegisterRenderCamera(Camera camera)
	{
		if(camera != null && camera.gameObject.GetComponent<CameraOnRender>() != null)
		{
			camera.gameObject.GetComponent<CameraOnRender>().Clear();
		}
	}

	/// <summary>
	/// Clear the instance of CameraOnRender on all cameras, and remove the script.
	/// </summary>
	public void UnRegisterAllRenderCameras()
	{
		Camera[] cameras = Camera.allCameras;
		if(cameras != null)
		{
			foreach(Camera cam in cameras)
			{
				UnRegisterRenderCamera(cam);
			}
		}
	}


    #region ----- Static Methods -----
	/// <summary>
	/// Get the currently stored Texture2D (since the last capture).
	/// If you did not take any screenshot before, this will return a null.
	/// </summary>
	/// <returns>The currently stored Texture2D.</returns>
	public static Texture2D CurrentTexture
	{
		get{
			return Instance.GetCurrentTexture();
		}
	}

	/// <summary>
	/// Return the sprite that converts from the current texture2D
	/// If you did not take any screenshot before, this will return a null.
	/// </summary>
	/// <returns>The sprite converts from _texture2D.</returns>
	public static Sprite CurrentSprite
	{
		get{
			return Instance.GetCurrentSprite();
		}
	}

	/// <summary>
	/// Get the currently stored RenderTexture (since the last capture, that captured by Camera only).
	/// If you did not take any screenshot before, this will return a null.
	/// </summary>
	/// <returns>The currently stored RenderTexture.</returns>
	public static RenderTexture CurrentRenderTexture
	{
		get{
			return Instance.GetCurrentRenderTexture();
		}
	}

    /// <summary>
	/// Deprecated : In the previous version this parameter is for the iCapture method only. Deprecates this parameter to prevent confuse.
	/// Instead of using this parameter, you can input the capture size in the iCapture method directly.
    /// </summary>
	public static Vector2 CurrentCaptureSize
	{
		get{
			return Instance.m_CaptureSize;
		}
	}

    /// <summary>
	/// Deprecated : In the previous version this method is for setting the Capture Size for the iCapture method only. Deprecates this method to prevent confuse.
	/// Instead of using this method, you can input the capture size in the iCapture method directly. 
    /// </summary>
	public static void iSetCaptureSize(Vector2 captureSize)
	{
		Instance.m_CaptureSize = captureSize;
	}

	/// <summary>
	/// Set the main onCaptured callback for receiving all images from all capture methods.
	/// </summary>
	/// <param name="mainOnCaptured">The callback to be fired at each capture.</param>
	public static void iSetMainOnCapturedCallback(Action mainOnCaptured)
	{
		Instance.SetMainOnCapturedCallback(mainOnCaptured);
	}

	/// <summary>
	/// Set the main onCaptured callback for receiving all images from all capture methods. Return the captured images as Texture2D.
	/// </summary>
	/// <param name="mainOnCaptured">The callback to be fired at each capture, return a Texture2D.</param>
	public static void iSetMainOnCapturedCallback(Action<Texture2D> mainOnCaptured)
	{
		Instance.SetMainOnCapturedCallback(mainOnCaptured);
	}

	/// <summary>
	/// Set the main onCaptured callback for receiving all images from all capture methods. Return the captured images as Sprite.
	/// </summary>
	/// <param name="mainOnCaptured">The callback to be fired at each capture, return a Sprite.</param>
	public static void iSetMainOnCapturedCallback(Action<Sprite> mainOnCaptured)
	{
		Instance.SetMainOnCapturedCallback(mainOnCaptured);
	}

	/// <summary>
	/// Set the main onCaptured callback for receiving all images from all capture methods. Return the captured images as RenderTexture.
	/// </summary>
	/// <param name="mainOnCaptured">The callback to be fired at each capture, return a RenderTexture.</param>
	public static void iSetMainOnCapturedCallback(Action<RenderTexture> mainOnCaptured)
	{
		Instance.SetMainOnCapturedCallback(mainOnCaptured);
	}

	/// <summary>
	/// Capture the whole screen.
	/// </summary>
	/// <param name="onCaptured">On captured.</param>
	public static void iCaptureScreen(Action<Texture2D> onCaptured = null)
	{
		Instance.CaptureScreen(onCaptured);
	}

	/// <summary>
	/// Capture a portion of the screen at a specific screen position.
	/// </summary>
	/// <param name="screenPosition">Screen position.</param>
	/// <param name="imageSize">The maximum image size for the screenshot.</param>
	/// <param name="onCaptured">On captured action, return the captured texture2D</param>
	public static void iCapture(Vector2 screenPosition, Vector2 imageSize, Action<Texture2D> onCaptured = null)
	{
		Instance.Capture(screenPosition, imageSize, onCaptured);
	}

    /// <summary>
    /// Capture image with the view of the target camera. Return a Texture2D.
    /// </summary>
    /// <param name="camera">Target Camera.</param>
    /// <param name="onCaptured">On captured action, return the captured Texture2D</param>
    /// <param name="targetWidth">Constraint the captured image into this size.</param>
    /// <param name="targetHeight">Constraint the captured image into this size.</param>
    public static void iCaptureWithCamera(Camera camera, Action<Texture2D> onCaptured = null, int targetWidth = 0, int targetHeight = 0)
    {
        Instance.CaptureWithCamera(camera, onCaptured, targetWidth, targetHeight);
    }

    /// <summary>
    /// Capture image with the view of the target camera. Return a Texture2D.
    /// </summary>
    /// <param name="camera">Target Camera.</param>
    /// <param name="scale">Apply this scale to capture image.</param>
    /// <param name="onCaptured">On captured action, return the captured Texture2D</param>
    public static void iCaptureWithCamera(Camera camera, float scale, Action<Texture2D> onCaptured = null)
    {
        Instance.CaptureWithCamera(camera, scale, onCaptured);
    }

    /// <summary>
    /// Capture image with the view of the target camera. Return a RenderTexture.
    /// </summary>
    /// <param name="camera">Target Camera.</param>
    /// <param name="onCaptured">On captured callback, return the captured RenderTexture.</param>
	/// <param name="targetWidth">The width for the texture.</param>
	/// <param name="targetHeight">The height for the texture.</param>
    /// <param name="blitToNewTexture">Create and return a new RenderTexture, so that will not be removed by the Clear method.</param>
    public static void iCaptureRenderTextureWithCamera(Camera camera, Action<RenderTexture> onCaptured = null, int targetWidth = 0, int targetHeight = 0, bool blitToNewTexture = true)
    {
        Instance.CaptureRenderTextureWithCamera(camera, onCaptured, targetWidth, targetHeight, blitToNewTexture);
    }

    /// <summary>
    /// Capture image with the view of the target camera. Return a RenderTexture.
    /// </summary>
    /// <param name="camera">Target Camera.</param>
    /// <param name="scale">Apply this scale to capture image. (Scale the image size down to the minimum of 0.1X and up to 4X)</param>
    /// <param name="onCaptured">On captured callback, return the captured RenderTexture.</param>
    /// <param name="blitToNewTexture">Create and return a new RenderTexture, so that will not be removed by the Clear method.</param>
    public static void iCaptureRenderTextureWithCamera(Camera camera, float scale, Action<RenderTexture> onCaptured = null, bool blitToNewTexture = true)
    {
        Instance.CaptureRenderTextureWithCamera(camera, scale, onCaptured, blitToNewTexture);
    }

    /// <summary>
    /// Attach a CameraOnRender script on the camera to capture image from camera.
    /// </summary>
    /// <param name="camera">Target Camera.</param>
    public static void iRegisterRenderCamera(Camera camera)
	{
		Instance.RegisterRenderCamera(camera);
	}

	/// <summary>
	/// Clear the instance of CameraOnRender and remove the script.
	/// </summary>
	/// <param name="camera">Target Camera.</param>
	public static void iUnRegisterRenderCamera(Camera camera)
	{
		Instance.UnRegisterRenderCamera(camera);
	}

	/// <summary>
	/// Clear the instance of CameraOnRender on all cameras, and remove the script.
	/// </summary>
	public static void iUnRegisterAllRenderCameras()
	{
		Instance.UnRegisterAllRenderCameras();
	}

	/// <summary>
	/// Clear the instance of ScreenshotHelper:
	/// Destroy the stored textures, remove callbacks, remove script from camera.
    /// (Clear when no longer need the captured texture)
	/// </summary>
	public static void iClear(bool clearCallback = true)
	{
        Instance.Clear(clearCallback);
	}
	#endregion


	#region ----- Others -----
	public void UpdateDebugText(string text)
	{
		if(m_DebugText != null)
		{
			Debug.Log(text);
			m_DebugText.text = text;
		}
	}

	public static Sprite ToSprite(Texture2D texture)
	{
		if(texture == null) return null;

		Vector2 pivot = new Vector2(0.5f, 0.5f);
		float pixelPerUnit = 100;
		return Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), pivot, pixelPerUnit);
	}
	#endregion
    
}
