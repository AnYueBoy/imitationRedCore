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

    private List<BaseEnemy> enemyList;

    public void init () {
        enemyList = new List<BaseEnemy> ();
        // FIXME: 创建临时敌人
        this.spawnEnemy (EnemyType.SINGLE_CANNON, new Vector3 (0, 0, -8));
    }

    public void localUpdate (float dt) {
        foreach (BaseEnemy enemy in enemyList) {
            if (enemy == null) {
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
        this.enemyList.Add (enemy);
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