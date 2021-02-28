using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobilePlatform : MonoBehaviour
{
    public Transform topPoint, bottomPoint;
    public float speed;
    public bool isUp;

    private Rigidbody2D body;
    private float topY, bottomY;
    void Start() {
        body = GetComponent<Rigidbody2D>();
        topY = topPoint.position.y;
        bottomY = bottomPoint.position.y;
        Destroy(topPoint.gameObject);
        Destroy(bottomPoint.gameObject);
    }
    void Update() {
        Movement();
    }
    void Movement() {
        if (isUp) {
            body.velocity = new Vector2(0, speed);
            if (transform.position.y >= topY) {
                isUp = false;
            }
        }
        else {
            body.velocity = new Vector2(0, -speed);
            if (transform.position.y <= bottomY) {
                isUp = true;
            }
        }
    }
}
