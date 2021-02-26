/*
 * @Author: l hy 
 * @Date: 2021-02-26 07:22:26 
 * @Description: 子弹管理
 * @Last Modified by: l hyy
 * @Last Modified time: 2021-02-26 07:39:327
 */

using System.Collections.Generic;
using UFramework.GameCommon;
using UFrameWork.Application;
using UnityEngine;
public class BulletManager : MonoBehaviour, IModule {

    public Transform bulletParent;

    private HashSet<Bullet> bullets = new HashSet<Bullet> ();

    public void init () {

    }

    public void localUpdate (float dt) {
        foreach (Bullet bullet in this.bullets) {
            if (bullet == null) {
                continue;
            }

            if (!bullet.gameObject.activeSelf) {
                continue;
            }
            bullet.localUpdate (dt);
        }
    }

    public void spawnBullet (List<Transform> barrelTransList) {
        GameObject barrelPrefab = ModuleManager.instance.assetsManager.getAssetByUrlSync<GameObject> (AssetUrlEnum.bulletUrl);
        foreach (Transform barrelTrans in barrelTransList) {
            GameObject barrelNode = ObjectPool.instance.requestInstance (barrelPrefab);
            Vector3 barrelWorldPos = barrelTrans.TransformPoint (Vector3.zero);
            barrelNode.transform.SetParent (this.bulletParent);
            barrelNode.transform.position = barrelWorldPos;
            Vector3 curBallPos = ModuleManager.instance.ballManager.currentBall.transform.position;
            Vector3 moveDir = new Vector3 (curBallPos.x, 0, curBallPos.z) - new Vector3 (barrelTrans.position.x, 0, barrelTrans.position.z);
            Bullet bullet = barrelNode.GetComponent<Bullet> ();
            moveDir = moveDir.normalized;
            bullet.init (moveDir);
            bullets.Add (bullet);
        }
    }
}