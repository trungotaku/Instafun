using System.Collections;
using System.Collections.Generic;
using Constants;
using UnityEngine;

public class ClothingSlot : MonoBehaviour
{
    [SerializeField] ClothingItem m_clothingItem;
    public ClothingItem ClothingItem => m_clothingItem;
    [SerializeField] ClothingFoldedItem m_clothingFoldedItem;

    [SerializeField] AreaRoot m_areaRoot;

    public ClothingFoldedItem ClothingFoldedItem => m_clothingFoldedItem;

    public void Awake()
    {
    }

    private void OnEnable()
    {
        Messenger.AddListener<BaseItem>(GameEvent.ON_END_DRAG_ITEM, OnEndDragItem);
    }
    private void OnDisable()
    {
        Messenger.RemoveListener<BaseItem>(GameEvent.ON_END_DRAG_ITEM, OnEndDragItem);
    }

    public void Hang()
    {
        m_clothingItem.transform.localPosition = Vector3.zero;
        m_clothingFoldedItem.transform.localPosition = Vector3.zero;

        m_clothingItem.gameObject.SetActive(true);
        m_clothingFoldedItem.gameObject.SetActive(false);
    }
    public void Fold(Vector3 beginPos_, float endPosY_)
    {
        m_clothingItem.gameObject.SetActive(false);

        m_clothingFoldedItem.sortingGroup.sortingLayerName = SortingLayerConfig.ITEM_IS_DRAGGING;
        m_clothingFoldedItem.gameObject.SetActive(true);
        m_clothingFoldedItem.transform.position = beginPos_;
        LeanTween.moveY(m_clothingFoldedItem.gameObject, endPosY_, 0.5f).setEaseOutBounce()
        .setOnComplete(() =>
        {
            m_clothingFoldedItem.sortingGroup.sortingLayerName = SortingLayerConfig.ITEM;
        });
    }
    public void PutOnCharacter()
    {
        m_clothingItem.gameObject.SetActive(false);
        m_clothingFoldedItem.gameObject.SetActive(false);
    }

    void OnEndDragItem(BaseItem item_)
    {

        if (item_ is ClothingFoldedItem && item_ == this.m_clothingFoldedItem)
        {
            ClothingFoldedItem foldItem = (ClothingFoldedItem)item_;
            if (UtilityExtension.IsPointInsideQuadrilateral(foldItem.Center, new Vector2[] { m_areaRoot.A, m_areaRoot.B, m_areaRoot.C, m_areaRoot.D }))
            {
                Debug.Log("Inside Clothing Slot");
                Hang();
            }
            else
            {
                Debug.Log("Outside Clothing Slot");
            }
        }
    }
}
