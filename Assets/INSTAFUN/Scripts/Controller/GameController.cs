using System.Collections;
using System.Collections.Generic;
using Constants;
using UnityEngine;
using System.Linq;

public class GameController : MonoBehaviour
{
    [SerializeField] GameView m_view;

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
        float deltaY = item_.transform.position.y - item_.CenterY;
        if (item_ is CharacterItem)
        {
            CharacterItem characterItem = (CharacterItem) item_;
            if (characterItem.FootPosY > m_view.FloorLimitPosMaxY)
            {
                LeanTween.moveY(item_.gameObject, m_view.FloorLimitPosMaxY + deltaY, 0.5f).setEaseOutBounce()
                    .setOnComplete(() =>
                    {
                        RefreshSortingOrder();
                    });
            }
            else if (characterItem.FootPosY < m_view.FloorLimitPosMinY)
            {
                LeanTween.moveY(item_.gameObject, m_view.FloorLimitPosMinY + deltaY, 0.25f).setEaseOutQuad()
                    .setOnComplete(() =>
                    {
                        RefreshSortingOrder();
                    });
            }
            else
            {
                RefreshSortingOrder();
            }
        }
        else if (item_ is CakeItem)
        {
            CakeItem cakeItem = (CakeItem) item_;
            if (cakeItem.CenterY > m_view.FloorLimitPosMaxY)
            {
                LeanTween.moveY(item_.gameObject, m_view.FloorLimitPosMaxY + deltaY, 0.5f).setEaseOutBounce()
                    .setOnComplete(() =>
                    {
                        RefreshSortingOrder();
                    });
            }
            else if (cakeItem.CenterY < m_view.FloorLimitPosMinY)
            {
                LeanTween.moveY(item_.gameObject, m_view.FloorLimitPosMinY + deltaY, 0.25f).setEaseOutQuad()
                    .setOnComplete(() =>
                    {
                        RefreshSortingOrder();
                    });
            }
            else
            {
                RefreshSortingOrder();
            }
        }
        else if (item_ is CushionItem)
        {
            CushionItem cushionItem = (CushionItem) item_;
            if (cushionItem.CenterY > m_view.FloorLimitPosMaxY)
            {
                LeanTween.moveY(item_.gameObject, m_view.FloorLimitPosMaxY + deltaY, 0.5f).setEaseOutBounce()
                    .setOnComplete(() =>
                    {
                        RefreshSortingOrder();
                    });
            }
            else if (cushionItem.CenterY < m_view.FloorLimitPosMinY)
            {
                LeanTween.moveY(item_.gameObject, m_view.FloorLimitPosMinY + deltaY, 0.25f).setEaseOutQuad()
                    .setOnComplete(() =>
                    {
                        RefreshSortingOrder();
                    });
            }
            else
            {
                RefreshSortingOrder();
            }
        }
    }

    public void RefreshSortingOrder()
    {
        m_view.sortingItems.OrderByDescending(x => x.CenterY).ToList().ForEach((i, item_) =>
        {
            item_.sortingGroup.sortingOrder = i;
        });
    }
}