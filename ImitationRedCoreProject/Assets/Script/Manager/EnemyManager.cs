/*
 * @Author: l hy 
 * @Date: 2021-02-20 19:23:21 
 * @Description: 敌人管理
 */

using System.Collections.Generic;
using UFramework.GameCommon;
using UFrameWork.Application;
using UnityEngine;

public class EnemyManager : MonoBehaviour, IModule {

    public Transform enemyParent;

    private HashSet<BaseEnemy> enemySet;

    public void init () {
        enemySet = new HashSet<BaseEnemy> ();
        // FIXME: 创建临时敌人
        this.spawnEnemy (EnemyType.SINGLE_CANNON, new Vector3 (0, 0, -8));
    }

    public void localUpdate (float dt) {
        foreach (BaseEnemy enemy in enemySet) {
            if (enemy == null) {
                continue;
            }

            if (!enemy.gameObject.activeSelf) {
                continue;
            }

            enemy.localUpdate (dt);
        }
    }

    public void spawnEnemy (EnemyType enemyType, Vector3 enemyPos) {
        string assetUrl = this.getEnemyUrlByType (enemyType);
        GameObject enemyPrefab = ModuleManager.instance.assetsManager.getAssetByUrlSync<GameObject> (assetUrl);
        GameObject enemyNode = ObjectPool.instance.requestInstance (enemyPrefab);
        enemyNode.transform.parent = this.enemyParent;

        enemyNode.transform.localPosition = enemyPos;

        BaseEnemy enemy = enemyNode.GetComponent<BaseEnemy> ();
        enemy.init ();
        this.enemySet.Add (enemy);
    }

    public void recycleEnemy (BaseEnemy enemy) {
        if (this.enemySet.Contains (enemy)) {
            ObjectPool.instance.returnInstance (enemy.transform.gameObject);
            this.enemySet.Remove (enemy);
        }
    }

    private string getEnemyUrlByType (EnemyType enemyType) {
        switch (enemyType) {
            case EnemyType.SINGLE_CANNON:
                return AssetUrlEnum.singleCannonUrl;
            default:
                Debug.LogError ("error enemyType: " + enemyType);
                return "";
        }
    }

}