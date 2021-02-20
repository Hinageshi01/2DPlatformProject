using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuBackGroundLoop : MonoBehaviour
{
    public Transform startPoint, endPoint;
    public float moveSpeed;

    private float startX, endX;
    void Start() {
        startX = startPoint.position.x;
        endX = endPoint.position.x;
    }
    void Update(){
        transform.position = new Vector2(transform.position.x - moveSpeed, transform.position.y);
        if (transform.position.x < endX) {
            transform.position = new Vector2(startX, transform.position.y);
        }
    }
}
