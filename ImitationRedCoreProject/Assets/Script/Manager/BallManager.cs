/*
 * @Author: l hy 
 * @Date: 2021-02-04 10:20:04 
 * @Description: 球体移动逻辑
 */

using System.Collections;
using System.Collections.Generic;
using UFramework.GameCommon;
using UFrameWork.Application;
using UnityEngine;

public class BallManager : MonoBehaviour, IModule {

    public LayerMask layerMask;

    public Transform ballParent = null;

    /* 球行走路径点 */
    private List<HitPointInfo> curPathList = new List<HitPointInfo> ();

    /* 产生的路径点 */
    private List<HitPointInfo> generatePathList = new List<HitPointInfo> ();

    [HideInInspector]
    public Ball currentBall = null;

    private List<GameObject> arrowNodeList = new List<GameObject> ();

    public void init () {
        GameObject ballPrefab = ModuleManager.instance.assetsManager.getAssetByUrlSync<GameObject> (AssetUrlEnum.ballUrl);
        GameObject ballNode = ObjectPool.instance.requestInstance (ballPrefab);
        ballNode.transform.SetParent (this.ballParent);
        ballNode.transform.position = new Vector3 (2.03f, 0, 0.235f);
        currentBall = ballNode.GetComponent<Ball> ();

        // FIXME: 事件没有移除
        ListenerManager.instance.add (EventEnum.refreshPathList, this, this.refreshPathList);
    }

    private void refreshPathList () {
        // 回收箭头
        this.recycleArrow ();

        if (this.generatePathList.Count <= 0) {
            return;
        }

        this.curPathList.Clear ();

        foreach (HitPointInfo hitPointInfo in this.generatePathList) {
            this.curPathList.Add (hitPointInfo);
        }
        this.pointIndex = 1;
    }

    public void localUpdate (float dt) {
        this.autonomyGenPath ();
        this.ballMove (dt);
    }

    private void autonomyGenPath () {
        if (ModuleManager.instance.inputManager.isTouch) {
            this.getReflectPath (ModuleManager.instance.inputManager.aimDir, ConstValue.reflectDis, this.generatePathList, this.layerMask);

            this.recycleArrow ();

            // 产生箭头
            this.generateArrow ();
        }
    }

    private void recycleArrow () {
        foreach (GameObject arrowNode in this.arrowNodeList) {
            ObjectPool.instance.returnInstance (arrowNode);
        }
    }

    private void generateArrow () {
        GameObject arrowPrefab = ModuleManager.instance.assetsManager.getAssetByUrlSync<GameObject> (AssetUrlEnum.arrowUrl);
        for (int i = 0; i < this.generatePathList.Count - 1; i++) {
            Vector3 startPathPos = this.generatePathList[i].hitPos;
            Vector3 nextPathPos = this.generatePathList[i + 1].hitPos;

            Vector3 diffVec = nextPathPos - startPathPos;
            float totalDis = diffVec.magnitude;
            Vector3 arrowDir = diffVec.normalized;
            int intervalIndex = 0;
            float angle = Vector3.Angle (Vector3.forward, arrowDir);
            if (arrowDir.x > 0) {
                angle = -angle;
            }

            Vector3 endPos = Vector3.zero;

            while (totalDis > 0) {
                endPos = startPathPos + arrowDir * (intervalIndex * ConstValue.arrowInterval);
                if (totalDis < ConstValue.arrowInterval) {
                    endPos = startPathPos + arrowDir * ((intervalIndex - 1) * ConstValue.arrowInterval + totalDis);
                }
                intervalIndex++;
                totalDis -= ConstValue.arrowInterval;
                GameObject arrowNode = ObjectPool.instance.requestInstance (arrowPrefab);
                this.arrowNodeList.Add (arrowNode);

                arrowNode.transform.parent = currentBall.arrowTransform;
                arrowNode.transform.position = endPos;
                arrowNode.transform.localEulerAngles = new Vector3 (0, 0, angle);
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

        HitPointInfo targetHitInfo = this.curPathList[pointIndex];
        Vector3 targetPos = targetHitInfo.hitPos;
        Vector3 targetVec = targetPos - currentBall.transform.position;
        Vector3 targetDir = targetVec.normalized;

        if (targetVec.magnitude < 0.3) {
            pointIndex++;
            this.currentBall.colliderEnter (targetHitInfo.hitObstacle);
            if (pointIndex >= this.curPathList.Count) {
                // 重新产生路径
                this.getReflectPath (targetDir, ConstValue.reflectDis, this.curPathList, this.layerMask);
                this.pointIndex = 1;
            }
        }

        currentBall.transform.Translate (targetDir * ConstValue.ballMoveSpeed * dt);
    }

    private void getReflectPath (Vector3 moveDir, float reflectDistance, List<HitPointInfo> pathList, LayerMask layerMask) {
        pathList.Clear ();
        Vector3 startPos = currentBall.transform.position;
        HitPointInfo startHitInfo = new HitPointInfo (startPos, null);
        pathList.Add (startHitInfo);
        while (reflectDistance > 0) {
            RaycastHit raycastHit;
            if (!Physics.Raycast (startPos, moveDir, out raycastHit, reflectDistance, layerMask)) {
                break;
            }
            HitPointInfo hitPointInfo = new HitPointInfo (raycastHit.point, raycastHit.collider.GetComponent<IObstacle> ());
            pathList.Add (hitPointInfo);
            reflectDistance -= (raycastHit.point - startPos).magnitude;

            startPos = raycastHit.point;
            moveDir = Vector3.Reflect (moveDir, raycastHit.normal);
        }

        if (reflectDistance > 0) {
            Vector3 endPoint = startPos + moveDir * reflectDistance;
            HitPointInfo endHitInfo = new HitPointInfo (endPoint, null);
            pathList.Add (endHitInfo);
        }
    }

    public void recycleBall () {
        ObjectPool.instance.returnInstance (this.currentBall.gameObject);
    }
}