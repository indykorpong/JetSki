using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetPractice : MonoBehaviour
{
    public int score = 1; //defalut = 1

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Bullet")
        {
            FindObjectOfType<TextGenerator>().CreateText(transform.position, score.ToString(), DisplayDamage.TextState.Green, 30);
            Destroy(collision.gameObject);
        }
    }
}
