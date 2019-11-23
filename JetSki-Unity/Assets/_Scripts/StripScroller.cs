using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StripScroller : MonoBehaviour
{
    [Header("Start")]
    public float tileX;
    public float tileY;
    public float scrollX;
    public float scrollY;
    [Space(3)]
    public float Divide = 1;
    public float Speed = 1;
    public bool IgnoreSpeed = true;
    [Space(3)]
    public Vector2 StartOffset;
    public Material S_material;


    // Start is called before the first frame update
    void Start()
    {
        if (S_material == null)
        {
            S_material = GetComponent<Renderer>().material;
            StartOffset = S_material.mainTextureOffset;

            S_material.mainTextureScale = new Vector2(tileX, tileY);
        }
    }

    void FixedUpdate()
    {
        Vector2 offset = new Vector2((Time.deltaTime * scrollX / Divide) * Speed, (Time.deltaTime * scrollY / Divide) * Speed);
        S_material.mainTextureOffset += offset;
    }
}
