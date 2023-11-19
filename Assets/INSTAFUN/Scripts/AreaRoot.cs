using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaRoot : MonoBehaviour
{
    [SerializeField] Transform a;
    [SerializeField] Transform b;
    [SerializeField] Transform c;
    [SerializeField] Transform d;

    public Vector2 A => a.position;
    public Vector2 B => b.position;
    public Vector2 C => c.position;
    public Vector2 D => d.position;
}
