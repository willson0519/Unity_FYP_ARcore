/// <summary>
/// By SwanDEV 2017
/// </summary>

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CameraOnRender : MonoBehaviour
{
	[HideInInspector] public RenderTexture m_RenderTexture;

	[HideInInspector] public bool m_ToCapture = true;
	private Action<Texture2D> _onCaptureCallback;
	private Action<RenderTexture> _onCaptureCallbackRTex;

	private bool _blitToNewTexture = true;

	private int _targetWidth = 0;
	private int _targetHeight = 0;
    private float _scale = 1f;

	void Start()
	{
		m_RenderTexture = new RenderTexture(4, 4, 24);
	}

	public void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		Graphics.Blit(source, destination);
		if(!m_ToCapture)
		{
			return;
		}
		m_ToCapture = false;

		if(_targetWidth > 0 && _targetHeight > 0)
		{
			m_RenderTexture = new RenderTexture(_targetWidth, _targetHeight, 24);
		}
		else if(m_RenderTexture.width != source.width * _scale || m_RenderTexture.height != source.height * _scale)
		{
			m_RenderTexture = new RenderTexture((int)(source.width * _scale), (int)(source.height * _scale), 24);
		}
		Graphics.Blit(source, m_RenderTexture);

		if(_onCaptureCallback != null)
		{
			_onCaptureCallback(GetLastTexture2D());
			_onCaptureCallback = null;
		}

		if(_onCaptureCallbackRTex != null)
		{
			if(_blitToNewTexture)
			{
				RenderTexture rTex = new RenderTexture(m_RenderTexture.width, m_RenderTexture.height, 24);
				Graphics.Blit(GetLastRenderTexture(), rTex);
				_onCaptureCallbackRTex(rTex);
			}
			else
			{
				_onCaptureCallbackRTex(GetLastRenderTexture());
			}
			_onCaptureCallbackRTex = null;
		}
	}

    public void SetOnCaptureCallback(Action<Texture2D> onCaptured, float scale)
    {
        _onCaptureCallback = onCaptured;
        _scale = Mathf.Clamp(scale, 0.1f, 4f);
        _targetWidth = 0;
        _targetHeight = 0;
        m_ToCapture = true;
    }

    public void SetOnCaptureCallback(Action<Texture2D> onCaptured, int targetWidth = 0, int targetHeight = 0)
	{
		_onCaptureCallback = onCaptured;
        _scale = 1f;
        _targetWidth = targetWidth;
		_targetHeight = targetHeight;
		m_ToCapture = true;
	}

    public void SetOnCaptureCallback(Action<RenderTexture> onCaptured, float scale, bool blitToNewTexture = true)
    {
        _onCaptureCallbackRTex = onCaptured;
        _blitToNewTexture = blitToNewTexture;
        _scale = Mathf.Clamp(scale, 0.1f, 4f);
        _targetWidth = 0;
        _targetHeight = 0;
        m_ToCapture = true;
    }

    public void SetOnCaptureCallback(Action<RenderTexture> onCaptured, int targetWidth = 0, int targetHeight = 0, bool blitToNewTexture = true)
	{
		_onCaptureCallbackRTex = onCaptured;
		_blitToNewTexture = blitToNewTexture;
        _scale = 1f;
        _targetWidth = targetWidth;
		_targetHeight = targetHeight;
		m_ToCapture = true;
	}

	public Texture2D GetLastTexture2D()
	{
		return _RenderTextureToTexture2D(m_RenderTexture);
	}

	public RenderTexture GetLastRenderTexture()
	{
		return m_RenderTexture;
	}

	private Texture2D _RenderTextureToTexture2D(RenderTexture source)
	{
		RenderTexture.active = source;
		Texture2D tex = new Texture2D(source.width, source.height, TextureFormat.RGB24, false);
		tex.ReadPixels(new Rect(0, 0, source.width, source.height), 0, 0);
		tex.Apply();
		RenderTexture.active = null;
		return tex;
	}

	public void Clear()
	{
		_onCaptureCallback = null;

		if(m_RenderTexture != null)
		{
			RenderTexture.Destroy(m_RenderTexture);
		}

		Destroy(this);
	}
}
