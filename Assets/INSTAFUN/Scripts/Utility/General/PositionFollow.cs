using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionFollow : MonoBehaviour
{
    [SerializeField] Transform m_followPos;

    bool m_trigger = false;
    void Start()
    {
        m_trigger = false;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (!m_trigger)
        {
            m_trigger = true;
            this.transform.position = m_followPos.position;
        }
    }
}
