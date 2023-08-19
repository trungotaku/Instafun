using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustormParticle : MonoBehaviour
{
    private void OnEnable()
    {
        this.GetComponent<ParticleSystem>().Play();
    }
}
