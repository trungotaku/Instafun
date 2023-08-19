using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Constants;
using UnityEngine;

public class CameraShake : SceneSingleton<CameraShake>
{
    Vector3 m_originalPos;
    private void Start()
    {
        m_originalPos = transform.position;
    }
    public void ShakeCamera(float duration_, int count_ = 2, float shakeAmt_ = 0.2f)
    {
        LeanTween.cancel(gameObject, true);
        float stepDuration = duration_ / (float)count_;
        for (int i = 0; i < count_ - 1; i++)
        {
            Vector3 newPos = UnityEngine.Random.insideUnitCircle.normalized * shakeAmt_;
            newPos.z = transform.position.z;
            LeanTween.move(gameObject, newPos, stepDuration).setEase(LeanTweenType.easeShake).setDelay(stepDuration * i);
        }
        LeanTween.move(gameObject, m_originalPos, stepDuration).setEase(LeanTweenType.easeShake).setDelay(stepDuration * (count_ - 1))
        .setOnComplete(() => transform.position = m_originalPos);
    }
    [ContextMenu("ShakeCamera")]
    void ShakeCamera()
    {
        ShakeCamera(0.4f, 2, 0.2f);
    }
}