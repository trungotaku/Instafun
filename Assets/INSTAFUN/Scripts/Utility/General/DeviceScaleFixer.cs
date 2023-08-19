using System.Collections;
using System.Collections.Generic;
using Constants;
using UnityEngine;
using UnityEngine.UI;

public class DeviceScaleFixer : MonoBehaviour
{
    [SerializeField] bool m_flip;

    CanvasScaler m_scaler;
    float m_deviceRateDefault;
    RectTransform m_canvasRect;
    private void Start()
    {
        m_scaler = GetComponent<CanvasScaler>();
        m_canvasRect = GetComponent<RectTransform>();
        m_deviceRateDefault = m_scaler.referenceResolution.x / m_scaler.referenceResolution.y;

        FixedUI();
    }

    void FixedUI()
    {
        int delta_matchWidthOrHeight = m_flip ? 1 : 0;
        // fixed UI for any tablet ratio
        if ((float)(m_canvasRect.sizeDelta.x / m_canvasRect.sizeDelta.y) > m_deviceRateDefault)
        {
            m_canvasRect.GetComponent<CanvasScaler>().matchWidthOrHeight = 1 - delta_matchWidthOrHeight;
        }
        else
        {
            //Debug.LogError("moblie");
            m_canvasRect.GetComponent<CanvasScaler>().matchWidthOrHeight = 0 + delta_matchWidthOrHeight;
        }
    }

    private void Update()
    {
#if UNITY_EDITOR
        FixedUI();
#endif
    }
}