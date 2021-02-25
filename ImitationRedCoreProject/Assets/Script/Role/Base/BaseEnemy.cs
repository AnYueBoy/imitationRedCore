/*
 * @Author: l hy 
 * @Date: 2021-02-20 09:38:40 
 * @Description: 敌人基类
 */

using System.Collections;
using System.Collections.Generic;
using UFrameWork.Application;
using UnityEngine;

public class BaseEnemy : MonoBehaviour, IObstacle {

    protected float attackInterval = 3f;

    protected float attackTimer = 0;

    protected Material fillMaterial = null;

    public SpriteRenderer fillNode = null;

    public void init () {
        this.fillMaterial = this.fillNode.material;
    }

    public void localUpdate (float dt) {
        this.rotateToBall (dt);
        this.attackCoolDown (dt);
    }

    private void rotateToBall (float dt) {
        Transform ballTrans = ModuleManager.instance.ballManager.currentBall.transform;
        Vector3 targetVec = this.gameObject.transform.position - ballTrans.position;
        Vector3 targetDir = targetVec.normalized;

        float angle = Vector3.Angle (Vector3.back, targetDir);
        if (targetDir.x > 0) {
            angle = -angle;
        }
        this.gameObject.transform.localEulerAngles = new Vector3 (0, angle, 0);
    }

    protected void attackCoolDown (float dt) {
        this.attackTimer += dt;
        float fillValue = this.attackTimer / this.attackInterval;
        this.refreshFill (fillValue);
        if (this.attackTimer > this.attackInterval) {
            this.attackTimer = 0;
            // TODO: 攻击逻辑
        }
    }

    protected void refreshFill (float fillValue) {
        fillValue = Mathf.Min (fillValue, 1);
        this.fillMaterial.SetFloat ("_Fill", fillValue);
    }

    public ItemType GetItemType () {
        return ItemType.ENEMY;
    }
}