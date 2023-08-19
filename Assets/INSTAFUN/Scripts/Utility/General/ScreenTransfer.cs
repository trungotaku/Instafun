using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenTransfer : SceneSingleton<ScreenTransfer>
{
    [SerializeField] CanvasGroup thisCg;
    float DURATION => .125f;
    public void WalkIn(Action cb_)
    {
        LeanTween.cancel(gameObject, true);
        thisCg.alpha = 0f;
        LeanTween.alphaCanvas(thisCg, 1, DURATION).setEaseOutQuad()
        .setOnComplete(() =>
        {
            cb_?.Invoke();
        });
    }
    public void WalkOut(float delay_ = 0f, Action cb_ = null)
    {
        LeanTween.cancel(gameObject, true);
        thisCg.alpha = 1f;
        LeanTween.alphaCanvas(thisCg, 0, DURATION).setEaseOutQuad().setDelay(delay_)
        .setOnComplete(() =>
        {
            cb_?.Invoke();
        });
    }
}
