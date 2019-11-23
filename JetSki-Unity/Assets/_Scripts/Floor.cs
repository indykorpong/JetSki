using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Floor : MonoBehaviour
{
    public GameObject floor;
    
    public float Speed = 1;
    public bool finished = false;

    public bool spawnObstacle = true;
    public List<GameObject> obstacles;
    public List<float> obstacleOffsets;

    public Collider2D coll()
    {
        return gameObject.GetComponent<Collider2D>();
    }

    public Collider2D coll3(int obstacleIndex)
    {
        return obstacles[obstacleIndex].GetComponent<Collider2D>();
    }

    private void Start()
    {
        //Instantiate(floor, transform.position + new Vector3(coll().bounds.size.x, 0, 0), Quaternion.identity);
        //transform.position = floor.transform.position + new Vector3(coll().bounds.extents.x + coll2().bounds.extents.x, 0, 0);

    }

    public void FixedUpdate()
    {
        transform.Translate(Vector3.left * Speed * Time.deltaTime);
        Vector3 ScreenPos = Camera.main.WorldToViewportPoint(transform.position + new Vector3(coll().bounds.extents.x,0,0));

        if(ScreenPos.x < 1)
        {
           
            if (!finished)
            {
                //float Offsetx = Random.Range(2, 6);
                GameObject tempFloor = Instantiate(floor, transform.position + new Vector3(coll().bounds.extents.x, 0, 0), Quaternion.identity);

                Collider2D coll2 = tempFloor.GetComponent<Collider2D>();

                tempFloor.transform.position += new Vector3(coll2.bounds.extents.x,0);

                // Obstacle Spawning
                if (spawnObstacle)
                {
                    int idx = Random.Range(0, obstacleOffsets.Count);
                    Instantiate(obstacles[Random.Range(0, obstacles.Count)], tempFloor.transform.position + new Vector3(obstacleOffsets[idx], 1.36f, 0), Quaternion.identity, tempFloor.transform);
                }

                finished = true;
            }
        }

        if(ScreenPos.x < 0)
        {
            Destroy(gameObject);            
        }
    }
}
