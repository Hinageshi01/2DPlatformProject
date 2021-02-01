using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFrog : MonoBehaviour
{
    public Transform leftPoint, rightPoint;
    public float speed;

    private Rigidbody2D body;
    private float leftX, rightX;
    private bool isFaceLeft;
    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        leftX = leftPoint.position.x;
        rightX = rightPoint.position.x;
        Destroy(leftPoint.gameObject);
        Destroy(rightPoint.gameObject);
    }

    void Update()
    {
        movement();
    }

    void movement() {
        if (isFaceLeft) {//朝左
            body.velocity = new Vector2(-speed, body.velocity.y);//移动
            if (transform.position.x <= leftX) {
                isFaceLeft = false;
                transform.localScale = new Vector3(-1, 1, 1);//转身
            }
        }
        else {//朝右
            body.velocity = new Vector2(speed, body.velocity.y);
            if (transform.position.x >= rightX) {
                isFaceLeft = true;
                transform.localScale = new Vector3(1, 1, 1);
            }
        }
    }
}
