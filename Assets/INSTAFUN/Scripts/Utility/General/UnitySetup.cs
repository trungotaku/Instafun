using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitySetup : MonoBehaviour
{
    private void Awake()
    {
        // #if !(DEVELOPMENT_BUILD || UNITY_EDITOR)
        //         Debug.unityLogger.logEnabled = false;
        // #endif
        Application.targetFrameRate = 60;
        Input.multiTouchEnabled = false;
    }
}