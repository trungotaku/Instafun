using System.Collections;
using System.Collections.Generic;
using Constants;
using UnityEngine;

public class TableItem : BaseItem
{
    [SerializeField] Transform a;
    [SerializeField] Transform b;
    [SerializeField] Transform c;
    [SerializeField] Transform d;

    public Vector2 A => a.position;
    public Vector2 B => b.position;
    public Vector2 C => c.position;
    public Vector2 D => d.position;

    private void OnEnable()
    {
        Messenger.AddListener<BaseItem>(GameEvent.ON_END_DRAG_ITEM, OnEndDragItem);
    }
    private void OnDisable()
    {
        Messenger.RemoveListener<BaseItem>(GameEvent.ON_END_DRAG_ITEM, OnEndDragItem);
    }

    void OnEndDragItem(BaseItem item_)
    {
        if (item_ is CakeItem)
        {
            CakeItem cakeItem = (CakeItem) item_;
            float deltaY = item_.transform.position.y - cakeItem.CenterY;
            Vector2 centerPos = (A + B + C + D) / 4f;

            if (UtilityExtension.IsPointInsideQuadrilateral(cakeItem.Center, new Vector2[] { A, B, C, D }))
            {
                Debug.Log("Inside");
                cakeItem.sortingGroup.sortingOrder = this.sortingGroup.sortingOrder + 1;
            }
            else
            {
                Debug.Log("Outside");
                if (cakeItem.CenterY < CenterY)
                {
                    cakeItem.sortingGroup.sortingOrder = this.sortingGroup.sortingOrder + 1;
                }
                else
                {
                    cakeItem.sortingGroup.sortingOrder = this.sortingGroup.sortingOrder - 1;
                }
            }
        }
        else if (item_ is CushionItem)
        {
            CushionItem cushionItem = (CushionItem) item_;
            float deltaY = item_.transform.position.y - cushionItem.CenterY;
            Vector2 centerPos = (A + B + C + D) / 4f;

            if (UtilityExtension.IsPointInsideQuadrilateral(cushionItem.Center, new Vector2[] { A, B, C, D }))
            {
                Debug.Log("Inside");
                cushionItem.sortingGroup.sortingOrder = this.sortingGroup.sortingOrder + 1;
            }
            else
            {
                Debug.Log("Outside");
                if (cushionItem.CenterY < CenterY)
                {
                    cushionItem.sortingGroup.sortingOrder = this.sortingGroup.sortingOrder + 1;
                }
                else
                {
                    cushionItem.sortingGroup.sortingOrder = this.sortingGroup.sortingOrder - 1;
                }
            }
        }
    }
}