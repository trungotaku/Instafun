using System.Collections;
using System.Collections.Generic;
using Constants;
using UnityEngine;
using UnityEngine.EventSystems;
using DragonBones;
using UnityEngine.Rendering;
using Unity.Linq;
using Transform = UnityEngine.Transform;

public class CharacterItem : BaseItem, IDragHandler, IEndDragHandler, IBeginDragHandler
{
    [SerializeField] AreaRoot m_boxRoot;
    [SerializeField] AreaRoot m_handLeftRoot;
    [SerializeField] AreaRoot m_handRightRoot;

    [SerializeField] ClothingPanel clothingPanel;
    [SerializeField] UnityEngine.Transform footRoot;
    [SerializeField] UnityEngine.Transform sofaRoot;
    [SerializeField] UnityArmatureComponent armatureComponent;

    public float FootPosY => footRoot.position.y;
    public Vector2 SofaPos => sofaRoot.position;

    private bool m_isDrag = false;

    Vector3 m_deltaPos = Vector3.zero;

    CharacterSkin m_currentCharacterSkin = CharacterSkin.Pink;

    private CharacterState m_state = CharacterState.Stand;

    [SerializeField] List<GameObject> _skinSlots;
    Dictionary<string, GameObject> _skinSlotDict = new Dictionary<string, GameObject>();

    private void Awake()
    {
        _skinSlots.ForEach(x => _skinSlotDict.Add(x.name, x));
        ChangeSkin(CharacterSkin.Pink);
    }

    private void OnEnable()
    {
        Messenger.AddListener<BaseItem>(GameEvent.ON_END_DRAG_ITEM, OnEndDragItem);
    }
    private void OnDisable()
    {
        Messenger.RemoveListener<BaseItem>(GameEvent.ON_END_DRAG_ITEM, OnEndDragItem);
    }

    private void Start()
    {
        Init();
    }

    public override void Init()
    {
        base.Init();
        Debug.Log("CharacterItem Init");

        m_state = CharacterState.Stand;
        armatureComponent.animation.Play("stand");
    }
    public void SitDown(Vector3 pos_)
    {
        m_state = CharacterState.Sit;
        armatureComponent.animation.Play("sit");
        LeanTween.move(this.gameObject, pos_, 0.125f).setEaseOutQuad();
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        m_isDrag = true;

        Vector3 thisPos = this.transform.position;
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        m_deltaPos = thisPos - mousePos;

        sortingGroup.sortingLayerName = SortingLayerConfig.ITEM_IS_DRAGGING;
        armatureComponent.animation.Play("drag_breath");
    }
    public void OnDrag(PointerEventData eventData)
    {
        m_isDrag = true;
        m_state = CharacterState.IsDragging;

        Vector3 thisPos = this.transform.position;
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        Vector3 pos = Vector3.right * mousePos.x + Vector3.up * mousePos.y + m_deltaPos;
        this.transform.position = new Vector3(pos.x, pos.y, 0);
        // this.transform.localPosition = new Vector3(this.transform.localPosition.x, this.transform.localPosition.y, 0);
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        m_isDrag = false;
        m_state = CharacterState.Stand;

        sortingGroup.sortingLayerName = SortingLayerConfig.ITEM;
        armatureComponent.animation.Play("stand");
        Messenger.Broadcast(GameEvent.ON_END_DRAG_ITEM, (BaseItem)this);
    }

    float m_hiTimer = 0f;
    private void Update()
    {
        // Update the timer
        m_hiTimer += Time.deltaTime;
        // Check if it's time to play the animation
        if (m_hiTimer >= 10f)
        {
            _PlayHiAnimation();
        }
    }

    public void ChangeSkin(CharacterSkin skin_)
    {
        Debug.Log("ChangeSkin: " + skin_);
        m_currentCharacterSkin = skin_;
        _skinSlots.ForEach(x =>
        {
            foreach (UnityEngine.Transform child in x.transform)
            {
                child.gameObject.SetActive(false);
            }
        });

        List<Sprite> spriteList = new List<Sprite>(Resources.LoadAll<Sprite>("UI/2/wearing/" + (int)skin_));
        spriteList.ForEach(_ =>
        {
            SpriteRenderer spriteRenderer = _skinSlotDict[_.name].GetComponentInChildren<SpriteRenderer>();
            if (spriteRenderer == null)
            {
                GameObject go = Instantiate(new GameObject(), _skinSlotDict[_.name].transform);
                go.transform.parent = _skinSlotDict[_.name].transform;
                go.transform.localPosition = Vector3.zero;
                spriteRenderer = go.AddComponent<SpriteRenderer>();
            }
            else
            {
                spriteRenderer.gameObject.SetActive(true);
            }
            spriteRenderer.sprite = _;
        });
    }

