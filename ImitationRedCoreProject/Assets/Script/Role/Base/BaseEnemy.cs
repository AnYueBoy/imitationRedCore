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

    protected float attackTimer = 0;

    protected Material fillMaterial = null;

    public Material exposionMaterial;

    public MeshRenderer meshRenderer;

    public SpriteRenderer fillNode = null;

    public List<Transform> barrelList = new List<Transform> ();

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
        float fillValue = this.attackTimer / ConstValue.attackInterval;
        this.refreshFill (fillValue);
        if (this.attackTimer > ConstValue.attackInterval) {
            this.attackTimer = 0;
            // 攻击
            this.attackAction ();
        }
    }

    protected void refreshFill (float fillValue) {
        fillValue = Mathf.Min (fillValue, 1);
        this.fillMaterial.SetFloat ("_Fill", fillValue);
    }

    protected void attackAction () {
        ModuleManager.instance.bulletManager.spawnBullet (this.barrelList);
    }

    public ItemType GetItemType () {
        return ItemType.ENEMY;
    }

    public void die () {
        this.meshRenderer.material = this.exposionMaterial;
        this.exposionMaterial.SetFloat ("_StartTime", Time.timeSinceLevelLoad);
    }
}