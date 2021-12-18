using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DiffrentResolutionChildCamera : MonoBehaviour
{
    [SerializeField] Vector2 RenderTextureSize = new Vector2(192, 108);

    Camera m_parentCamera, m_camera;
    RenderTexture m_renderTexture;

    RawImage m_particleRenderer;
    
    void Start() {
        m_parentCamera = transform.parent.GetComponent<Camera>();
        m_camera = GetComponent<Camera>();
    
        m_particleRenderer = GameObject.FindGameObjectWithTag("ParticleRenderer").GetComponent<RawImage>();

        SetRenderTexture();
    }

    void Update() {
        if(m_camera.orthographicSize != m_parentCamera.orthographicSize){
            RenderTextureSize *= m_parentCamera.orthographicSize/m_camera.orthographicSize;
            SetRenderTexture();
            
            m_camera.orthographicSize = m_parentCamera.orthographicSize;
        }
    }

    void SetRenderTexture() {
        if(m_renderTexture)
            m_renderTexture.Release();
        m_renderTexture = new RenderTexture((int)RenderTextureSize.x, (int)RenderTextureSize.y, 16);
        m_renderTexture.filterMode = FilterMode.Point;
        m_camera.targetTexture = m_renderTexture;
        m_particleRenderer.texture = m_renderTexture;
    }
}
