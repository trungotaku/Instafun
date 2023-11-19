using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class ViewManager : SceneSingleton<ViewManager>
{
    #region INITIALIZATION
    const string PREFABS_BASE_PATH = "ViewPrefabs/";
    [SerializeField] GameObject m_blockGO;
    Dictionary<string, BaseView> m_instantiatedViews;
    [Header("INITIALIZATION")]
    [SerializeField] List<BaseView> EarlyInitializationViews;
    [Header("STACK (ONLY SEE, DONT EDIT PLEASE!!)")]
    [SerializeField] List<BaseView> m_viewStack;

    void Awake()
    {
        _InitializeUI();
    }
    void _InitializeUI()
    {
        m_instantiatedViews = new Dictionary<string, BaseView>();
        m_viewStack = new List<BaseView>();
        EarlyInitializationViews.ForEach(view =>
        {
            m_instantiatedViews.Add(view.GetType().Name, view);
        });
    }
    #endregion INITIALIZATION

    #region PRIVATE METHODS
    BaseView _Show(string viewId_, Dictionary<string, object> params_ = null, Action onCompleted_ = null)
    {
        BaseView view = _GetView(viewId_);
        if (view == null)
        {
            Debug.LogError("View prefab not found " + viewId_.ToString());
            return null;
        }
        _LockViewRootInHierarchy(true);
        BaseView currentView = CurrentView;
        if (currentView != null)
        {
            if (viewId_ != currentView.Id)
            {
                currentView.Lock(true);
            }
            else
            {
                Debug.LogWarning("Warning!!! This view is existed in STACK: " + viewId_.ToString());
                _RemoveViewFromStack();
                view.DoTaskBeforeShow(params_);
                StartCoroutine(_ExecuteWalkIn(view));
                return view;
            }
        }
        view.DoTaskBeforeShow(params_);
        StartCoroutine(_ExecuteWalkIn(view, onCompleted_));
        return view;
    }
    T _Show<T>(Dictionary<string, object> params_ = null, Action onCompleted_ = null) where T : BaseView
    {
        string viewId = typeof(T).Name;
        BaseView view = _GetView(viewId);
        if (view == null)
        {
            Debug.LogError("View prefab not found " + viewId.ToString());
            return null;
        }
        _LockViewRootInHierarchy(true);
        BaseView currentView = CurrentView;
        if (currentView != null)
        {
            if (viewId != currentView.GetType().Name)
            {
                currentView.Lock(true);
            }
            else
            {
                Debug.LogWarning("Warning!!! This view is existed in STACK: " + viewId.ToString());
                _RemoveViewFromStack();
                view.DoTaskBeforeShow(params_);
                StartCoroutine(_ExecuteWalkIn(view));
                return view as T;
            }
        }
        view.DoTaskBeforeShow(params_);
        StartCoroutine(_ExecuteWalkIn(view, onCompleted_));
        return view as T;
    }
    void _Hide(BaseView view_, Action onCompleted_)
    {
        if (view_ == null)
        {
            Debug.LogError("[ViewManager] Error!!! This view is null");
            return;
        };
        BaseView currentView = CurrentView;
        if (view_.GetType().Name != currentView.GetType().Name)
        {
            Debug.LogError("[ViewManager] Error!!! This view is not the last view in STACK!!!");
            return;
        }
        view_.DoTaskBeforeHide();
        if (view_.gameObject.activeSelf)
        {
            StartCoroutine(_ExecuteWalkOut(view_, onCompleted_));
        }
        else
        {
            onCompleted_?.Invoke();
        }
    }
    void _QuickHide(BaseView view_)
    {
        if (view_ == null)
        {
            Debug.LogError("[ViewManager] Error!!! This view is null");
            return;
        };
        BaseView currentView = CurrentView;
        if (view_.GetType().Name != currentView.GetType().Name)
        {
            Debug.LogError("[ViewManager] Error!!! This view is not the last view in STACK!!!");
            return;
        }
        view_.DoTaskBeforeHide();
        if (view_.gameObject.activeSelf)
        {
            if (view_ != null)
            {
                if (m_viewStack.Count > 0)
                {
                    _LockViewRootInHierarchy(false);
                    CurrentView.Lock(false);
                }
                view_.DoWhenHideIsCompleted();
            }
        }
    }
    BaseView _GetView(string viewId_)
    {
        if (m_instantiatedViews == null) return null;

        if (m_instantiatedViews.ContainsKey(viewId_))
        {
            return m_instantiatedViews[viewId_];
        }
        else
        {
            BaseView bview = Instantiate(Resources.Load<BaseView>(PREFABS_BASE_PATH + viewId_), transform, false) as BaseView;
            bview.name = viewId_;
            m_instantiatedViews.Add(viewId_, bview);

            return m_instantiatedViews[viewId_];
        }
    }
    void _LockViewRootInHierarchy(bool isTrue_)
    {
        m_blockGO.SetActive(isTrue_);
    }
    IEnumerator _ExecuteWalkIn(BaseView view_, Action onCompleted_ = null)
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
    IEnumerator _ExecuteWalkOut(BaseView view_, Action onCompleted_ = null)
    {
        if (view_ != null)
        {
            float waitTime = view_.GetWalkOutTime();
            view_.WalkOut();

            yield return new WaitForSecondsRealtime(waitTime);

            if (m_viewStack.Count > 0)
            {
                _LockViewRootInHierarchy(false);
                CurrentView.Lock(false);
            }
            view_.DoWhenHideIsCompleted();
            onCompleted_?.Invoke();
        }
        yield break;
    }
    void _AddViewToStack(BaseView view_)
    {
        m_viewStack.Add(view_);
        if (m_viewStack.Count > 1)
        {
            m_viewStack.Last().PreviousView = m_viewStack[m_viewStack.Count - 2];
            m_viewStack[m_viewStack.Count - 2].NextView = view_;
        }
    }
    void _RemoveViewFromStack()
    {
        if (m_viewStack.Count > 0)
        {
            m_viewStack.RemoveAt(m_viewStack.Count - 1);
            if (m_viewStack.Count > 0) m_viewStack.LastOrDefault().NextView = null;
        }
    }
    void _DisableAllPreviousViewInStack()
    {
        if (m_viewStack.Count > 0)
        {
            for (int i = 0; i < m_viewStack.Count - 1; i++)
            {
                m_viewStack[i].gameObject.SetActive(false);
            }
        }
    }
    BaseView _GetCurrentView()
    {
        return m_viewStack.Count > 0 ? m_viewStack.Last() : null;
    }
    BaseView _GetPreviousView()
    {
        return m_viewStack.Count > 1 ? m_viewStack[m_viewStack.Count - 2] : null;
    }
    #endregion PRIVATE METHODS

    #region OBSOLETE METHODS
    public static List<BaseView> ViewStack { get { return Instance.m_viewStack; } }
    public static BaseView PreviousView { get { return Instance._GetPreviousView(); } }
    public static void LockViewRootInHierarchy(bool isTrue_) { Instance._LockViewRootInHierarchy(isTrue_); }
    public static void AddViewToStack(BaseView view_) { Instance._AddViewToStack(view_); }
    public static void RemoveViewFromStack() { Instance._RemoveViewFromStack(); }
    public static void DisableAllPreviousViewInStack() { Instance._DisableAllPreviousViewInStack(); }
    #endregion OBSOLETE METHODS

    #region PUBLIC METHODS
    public static BaseView CurrentView { get { return Instance._GetCurrentView(); } }
    public static BaseView Show(string viewId_, Dictionary<string, object> params_ = null, Action onCompleted_ = null) { return Instance._Show(viewId_, params_, onCompleted_); }
    public static T Show<T>(Dictionary<string, object> params_, Action onCompleted_ = null) where T : BaseView { return Instance._Show<T>(params_, onCompleted_); }
    public static T Show<T>(Action onCompleted_ = null) where T : BaseView { return Instance._Show<T>(null, onCompleted_); }
    public static void Hide(Action onCompleted_ = null) { Instance._Hide(CurrentView, onCompleted_); }
    public static void QuickHide() { Instance._QuickHide(CurrentView); }
    #endregion PUBLIC METHODS
}