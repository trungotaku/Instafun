using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class UnityExtension
{
    public static IEnumerator DoDelayOneFrame(Action cb_)
    {
        yield return null;
        cb_?.Invoke();
    }
}