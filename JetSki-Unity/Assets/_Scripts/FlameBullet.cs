using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlameBullet : MonoBehaviour
{
    [Header("status")]
    public float Damage = 10;
    [Range(0, 0.3f)]
    public float excess;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Vector3 screenPoint = Camera.main.WorldToViewportPoint(transform.position);
        if (screenPoint.x < 1 + excess && screenPoint.x > 0 - excess && screenPoint.y < 1 + excess && screenPoint.y > 0 - excess)
        {

        }
        else
        {
            Destroy(gameObject);
        }
    }
}
