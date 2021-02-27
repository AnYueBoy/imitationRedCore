/*
 * @Author: l hy 
 * @Date: 2021-02-06 16:14:00 
 * @Description: 球
 */

using UFrameWork.Application;
using UnityEngine;
public class Ball : MonoBehaviour {

    public Transform node {
        get {
            return this.transform;
        }
    }

    public Transform arrowTransform = null;

    public void colliderEnter (IObstacle obstacle) {
        if (obstacle == null) {
            return;
        }
        ItemType itemType = obstacle.GetItemType ();
        if (itemType == ItemType.ENEMY) {
            BaseEnemy enemy = obstacle as BaseEnemy;
            enemy.die ();
            // ModuleManager.instance.enemyManager.recycleEnemy (obstacle as BaseEnemy);
        }
    }

}