using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("status")]
    public float Damage;
    public float speed;
    public int PierceCount = 1;
    public Rigidbody2D rd;
    [Range(0, 0.3f)]
    public float excess;

    // Start is called before the first frame update
    void Start()
    {
        rd.AddForce(Vector3.right * speed);
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
