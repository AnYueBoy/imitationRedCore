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
        GameObject bulletPrefab = ModuleManager.instance.assetsManager.getAssetByUrlSync<GameObject> (AssetUrlEnum.bulletUrl);
        foreach (Transform barrelTrans in barrelTransList) {
            GameObject bulletNode = ObjectPool.instance.requestInstance (bulletPrefab);
            Vector3 bulletWorldPos = barrelTrans.position;
            Vector3 bulletWorldEuler = barrelTrans.eulerAngles;
            bulletNode.transform.SetParent (this.bulletParent);
            bulletNode.transform.position = bulletWorldPos;
            bulletNode.transform.eulerAngles = bulletWorldEuler;

            Bullet bullet = bulletNode.GetComponent<Bullet> ();
            bullets.Add (bullet);
        }
    }
}