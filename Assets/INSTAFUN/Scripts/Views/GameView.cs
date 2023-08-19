using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameView : BaseView
{
    public override ViewId Id => ViewId.GameView;
    [SerializeField] Transform behindFloorLimitPos;
    [SerializeField] Transform frontFloorLimitPos;

    public float FloorLimitPosMaxY => behindFloorLimitPos.position.y;
    public float FloorLimitPosMinY => frontFloorLimitPos.position.y;
    public List<BaseItem> sortingItems = new List<BaseItem>();
}