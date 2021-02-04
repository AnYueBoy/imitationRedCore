/*
 * @Author: l hy 
 * @Date: 2021-02-04 10:20:04 
 * @Description: 球体移动逻辑
 */

using System.Collections;
using System.Collections.Generic;
using UFrameWork.Application;
using UnityEngine;

public class BallMove : MonoBehaviour {

    private readonly float moveSpeed = 1;

    private Vector2 moveDir = Vector2.right;

    void Start () { }

    void Update () {
        move ();
    }

    private void move () {
        moveDir = ApplicationManager.instance.inputManager.moveDir;
        transform.Translate (new Vector3 (moveSpeed * moveDir.x * Time.deltaTime, 0, moveSpeed * moveDir.y * Time.deltaTime));
    }
}