    void OnEndDragItem(BaseItem item_)
    {

        if (item_ is ClothingItem)
        {
            ClothingItem clothingItem = (ClothingItem)item_;
            if (UtilityExtension.IsPointInsideQuadrilateral(clothingItem.Center, new Vector2[] { m_boxRoot.A, m_boxRoot.B, m_boxRoot.C, m_boxRoot.D }))
            {
                Debug.Log("Inside");
                clothingItem.ClothingSlot.PutOnCharacter();
                clothingPanel.GetSlotByCharacterSkin(m_currentCharacterSkin).Fold(transform.position, footRoot.position.y - 0.25f);
                ChangeSkin(clothingItem.CharacterSkin);

                _PlayHiAnimation();
            }
            else
            {
                Debug.Log("Outside");
                clothingItem.gameObject.LeanMoveLocal(Vector3.zero, 0.125f).setEaseOutQuad();
            }
        }
        else if (item_ is ClothingFoldedItem)
        {
            ClothingFoldedItem foldItem = (ClothingFoldedItem)item_;
            if (UtilityExtension.IsPointInsideQuadrilateral(foldItem.Center, new Vector2[] { m_boxRoot.A, m_boxRoot.B, m_boxRoot.C, m_boxRoot.D }))
            {
                Debug.Log("Inside");
                foldItem.ClothingSlot.PutOnCharacter();
                clothingPanel.GetSlotByCharacterSkin(m_currentCharacterSkin).Fold(transform.position, footRoot.position.y - 0.25f);
                ChangeSkin(foldItem.CharacterSkin);

                _PlayHiAnimation();
            }
            else
            {
                Debug.Log("Outside");
            }
        }
        else if (item_ is CushionItem)
        {
            CushionItem cushionItem = (CushionItem)item_;
            if (m_handLeftRoot.GetComponentInChildren<BaseItem>() == null
            && UtilityExtension.IsPointInsideQuadrilateral(cushionItem.HandPos, new Vector2[] { m_handLeftRoot.A, m_handLeftRoot.B, m_handLeftRoot.C, m_handLeftRoot.D }))
            {
                Debug.Log("LeftHand CharacterItem");
                _AttachItem(m_handLeftRoot.transform, cushionItem.transform);

                switch (m_state)
                {
                    case CharacterState.Stand:
                    case CharacterState.Hi:
                        armatureComponent.animation.Play("handl_hold");
                        break;
                    case CharacterState.Sit:
                        armatureComponent.animation.Play("sit_handl_hold");
                        break;
                }
            }
            else if (m_handRightRoot.GetComponentInChildren<BaseItem>() == null
            && UtilityExtension.IsPointInsideQuadrilateral(cushionItem.HandPos, new Vector2[] { m_handRightRoot.A, m_handRightRoot.B, m_handRightRoot.C, m_handRightRoot.D }))
            {
                Debug.Log("RightHand CharacterItem");
                _AttachItem(m_handRightRoot.transform, cushionItem.transform);

                switch (m_state)
                {
                    case CharacterState.Stand:
                    case CharacterState.Hi:
                        armatureComponent.animation.Play("handr_hold");
                        break;
                    case CharacterState.Sit:
                        armatureComponent.animation.Play("sit_handr_hold");
                        break;
                }
            }
            else
            {
                Debug.Log("Outside CharacterItem");
                // _AttachItem(cushionItem.ParentAtInit, cushionItem.transform);
            }
        }
        else if (item_ is CakeItem)
        {
            CakeItem cakeItem = (CakeItem)item_;
            if (m_handLeftRoot.GetComponentInChildren<BaseItem>() == null
            && UtilityExtension.IsPointInsideQuadrilateral(cakeItem.HandPos, new Vector2[] { m_handLeftRoot.A, m_handLeftRoot.B, m_handLeftRoot.C, m_handLeftRoot.D }))
            {
                Debug.Log("LeftHand CharacterItem");
                _AttachItem(m_handLeftRoot.transform, cakeItem.transform);

                switch (m_state)
                {
                    case CharacterState.Stand:
                    case CharacterState.Hi:
                        armatureComponent.animation.Play("handl_hold");
                        break;
                    case CharacterState.Sit:
                        armatureComponent.animation.Play("sit_handl_hold");
                        break;
                }
            }
            else if (m_handRightRoot.GetComponentInChildren<BaseItem>() == null &&
            UtilityExtension.IsPointInsideQuadrilateral(cakeItem.HandPos, new Vector2[] { m_handRightRoot.A, m_handRightRoot.B, m_handRightRoot.C, m_handRightRoot.D }))
            {
                Debug.Log("RightHand CharacterItem");
                _AttachItem(m_handRightRoot.transform, cakeItem.transform);

                switch (m_state)
                {
                    case CharacterState.Stand:
                    case CharacterState.Hi:
                        armatureComponent.animation.Play("handr_hold");
                        break;
                    case CharacterState.Sit:
                        armatureComponent.animation.Play("sit_handr_hold");
                        break;
                }
            }
            else
            {
                Debug.Log("Outside CharacterItem");
                // _AttachItem(cushionItem.ParentAtInit, cushionItem.transform);
            }
        }
    }
    void _AttachItem(Transform parent_, Transform itemTrans_)
    {
        itemTrans_.parent = parent_;
        itemTrans_.localPosition = Vector3.zero;
        // itemTrans_.localRotation = Quaternion.identity;
        // itemTrans_.localScale = Vector3.one;
    }
    void _PlayHiAnimation()
    {
        if (m_state.Equals(CharacterState.Stand))
        {
            // Play the "Hi" animation
            armatureComponent.animation.Play("Hi", 1);
            // Reset the timer
            m_hiTimer = 0f;
        }
    }
}

public enum CharacterState
{
    Stand,
    IsDragging,
    Hi,
    Sit
}