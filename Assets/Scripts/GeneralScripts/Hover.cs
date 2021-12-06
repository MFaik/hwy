using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hover : MonoBehaviour
{
    Vector2 m_startPosition;
 
    [SerializeField] Vector2 HoverDistance = new Vector2(0,0.1f);
    [SerializeField] float LoopDuration = 1;
    [SerializeField] float LoopOffset = 0;

    float m_currentDegree;
    // Start is called before the first frame update
    void Start()
    {
        m_startPosition = transform.position;
        m_currentDegree = (LoopOffset/LoopDuration)*Mathf.PI;
    }

    // Update is called once per frame
    void Update()
    {
        m_currentDegree += (Time.deltaTime/LoopDuration)*Mathf.PI;
        transform.position = m_startPosition + HoverDistance*Mathf.Sin(m_currentDegree);
    }
}
