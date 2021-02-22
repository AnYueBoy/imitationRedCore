/*
 * @Author: l hy 
 * @Date: 2021-02-20 09:38:40 
 * @Description: 敌人基类
 */

using System.Collections;
using System.Collections.Generic;
using UFrameWork.Application;
using UnityEngine;

public class BaseEnemy : MonoBehaviour {

    protected void OnCollisionEnter (Collision other) {

    }

    protected float lockAtInterval = 0.5f;

    protected float shootInterval = 0.8f;

    public void localUpdate (float dt) {
        this.rotateToBall (dt);
    }

    private void rotateToBall (float dt) {
        Transform ballTrans = ModuleManager.instance.ballManager.currentBall.transform;
        Vector3 targetVec = this.gameObject.transform.position - ballTrans.position;
        Vector3 targetDir = targetVec.normalized;

        float angle = Vector3.Angle (Vector3.forward, targetDir);
        if (targetDir.x > 0) {
            angle = -angle;
        }
        this.gameObject.transform.localEulerAngles = new Vector3 (0, angle, 0);
    }

}