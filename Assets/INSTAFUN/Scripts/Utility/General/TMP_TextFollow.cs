using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TMP_TextFollow : MonoBehaviour
{
    [SerializeField] TMP_Text followText;

    TMP_Text m_text;

    private void Start()
    {
        m_text = GetComponent<TMP_Text>();
        m_currentStr = "";
    }

    string m_currentStr;
    private void Update()
    {
        if (m_currentStr != followText.text)
        {
            m_currentStr = followText.text;
            m_text.text = m_currentStr;
        }
    }
}