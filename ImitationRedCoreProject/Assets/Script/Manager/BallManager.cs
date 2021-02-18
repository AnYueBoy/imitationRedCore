﻿/*
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

    /* 球行走路径点 */
    private List<Vector3> curPathList = new List<Vector3> ();

    /* 产生的路径点 */
    private List<Vector3> generatePathList = new List<Vector3> ();

    private readonly float moveSpeed = 1;

    [HideInInspector]
    public Ball currentBall = null;

    /* 测试逻辑 */
    public LineRenderer lineRenderer = null;

    public void init () {
        GameObject ballPrefab = AssetsManager.instance.getAssetByUrlSync<GameObject> (AssetUrlEnum.ballUrl);
        GameObject ballNode = ObjectPool.instance.requestInstance (ballPrefab);
        ballNode.transform.SetParent (this.ballParent);
        ballNode.transform.position = new Vector3 (2.03f, 0, 0.235f);
        currentBall = ballNode.GetComponent<Ball> ();

        // FIXME: 事件没有移除
        ListenerManager.instance.add (EventEnum.refreshPathList, this, this.refreshPathList);
    }

    private void refreshPathList () {
        if (this.generatePathList.Count <= 0) {
            return;
        }

        this.curPathList.Clear ();

        foreach (Vector3 pathPos in this.generatePathList) {
            this.curPathList.Add (pathPos);
        }
        this.pointIndex = 1;
    }

    private Vector3 preMoveDir = Vector3.zero;

    public void localUpdate (float dt) {
        this.autonomyGenPath ();
        this.ballMove (dt);
    }

    private void autonomyGenPath () {
        if (InputManager.instance.isTouch && InputManager.instance.aimDir != preMoveDir) {
            this.preMoveDir = InputManager.instance.aimDir;
            this.getReflectPath (this.preMoveDir, ConstValue.reflectDis, this.generatePathList, this.layerMask);

            this.lineRenderer.positionCount = this.generatePathList.Count;
            this.lineRenderer.SetPositions (this.generatePathList.ToArray ());

            // 产生箭头
            this.generateArrow ();
        }
    }

    private void generateArrow () {
        for (int i = 0; i < this.generatePathList.Count - 1; i++) {
            Vector3 startPathPos = this.generatePathList[i];
            Vector3 nextPathPos = this.generatePathList[i + 1];

            GameObject arrowPrefab = AssetsManager.instance.getAssetByUrlSync<GameObject> (AssetUrlEnum.arrowUrl);

            Vector3 diffVec = nextPathPos - startPathPos;
            float totalDis = diffVec.magnitude;
            Vector3 arrowDir = diffVec.normalized;
            int intervalIndex = 0;

            while (totalDis > 0) {
                Vector3 endPos = startPathPos + arrowDir * (intervalIndex * ConstValue.arrowInterval);
                if (totalDis < ConstValue.arrowInterval) {
                    endPos = startPathPos + arrowDir * (intervalIndex * (ConstValue.arrowInterval - 1) + totalDis);
                }
                intervalIndex++;
                totalDis -= ConstValue.arrowInterval;
                GameObject arrowNode = ObjectPool.instance.requestInstance (arrowPrefab);
                float angle = Vector3.Angle (Vector3.right, endPos);

                arrowNode.transform.parent = currentBall.arrowTransform;
                arrowNode.transform.position = endPos;
                arrowNode.transform.eulerAngles = new Vector3 (0, 0, angle);
            }
        }
    }

    private int pointIndex = 1;
    private void ballMove (float dt) {
        if (this.curPathList.Count <= 0) {
            return;
        }

        // 到达路径终点
        if (pointIndex >= this.curPathList.Count) {
            return;
        }

        Vector3 targetPos = this.curPathList[pointIndex];
        Vector3 targetVec = targetPos - currentBall.transform.position;
        Vector3 targetDir = targetVec.normalized;

        if (targetVec.magnitude < 0.3) {
            pointIndex++;
            if (pointIndex >= this.curPathList.Count) {
                // 重新产生路径
                Debug.Log ("ReGenerate Points");
                this.getReflectPath (targetDir, ConstValue.reflectDis, this.curPathList, this.layerMask);
                this.pointIndex = 1;
            }
        }

        currentBall.transform.Translate (targetDir * moveSpeed * dt);
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