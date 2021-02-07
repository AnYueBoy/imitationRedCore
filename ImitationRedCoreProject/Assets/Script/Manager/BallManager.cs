/*
 * @Author: l hy 
 * @Date: 2021-02-04 10:20:04 
 * @Description: 球体移动逻辑
 */

using System.Collections;
using System.Collections.Generic;
using UFramework.GameCommon;
using UnityEngine;

public class BallManager : MonoBehaviour {

    public LayerMask layerMask;

    public Transform ballParent = null;

    /* 路径点 */
    private List<Vector3> pathList = new List<Vector3> ();

    private readonly float moveSpeed = 1;

    [HideInInspector]
    public Ball currentBall = null;

    public void init () {
        GameObject ballPrefab = AssetsManager.instance.getAssetByUrlSync<GameObject> (AssetUrlEnum.ballUrl);
        GameObject ballNode = ObjectPool.instance.requestInstance (ballPrefab);
        ballNode.transform.SetParent (this.ballParent);
        ballNode.transform.position = new Vector3 (2.03f, 0, 0.235f);
        currentBall = ballNode.GetComponent<Ball> ();
    }

    private Vector3 preMoveDir = Vector3.zero;

    public void localUpdate () {
        if (InputManager.instance.isTouch && InputManager.instance.moveDir != preMoveDir) {
            this.preMoveDir = InputManager.instance.moveDir;
            this.getReflectPath (this.preMoveDir, ConstValue.reflectDis, this.pathList, this.layerMask);
            this.pointIndex = 1;
        }
        move ();
    }

    private int pointIndex = 1;
    private void move () {
        if (pointIndex >= this.pathList.Count) {
            return;
        }
        Vector3 targetPos = this.pathList[pointIndex];
        Vector3 targetVec = targetPos - currentBall.transform.position;
        Vector3 targetDir = targetVec.normalized;

        if (targetVec.magnitude < 10) {
            pointIndex++;
            if (pointIndex >= this.pathList.Count) {
                // 重新产生路径
                this.getReflectPath (targetDir, ConstValue.reflectDis, this.pathList, this.layerMask);
                this.pointIndex = 1;
            }
        }

        currentBall.transform.Translate (targetDir * moveSpeed * Time.deltaTime);
    }

    private void getReflectPath (Vector3 moveDir, float reflectDistance, List<Vector3> pathList, LayerMask layerMask) {
        pathList.Clear ();
        Vector3 startPos = currentBall.transform.position;
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