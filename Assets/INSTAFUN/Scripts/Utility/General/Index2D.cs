using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct Index2D
{
    public int x;
    public int y;

    public Index2D(int x_, int y_)
    {
        x = x_;
        y = y_;
    }

    public void Set(int x_, int y_)
    {
        x = x_;
        y = y_;
    }

    public void Add(Index2D delta_)
    {
        x += delta_.x;
        y += delta_.y;
    }

    public void Minus(Index2D delta_)
    {
        x -= delta_.x;
        y -= delta_.y;
    }
}