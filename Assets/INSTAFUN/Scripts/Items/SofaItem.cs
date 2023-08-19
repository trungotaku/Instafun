using System.Collections;
using System.Collections.Generic;
using Constants;
using UnityEngine;

public class SofaItem : BaseItem
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
        if (item_ is CharacterItem)
        {
            CharacterItem characterItem = (CharacterItem) item_;
            float deltaY = item_.transform.position.y - characterItem.SofaPos.y;
            Vector2 centerPos = (A + B + C + D) / 4f;

            if (UtilityExtension.IsPointInsideQuadrilateral(characterItem.SofaPos, new Vector2[] { A, B, C, D }))
            {
                Debug.Log("Inside");
                characterItem.SitDown(centerPos + deltaY * Vector2.up);
            }
            else
            {
                Debug.Log("Outside");
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