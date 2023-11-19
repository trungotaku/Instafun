using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClothingSlot : MonoBehaviour
{
    [SerializeField] ClothingItem m_clothingItem;
    public ClothingItem ClothingItem => m_clothingItem;
}
