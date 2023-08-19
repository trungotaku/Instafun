using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool<T> where T : Component
{
    Transform _root;
    public Transform Root => _root;
    T _sample;
    List<T> _poolElements = new List<T>();
    public List<T> Elements => _poolElements;

    List<T> _poolUsedElements = new List<T>();
    public List<T> UsedElements => _poolUsedElements;

    public ObjectPool(T element, Transform root_ = null)
    {
        _sample = element;
        if (root_ == null)
        {
            this._root = _sample.transform.parent;
        }
        else
        {
            this._root = root_;
        }
        _poolUsedElements = new List<T>();
        _poolElements = new List<T>();
    }

    public T Get(string name_ = "")
    {
        if (_poolElements.Count == 0)
        {
            var go = Object.Instantiate(_sample.gameObject, this._root) as GameObject;
            go.SetActive(true);
            go.transform.localScale = Vector3.one;
            if (!string.IsNullOrEmpty(name_)) go.name = name_;
            _poolUsedElements.Add(go.GetComponent<T>());
            return go.GetComponent<T>();
        }

        var element = _poolElements[0];
        element.gameObject.SetActive(true);
        element.transform.localScale = Vector3.one;
        if (!string.IsNullOrEmpty(name_)) element.name = name_;
        _poolElements.RemoveAt(0);
        _poolUsedElements.Add(element);
        return element;
    }

    public T Get(Transform otherRoot_, string name_ = "")
    {
        if (_poolElements.Count == 0)
        {
            var go = Object.Instantiate(_sample.gameObject, otherRoot_) as GameObject;
            go.gameObject.SetActive(true);
            if (!string.IsNullOrEmpty(name_)) go.name = name_;
            _poolUsedElements.Add(go.GetComponent<T>());
            return go.GetComponent<T>();
        }

        var element = _poolElements[0];
        element.gameObject.SetActive(true);
        element.transform.localScale = Vector3.one;
        element.transform.SetParent(otherRoot_, false);
        if (!string.IsNullOrEmpty(name_)) element.name = name_;
        _poolElements.RemoveAt(0);
        _poolUsedElements.Add(element);
        return element;
    }

    public void Store(T element, bool parentReset_ = false)
    {
        if (parentReset_) element.transform.SetParent(this._root, false);
        element.gameObject.SetActive(false);
        _poolElements.Add(element);
        _poolUsedElements.Remove(element);
    }

    public void StoreWithOutDisableGO(T element)
    {
        _poolElements.Add(element);
        _poolUsedElements.Remove(element);
    }
    public void StoreAllUsedElements(bool parentReset_ = false)
    {
        _poolUsedElements.ForEach(element =>
        {
            if (parentReset_) element.transform.SetParent(this._root, false);
            element.gameObject.SetActive(false);
            _poolElements.Add(element);
        });
        _poolUsedElements.Clear();
    }
}