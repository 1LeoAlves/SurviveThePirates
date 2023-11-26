using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadersEvents : MonoBehaviour
{
    public void DestroyItSelf()
    {
        Destroy(transform.parent.gameObject);
    }
}
