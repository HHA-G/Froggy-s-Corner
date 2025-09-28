using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class EnemyShooting : MonoBehaviour
{
    public GameObject bullet;
    public Transform bulletPos;

    private float Timer;
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
        Debug.Log(distance);

        if (distance < 12)
        {
            Timer += Time.deltaTime;

        }
        if (Timer > 2)
        {
            Timer = 0;
            shoot();
        }
    }
    
    void shoot()
    {
        GameObject newBullet = Instantiate(bullet, bulletPos.position, Quaternion.identity);
        Collision collisionScript = newBullet.GetComponent<Collision>();
        if (collisionScript != null && player != null)
        {
            collisionScript.playerHealth = player.GetComponent<HealthSystem>();
        }
    }
}
