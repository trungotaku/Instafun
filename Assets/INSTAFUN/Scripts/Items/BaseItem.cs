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

    public virtual void Init()
    {
        sortingGroup.sortingOrder = this.transform.GetSiblingIndex();
    }
}