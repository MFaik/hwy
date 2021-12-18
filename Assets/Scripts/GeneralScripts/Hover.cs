using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Hover : MonoBehaviour
{
    Vector2 m_startPosition;

    public bool IsHovering{ get; private set;} = false;

    [SerializeField] Vector2 HoverDistance = new Vector2(0,0.1f);
    [SerializeField] float LoopDuration = 1;
    [SerializeField] float LoopOffset = 0;
    
    [SerializeField] Vector2 StartPositionOffset = new Vector2(0,0);
    [SerializeField] float StartOffsetDuration = 0;

    float m_currentDegree;

    void Start() {
        m_startPosition = StartPositionOffset+(Vector2)transform.position;
        m_currentDegree = (LoopOffset/LoopDuration)*Mathf.PI;
        transform.DOMove(m_startPosition,StartOffsetDuration).SetEase(Ease.Linear).OnComplete(()=>{IsHovering = true;});
    }

    // Update is called once per frame
    void Update() {
        if(!IsHovering)
            return;
        m_currentDegree += (Time.deltaTime/LoopDuration)*Mathf.PI;
        transform.position = m_startPosition + HoverDistance*Mathf.Sin(m_currentDegree);
    }
}
