using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFly : MonoBehaviour
{
    public float speed;
    private GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        float distance = Vector2.Distance(transform.position, player.transform.position);
        if (distance < 12)
        {
            chase();
            flip();
        }
    }
    private void chase()
    {
        transform.position = Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
    }
    private void flip()
    {
        if (transform.position.x < player.transform.position.x)
        {
            transform.localScale = new Vector3(-0.7f, 0.7f, 0.7f);
        }
        else
        {
            transform.localScale = new Vector3(0.7f, 0.7f, 0.7f);
        }
    }
}
