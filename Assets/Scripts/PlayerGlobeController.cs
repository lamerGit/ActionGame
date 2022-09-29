using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGlobeController : MonoBehaviour
{
    RectTransform hpGlove;
    private void Awake()
    {
        hpGlove=transform.GetChild(0).transform.GetChild(0).GetComponent<RectTransform>();
    }

    public void GloveChange(float ratio)
    {
        hpGlove.localPosition = new Vector3(0, hpGlove.rect.height * ratio+10.0f - hpGlove.rect.height, 0);
    }
}
