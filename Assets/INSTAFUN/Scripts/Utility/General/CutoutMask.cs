using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;

public class CutoutMask : Image
{
    public override Material materialForRendering
    {
        get
        {
            Material result = new Material(base.materialForRendering);
            result.SetInt("_StencilComp", (int)CompareFunction.NotEqual);
            return result;
        }
    }
}
