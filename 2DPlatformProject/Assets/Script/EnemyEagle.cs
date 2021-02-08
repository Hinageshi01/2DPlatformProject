using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyEagle : MonoBehaviour
{
    public Transform topPoint, bottomPoint;
    public float speed;

    private Rigidbody2D body;
    private Collider2D collisionBox;
    private float topY, bottomY;
    private bool isUp = true;
    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        collisionBox = GetComponent<Collider2D>();
        topY = topPoint.position.y;
        bottomY = bottomPoint.position.y;
        Destroy(topPoint.gameObject);
        Destroy(bottomPoint.gameObject);
    }

    void Update()
    {
        movement();
    }
    void movement() {
        if (isUp) {
            body.velocity = new Vector2(body.velocity.x, speed);
            if (transform.position.y >= topY) {
                isUp = false;
            }
        }
        else {
            body.velocity = new Vector2(body.velocity.x, -speed);
            if (transform.position.y <= bottomY) {
                isUp = true;
            }
        }
    }
}
