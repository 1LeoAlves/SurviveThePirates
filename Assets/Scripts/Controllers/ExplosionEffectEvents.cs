using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionEffectEvents : MonoBehaviour
{
    public void DestroyItSelf()
    {
        Destroy(gameObject);
    }
}
