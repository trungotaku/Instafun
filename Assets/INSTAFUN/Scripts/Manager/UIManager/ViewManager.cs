using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.Linq;
using System.Linq;

public class ViewManager : SceneSingleton<ViewManager>
{
    private const string PREFABS_BASE_PATH = "ViewPrefabs/";
    [SerializeField] GameObject m_lockGO;
    public BaseView CurrentView
    {
        get { return viewStack.Count > 0 ? viewStack.Last() : null; }
    }
    public BaseView PreviousView
    {
        get { return viewStack.Count > 1 ? viewStack.Last().PreviousView : null; }
    }
    private Dictionary<ViewId, string> m_viewsDictionary;
    private Dictionary<ViewId, BaseView> m_instantiatedViews;
    [Header("INITIALIZATION")]
    [SerializeField] private List<BaseView> EarlyInitializationViews;
    [Header("STACK (ONLY SEE, DONT EDIT PLEASE!!)")]
    [SerializeField] private List<BaseView> viewStack;

    public static List<BaseView> GetViewStack()
    {
        return Instance.viewStack;
    }

    void Awake()
    {
        InitializeUI();
    }

    void InitializeUI()
    {
        m_instantiatedViews = new Dictionary<ViewId, BaseView>();
        m_viewsDictionary = new Dictionary<ViewId, string>();

        foreach (ViewId id in (ViewId[]) Enum.GetValues(typeof(ViewId)))
        {
            m_viewsDictionary.Add(id, id.ToString());
        }

        viewStack = new List<BaseView>();

        EarlyInitializationViews.ForEach(view => m_instantiatedViews.Add(view.Id, view));
    }
    public static void Show(ViewId viewID_, Dictionary<string, object> params_, Action onCompleted_ = null)
    {
        Instance._Show(viewID_, params_, onCompleted_);
    }
    public static void Show(ViewId viewID_, Action onCompleted_ = null)
    {
        Instance._Show(viewID_, null, onCompleted_);
    }
    void _Show(ViewId viewID_, Dictionary<string, object> params_ = null, Action onCompleted_ = null)
    {
        BaseView view = GetView(viewID_);
        if (view == null)
        {
            Debug.LogError("View prefab not found " + viewID_.ToString());
            return;
        }

        LockViewRootInHierarchy(true);
        BaseView currentView = CurrentView;
        if (currentView != null)
        {
            if (viewID_ != currentView.Id)
            {
                currentView.Lock(true);
            }
            else
            {
                Debug.LogWarning("Warning!!! This view is existed in STACK: " + viewID_.ToString());
                _RemoveViewFromStack();
                view.Show(params_);
                StartCoroutine(ExecuteWalkIn(view));
                return;
            }
        }

        view.Show(params_);
        StartCoroutine(ExecuteWalkIn(view, onCompleted_));
    }
    public static void Hide(Action onCompleted_ = null)
    {
        Instance._Hide(Instance.CurrentView, onCompleted_);
    }
    void _Hide(BaseView view_, Action onCompleted_)
    {
        if (view_ == null)
        {
            Debug.LogError("[ViewManager] Error!!! This view is null");
            return;
        };

        BaseView currentView = CurrentView;
        if (view_.Id != currentView.Id)
        {
            Debug.LogError("[ViewManager] Can't hide, this view is not the last view in STACK!!!");
            return;
        }
        view_.Hide();
        if (view_.gameObject.activeSelf)
        {
            StartCoroutine(ExecuteWalkOut(view_, onCompleted_));
        }
        else
        {
            onCompleted_?.Invoke();
        }
    }
    BaseView GetView(ViewId index_)
    {
        if (m_instantiatedViews == null) return null;

        if (m_instantiatedViews.ContainsKey(index_))
        {
            return m_instantiatedViews[index_];
        }
        else
        {
            BaseView bview = Instantiate(Resources.Load<BaseView>(PREFABS_BASE_PATH + m_viewsDictionary[index_]), transform, false) as BaseView;
            bview.name = m_viewsDictionary[index_];
            m_instantiatedViews.Add(index_, bview);

            return m_instantiatedViews[index_];
        }
    }
    IEnumerator ExecuteWalkIn(BaseView view_, Action onCompleted_ = null)
    {
        if (view_ != null)
        {
            float waitTime = view_.GetWalkInTime();
            view_.WalkIn();
            yield return new WaitForSecondsRealtime(waitTime);

            view_.DoWhenShowIsCompleted();

            onCompleted_?.Invoke();
        }
        else
        {
            Debug.LogError("[ViewManager] [ExecuteWalkIn] view is null!!!");
            onCompleted_?.Invoke();
        }
        yield break;
    }
    IEnumerator ExecuteWalkOut(BaseView view_, Action onCompleted_ = null)
    {
        if (view_ != null)
        {
            float waitTime = view_.GetWalkOutTime();
            view_.WalkOut();

            yield return new WaitForSecondsRealtime(waitTime);

            if (viewStack.Count > 0)
            {
                LockViewRootInHierarchy(false);
                CurrentView.Lock(false);
            }
            view_.DoWhenHideIsCompleted();
            onCompleted_?.Invoke();
        }
        yield break;
    }
    public static void LockViewRootInHierarchy(bool isTrue_)
    {
        Instance._Lock(isTrue_);
    }
    void _Lock(bool isTrue_)
    {
        m_lockGO.SetActive(isTrue_);
    }
    public static void AddViewToStack(BaseView view_)
    {
        Instance._AddViewToStack(view_);
    }
    private void _AddViewToStack(BaseView view_)
    {
        viewStack.Add(view_);
        if (viewStack.Count > 1)
        {
            viewStack.Last().PreviousView = viewStack[viewStack.Count - 2];
            viewStack[viewStack.Count - 2].NextView = view_;
        }
    }
    public static void RemoveViewFromStack()
    {
        Instance._RemoveViewFromStack();
    }
    private void _RemoveViewFromStack()
    {
        if (viewStack.Count > 0)
        {
            viewStack.RemoveAt(viewStack.Count - 1);
            if (viewStack.Count > 0) viewStack.LastOrDefault().NextView = null;
        }
    }

    void _DisableAllPreviousViewInStack()
    {
        if (viewStack.Count > 0)
        {
            for (int i = 0; i < viewStack.Count - 1; i++)
            {
                viewStack[i].gameObject.SetActive(false);
            }
        }
    }
    public static void DisableAllPreviousViewInStack()
    {
        Instance._DisableAllPreviousViewInStack();
    }
}