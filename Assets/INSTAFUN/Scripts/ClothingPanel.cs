using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClothingPanel : MonoBehaviour
{
    [SerializeField] List<ClothingSlot> m_slots;
    public List<ClothingSlot> Slots => m_slots;

    Dictionary<CharacterSkin, ClothingSlot> m_slotDict = new Dictionary<CharacterSkin, ClothingSlot>();

    public void Awake()
    {
        m_slots.ForEach(x =>
        {
            m_slotDict.Add(x.ClothingItem.CharacterSkin, x);
            x.Hang();
        });
        m_slots[0].PutOnCharacter();
    }

    public ClothingSlot GetSlotByCharacterSkin(CharacterSkin characterSkin)
    {
        return m_slotDict[characterSkin];
    }
}
