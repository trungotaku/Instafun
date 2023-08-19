using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIAnim : MonoBehaviour
{
    RectTransform thisRect;
    CanvasGroup thisCg;
    void Awake()
    {
        thisCg = GetComponent<CanvasGroup>();
        thisRect = GetComponent<RectTransform>();
    }
    public void DoAppearDefault(float duration_ = .5f, float delay_ = 0f, Action cb_ = null)
    {
        this.gameObject.SetActive(true);
        thisCg.alpha = 0;
        thisRect.localScale = Vector3.one * 1.5f;

        LeanTween.cancel(gameObject, true);
        LeanTween.alphaCanvas(thisCg, 1f, duration_ * .5f).setEase(LeanTweenType.easeOutQuad).setDelay(delay_);
        LeanTween.scale(thisRect, Vector3.one, duration_).setEase(LeanTweenType.easeOutBack).setDelay(delay_)
            .setOnComplete(() =>
            {
                cb_?.Invoke();
            });
    }
    public void DoAppear2(float duration_ = .5f, float delay_ = 0f, Action cb_ = null)
    {
        this.gameObject.SetActive(true);
        thisCg.alpha = 0;
        thisRect.localScale = Vector3.one * 1f;

        LeanTween.alphaCanvas(thisCg, 1f, duration_ * .5f).setEase(LeanTweenType.easeOutQuad).setDelay(delay_);
        LeanTween.scale(thisRect, Vector3.one, duration_).setEase(LeanTweenType.easeOutQuad).setDelay(delay_)
            .setOnComplete(() =>
            {
                cb_?.Invoke();
            });
    }
    public void DoDisappear2(float duration_ = .5f, float delay_ = 0f, Action cb_ = null)
    {
        this.gameObject.SetActive(true);
        // thisCg.alpha = 1;
        thisRect.localScale = Vector3.one * 1f;

        LeanTween.alphaCanvas(thisCg, 0f, duration_).setEase(LeanTweenType.easeOutQuad).setDelay(delay_)
            .setOnComplete(() =>
            {
                cb_?.Invoke();
            });
    }
    public void DoDisappear3(float duration_ = .5f, float delay_ = 0f, Action cb_ = null)
    {
        this.gameObject.SetActive(true);
        thisRect.localScale = Vector3.one * 1f;

        // LeanTween.alphaCanvas(thisCg, 0f, duration_ * .5f).setEase(LeanTweenType.easeOutQuad).setDelay(delay_ + duration_ * .5f);
        LeanTween.alphaCanvas(thisCg, 0f, duration_).setEase(LeanTweenType.easeOutQuad).setDelay(delay_);
        LeanTween.scale(thisRect, Vector3.one * 1.25f, duration_).setEase(LeanTweenType.easeOutQuad).setDelay(delay_)
            .setOnComplete(() =>
            {
                cb_?.Invoke();
            });
    }
    public void TweenAlpha(float alpha_, float duration_ = .5f, float delay_ = 0f, Action cb_ = null)
    {
        this.gameObject.SetActive(true);

        LeanTween.alphaCanvas(thisCg, alpha_, duration_).setEase(LeanTweenType.easeOutQuad).setDelay(delay_)
            .setOnComplete(() =>
            {
                cb_?.Invoke();
            });
    }
    public void DoAppearScoreVFX(float duration_ = .5f, float delay_ = 0f, Action cb_ = null)
    {
        this.gameObject.SetActive(true);
        thisCg.alpha = 0;
        thisRect.localScale = Vector3.one * 1.5f;

        LeanTween.cancel(gameObject, true);
        LeanTween.alphaCanvas(thisCg, 1f, duration_ * .25f).setEase(LeanTweenType.easeOutQuad).setDelay(delay_);
        LeanTween.scale(thisRect, Vector3.one, duration_).setEase(LeanTweenType.easeOutBack).setDelay(delay_)
            .setOnComplete(() =>
            {
                cb_?.Invoke();
            });
    }
    public void SetActive(bool active_)
    {
        this.gameObject.SetActive(active_);
    }
    public void SetAlpha(float alpha_)
    {
        if (thisCg == null)
        {
            thisCg = GetComponent<CanvasGroup>();
        }
        thisCg.alpha = alpha_;
    }
}