using System.Collections;
using System.Collections.Generic;
using Constants;
using UnityEngine;
using UnityEngine.EventSystems;
using DragonBones;
using UnityEngine.Rendering;

public class CharacterItem : BaseItem, IDragHandler, IEndDragHandler, IBeginDragHandler
{
    [SerializeField] UnityEngine.Transform footRoot;
    [SerializeField] UnityEngine.Transform sofaRoot;
    [SerializeField] UnityArmatureComponent armatureComponent;

    public float FootPosY => footRoot.position.y;
    public Vector2 SofaPos => sofaRoot.position;

    private bool m_isDrag = false;

    Vector3 m_deltaPos = Vector3.zero;

    private CharacterState m_state = CharacterState.Stand;

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
        Messenger.Broadcast(GameEvent.ON_END_DRAG_ITEM, (BaseItem) this);
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
}

public enum CharacterState
{
    Stand,
    IsDragging,
    Hi,
    Sit
}