using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class BaseItem : MonoBehaviour
{
    public SortingGroup sortingGroup;
    [SerializeField] Transform center;
    public Vector2 Center => center.position;
    public float CenterY => center.position.y;

    Transform m_parentAtInit;
    public Transform ParentAtInit => m_parentAtInit;
    private void Awake()
    {
        m_parentAtInit = this.transform.parent;
    }

    public virtual void Init()
    {
        sortingGroup.sortingOrder = this.transform.GetSiblingIndex();
        m_parentAtInit = this.transform.parent;
    }
}