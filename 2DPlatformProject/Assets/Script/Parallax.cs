using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    public new Transform camera;
    public float moveRate;
    public bool lockY;

    private float bgStartX, bgStartY;
    void Start(){
        bgStartX = transform.position.x;
        bgStartY = transform.position.y;
    }
    void Update(){
        if (lockY) {
            transform.position = new Vector2(bgStartX + camera.position.x * moveRate, transform.position.y);
        }
        else {
            transform.position = new Vector2(bgStartX + camera.position.x, bgStartY + camera.position.y) * moveRate;
        }
    }
}
 