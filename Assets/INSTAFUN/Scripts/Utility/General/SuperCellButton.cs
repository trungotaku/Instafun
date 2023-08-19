using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SuperCellButton : MonoBehaviour, IPointerClickHandler, IPointerExitHandler, IPointerDownHandler
{
    const float DURATION = 0.04f;
    // const float SCALE_DOWN = 0.94285714285f;
    const float SCALE_DOWN = 0.94f;
    //const float SCALE_UP = 1.05714285714f;
    const float SCALE_UP = 1f;
    bool m_clickedFlag;
    Selectable m_uiSelectable;

    private void Awake()
    {
        m_clickedFlag = false;
        m_uiSelectable = GetComponent<Selectable>();
    }
    public void DoAnimation()
    {
        LeanTween.cancel(gameObject);
        
        var seq = LeanTween.sequence();
        // seq.append(gameObject.LeanScale(Vector3.one * SCALE_DOWN, DURATION * 0.5f).setEase(LeanTweenType.easeOutQuad));
        seq.append(gameObject.LeanScale(Vector3.one * SCALE_UP, DURATION * 1f).setEase(LeanTweenType.easeOutQuad));
        // seq.append(gameObject.LeanScale(Vector3.one, DURATION * 0.75f).setEase(LeanTweenType.easeOutQuad));
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (m_uiSelectable != null && !m_uiSelectable.interactable) return;
        //Debug.LogError("OnPointerClick");
        m_clickedFlag = true;
        DoAnimation();
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        if (m_uiSelectable != null && !m_uiSelectable.interactable) return;

        LeanTween.cancel(gameObject);
        transform.localScale = Vector3.one * SCALE_DOWN;
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        //Debug.LogError("OnPointerExit");
        if (!m_clickedFlag)
        {
            LeanTween.cancel(gameObject);
            this.transform.localScale = Vector3.one;
        }
        m_clickedFlag = false;
    }
}