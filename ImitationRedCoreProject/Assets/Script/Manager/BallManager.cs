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

public class BallManager : MonoBehaviour {

    public LayerMask layerMask;

    public Transform ballParent = null;

    /* 路径点 */
    private List<Vector3> pathList = new List<Vector3> ();

    private readonly float moveSpeed = 1;

    private Vector3 moveDir = Vector3.zero;

    private Ball currentBall = null;

    public void init () {
        GameObject ballPrefab = ApplicationManager.instance.assetsManager.getAssetByUrlSync<GameObject> (AssetUrlEnum.ballUrl);
        GameObject ballNode = ObjectPool.getInstance ().requestInstance (ballPrefab);
        ballNode.transform.SetParent (this.ballParent);
        ballNode.transform.position = new Vector3 (2.03f, 0, 0.235f);
        currentBall = ballNode.GetComponent<Ball> ();
    }

    public void localUpdate () {
        move ();
    }

    private void move () {
        moveDir = ApplicationManager.instance.inputManager.moveDir;
        currentBall.transform.Translate (moveDir * moveSpeed * Time.deltaTime);
    }

    public void getReflectPath (Vector3 moveDir, float reflectDistance, List<Vector3> pathList, LayerMask layerMask) {
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