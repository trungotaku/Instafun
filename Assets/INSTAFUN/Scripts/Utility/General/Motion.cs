
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Motion : MonoBehaviour
{
    private float v = 0.0f;
    private Vector3 v3Pos;
    private float g = 9.8f;
    void Start()
    {
        v3Pos = transform.position;
    }

    // deltaY = Vprev*t - 1/2*g*(t*t)
    // deltaV = -g*t
    void Update()
    {
        float up = 0.0f;  // Upward force

        if (Input.GetKey(KeyCode.A))
        {
            up = 8.0f;  // Apply some upward force
            Debug.Log("Down");
        }

        float t = Time.deltaTime;
        float delta = v * t + (up - g) * t * t * 0.5f;
        v = v + (up - g) * t;
        v3Pos.y += delta;
        // if (v3Pos.y < 0.0f)
        // {
        //     v3Pos.y = 0.0f;
        //     v = -v * .8f;
        // }
        transform.position = v3Pos;

    }
}