using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogSpin : MonoBehaviour
{
    Rigidbody2D m_rigidbody;
    void Start()
    {
        m_rigidbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        m_rigidbody.angularVelocity = -m_rigidbody.velocity.x*15;
    }
}
