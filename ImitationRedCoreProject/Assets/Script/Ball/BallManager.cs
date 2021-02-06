/*
 * @Author: l hy 
 * @Date: 2021-02-04 10:20:04 
 * @Description: 球体移动逻辑
 */

using System.Collections;
using System.Collections.Generic;
using UFrameWork.Application;
using UnityEngine;

public class BallManager : MonoBehaviour {

    [Header ("行进路径检测点")]
    public Transform travelPoint = null;

    public LayerMask layerMask;

    private readonly float moveSpeed = 1;

    /* 路径点 */
    private List<Vector3> pathList = new List<Vector3> ();

    private Vector3 moveDir = Vector3.zero;

    void Update () {
        move ();
    }

    private void move () {
        moveDir = ApplicationManager.instance.inputManager.moveDir;
        transform.Translate (moveDir * moveSpeed * Time.deltaTime);
    }

    public void getReflectPath (Vector3 moveDir, float reflectDistance, List<Vector3> pathList, LayerMask layerMask) {
        Vector3 startPos = this.travelPoint.position;
        pathList.Add (startPos);
        while (reflectDistance > 0) {
            RaycastHit raycastHit;
            if (!Physics.Raycast (startPos, moveDir, out raycastHit, reflectDistance, layerMask)) {
                break;
            }

            pathList.Add (raycastHit.point);
            reflectDistance -= (raycastHit.point - startPos).magnitude;

            startPos = raycastHit.point;
            moveDir = Vector3.Reflect (moveDir, raycastHit.normal);
        }

        if (reflectDistance > 0) {
            Vector3 endPoint = startPos + moveDir * reflectDistance;
            pathList.Add (endPoint);
        }
    }
}