using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class FPSCounter : MonoBehaviour
{
    float               m_fFPS = 0.0f;
    System.DateTime     m_lastFrame;

    #region Properties

    public float FPS => m_fFPS;

    #endregion

    private void Start()
    {
        m_lastFrame = System.DateTime.Now;
    }

    void Update()
    {
        System.DateTime frameTime = System.DateTime.Now;
        float fTick = (float)frameTime.Subtract(m_lastFrame).TotalSeconds;
        if (fTick > float.Epsilon)
        {
            float fTarget = 1.0f / fTick;
            m_fFPS += (fTarget - m_fFPS) * Time.deltaTime;
        }
        m_lastFrame = frameTime;
    }
}
