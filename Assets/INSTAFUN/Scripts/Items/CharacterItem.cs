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
    [SerializeField] Transform a;
    [SerializeField] Transform b;
    [SerializeField] Transform c;
    [SerializeField] Transform d;

    public Vector2 A => a.position;
    public Vector2 B => b.position;
    public Vector2 C => c.position;
    public Vector2 D => d.position;

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
        if (m_state.Equals(CharacterState.Stand) && m_hiTimer >= 10f)
        {
            // Play the "Hi" animation
            armatureComponent.animation.Play("Hi", 1);
            // Reset the timer
            m_hiTimer = 0f;
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
            if (UtilityExtension.IsPointInsideQuadrilateral(clothingItem.Center, new Vector2[] { A, B, C, D }))
            {
                Debug.Log("Inside");
                clothingItem.ClothingSlot.gameObject.SetActive(false);
                clothingPanel.EnableSlotByCharacterSkin(m_currentCharacterSkin);
                ChangeSkin(clothingItem.CharacterSkin);
            }
            else
            {
                Debug.Log("Outside");
                clothingItem.gameObject.LeanMoveLocal(Vector3.zero, 0.125f).setEaseOutQuad();
            }
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