using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseView : MonoBehaviour
{
    public string Id => GetType().Name;
    public virtual bool IsFullScreen => false;
    [HideInInspector] public BaseView PreviousView;
    [HideInInspector] public BaseView NextView;
    [HideInInspector] public CanvasGroup canvasGroup;

    public virtual void Initialize(Dictionary<string, object> params_ = null)
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }
    public void DoTaskBeforeHide()
    {
        ViewManager.RemoveViewFromStack();
        PreviousView = null;
        NextView = null;

        List<BaseView> views = ViewManager.ViewStack;
        for (int i = views.Count - 1; i >= 0; i--)
        {
            if (views[i] != null)
            {
                if (!views[i].gameObject.activeSelf) views[i].InitializeAfterEnableAgain();

                if (views[i].IsFullScreen)
                {
                    break;
                }
            }
        }
    }
    public virtual void DoWhenHideIsCompleted()
    {
        gameObject.SetActive(false);
    }
    public virtual void DoWhenShowIsCompleted()
    {
        if (IsFullScreen)
        {
            ViewManager.DisableAllPreviousViewInStack();
        }
    }
    public void DoTaskBeforeShow(Dictionary<string, object> params_, bool hasInitialize_ = true)
    {
        ViewManager.AddViewToStack(this);
        NextView = null;
        gameObject.SetActive(true);
        transform.SetAsLastSibling();

        if (hasInitialize_) Initialize(params_);
    }
    public virtual void WalkIn()
    {
        ViewManager.LockViewRootInHierarchy(true);
        Lock(true);
        StartCoroutine(WaitWalkCompleted(GetWalkInTime()));
    }
    public virtual void WalkOut()
    {
        ViewManager.LockViewRootInHierarchy(true);
        Lock(true);
        StartCoroutine(WaitWalkCompleted(GetWalkOutTime()));
    }
    public virtual float GetWalkInTime() { return 0.0f; }
    public virtual float GetWalkOutTime() { return GetWalkInTime(); }
    IEnumerator WaitWalkCompleted(float duration_)
    {
        yield return new WaitForSecondsRealtime(duration_);
        ViewManager.LockViewRootInHierarchy(false);
        Lock(false);
    }
    public virtual void Lock(bool isTrue_)
    {
        canvasGroup.blocksRaycasts = !isTrue_;

        if (isTrue_)
        {
            canvasGroup.interactable = true;
        }
    }
    public virtual void InitializeAfterEnableAgain()
    {
        gameObject.SetActive(true);
    }
}
public interface IPopup
{

}