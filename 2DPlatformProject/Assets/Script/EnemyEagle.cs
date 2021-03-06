﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyEagle : Enemy
{
    public Transform topPoint, bottomPoint;
    public float speed;
    public bool isUp;

    private Rigidbody2D body;
    private float topY, bottomY;
    protected override void Start() {
        base.Start();
        body = GetComponent<Rigidbody2D>();
        topY = topPoint.position.y;
        bottomY = bottomPoint.position.y;
        Destroy(topPoint.gameObject);
        Destroy(bottomPoint.gameObject);
    }
    void Update() {
        movement();
    }
    void movement() {
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
