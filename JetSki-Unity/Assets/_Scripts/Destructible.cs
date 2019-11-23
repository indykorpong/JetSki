using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destructible : MonoBehaviour
{
    GameObject player;
    Sprite sprite;
    Vector3 objExtents;

    [Header("Status")]
    public float life = 3;
    public float damage = 2;

    // Start is called before the first frame update
    void Start()
    {
        sprite = GetComponent<SpriteRenderer>().sprite;
        objExtents = sprite.bounds.extents;
    }

    // Update is called once per frame
    void Update()
    {
        /*
        if (GameObject.FindWithTag("Player") != null)
        {
            player = GameObject.FindWithTag("Player");
            Vector3 playerExtents = player.GetComponent<BoxCollider2D>().bounds.extents;

            if (Hit(player.transform.position, playerExtents, objExtents))
            {
                Debug.Log("you hit the " + name);
            }
        }
        */
    }
    
    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            //Debug.Log("you hit the " + name);
            collision.gameObject.GetComponent<Character>().SayTextHit();
            collision.gameObject.GetComponent<Character>().Hit = true;

            //! Reduce player health
            collision.gameObject.GetComponent<Character>().GetDamage(damage);
        }

        if(collision.gameObject.tag == "Bullet")
        {
            //Debug.Log("bullet hit!");
            Bullet bullet = collision.gameObject.GetComponent<Bullet>();

            //! Reduce Enemy health by bullet power
            life -= bullet.Damage;

            Destroy(collision.gameObject);

            if (life <= 0)
            {
                GameManager.UpdateScore(1);
                Destroy(gameObject);
            }
        }

        if (collision.gameObject.tag == "Flame")
        {
            Debug.Log("flame hit!");
            FlameBullet bullet = collision.gameObject.GetComponent<FlameBullet>();

            //! Reduce Enemy health by bullet power
            life -= bullet.Damage;

            Destroy(collision.gameObject);

            if (life <= 0)
            {
                GameManager.UpdateScore(1);
                Destroy(gameObject);
            }
        }
    }
}
