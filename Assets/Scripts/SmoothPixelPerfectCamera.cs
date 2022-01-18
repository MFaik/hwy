using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SmoothPixelPerfectCamera : MonoBehaviour
{
    [SerializeField] float PixelSize = 8;
    [SerializeField] float DefaultOrthogrophicSize = 9; 
    [SerializeField] Vector2 DefaultRenderTextureSize = new Vector2(256, 144);

    Camera m_mainCamera, m_camera;
    RenderTexture m_renderTexture;

    [SerializeField] Canvas PixelCanvas;
    RectTransform m_pixelCanvasRect;
    RawImage m_pixelRawImage;
    
    void Start() {
        m_mainCamera = Camera.main;
        m_camera = GetComponent<Camera>();
    
        m_pixelCanvasRect = PixelCanvas.GetComponent<RectTransform>();
        m_pixelRawImage = PixelCanvas.GetComponentInChildren<RawImage>();
            
        SetRenderTexture(DefaultRenderTextureSize);
    }

    void Update() {
        if(m_camera.orthographicSize != m_mainCamera.orthographicSize+1){
            m_camera.orthographicSize = m_mainCamera.orthographicSize+1;

            m_pixelCanvasRect.localScale = Vector2.one*(m_camera.orthographicSize/DefaultOrthogrophicSize);

            SetRenderTexture(DefaultRenderTextureSize * (m_camera.orthographicSize/DefaultOrthogrophicSize));
        }

        float x = m_mainCamera.transform.position.x * PixelSize;
        float y = m_mainCamera.transform.position.y * PixelSize;
        float roundX = (int)(x+.5f);
        float roundY = (int)(y+.5f);
        transform.position = new Vector3(roundX, roundY, -PixelSize) / PixelSize;

        m_pixelCanvasRect.localPosition = new Vector3((roundX - x)  / PixelSize ,(roundY - y)  / PixelSize, 10);
    }

    void SetRenderTexture(Vector2 renderTextureSize) {
        if(!m_pixelRawImage){
            Debug.LogError("No Renderer");
            return;
        }
        if(m_renderTexture){
            m_renderTexture.Release();
        }
        m_renderTexture = new RenderTexture((int)renderTextureSize.x, (int)renderTextureSize.y, 16);
        m_renderTexture.filterMode = FilterMode.Point;
        m_camera.targetTexture = m_renderTexture;
        m_pixelRawImage.texture = m_renderTexture;      
    }
}
