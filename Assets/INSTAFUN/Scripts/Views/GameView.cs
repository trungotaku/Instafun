using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameView : BaseView
{
    [SerializeField] Transform behindFloorLimitPos;
    [SerializeField] Transform frontFloorLimitPos;

    public float FloorLimitPosMaxY => behindFloorLimitPos.position.y;
    public float FloorLimitPosMinY => frontFloorLimitPos.position.y;
    public List<BaseItem> sortingItems = new List<BaseItem>();
}