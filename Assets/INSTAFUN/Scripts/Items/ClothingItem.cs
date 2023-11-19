using System.Collections;
using System.Collections.Generic;
using Constants;
using UnityEngine;
using UnityEngine.EventSystems;

public class ClothingItem : BaseItem, IDragHandler, IEndDragHandler, IBeginDragHandler
{
    private bool m_isDrag = false;
    Vector3 m_deltaPos = Vector3.zero;
    [SerializeField] ClothingSlot m_clothingSlot;
    [SerializeField] CharacterSkin _characterSkin;
    public CharacterSkin CharacterSkin => _characterSkin;
    public ClothingSlot ClothingSlot => m_clothingSlot;

    public override void Init()
    {
        base.Init();
        transform.localPosition = Vector3.zero;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        m_isDrag = true;

        Vector3 thisPos = this.transform.position;
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        m_deltaPos = thisPos - mousePos;

        sortingGroup.sortingLayerName = SortingLayerConfig.ITEM_IS_DRAGGING;
    }
    public void OnDrag(PointerEventData eventData)
    {
        m_isDrag = true;

        Vector3 thisPos = this.transform.position;
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        Vector3 pos = Vector3.right * mousePos.x + Vector3.up * mousePos.y + m_deltaPos;
        this.transform.position = new Vector3(pos.x, pos.y, 0);
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        m_isDrag = false;

        sortingGroup.sortingLayerName = SortingLayerConfig.ITEM;
        Messenger.Broadcast(GameEvent.ON_END_DRAG_ITEM, (BaseItem)this);
    }
}

public enum CharacterSkin
{
    None = -1,
    Pink = 1,
    Green = 2,
    WhiteBlack = 3,
    Purple = 4,
}