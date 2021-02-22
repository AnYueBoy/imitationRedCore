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

    private void OnCollisionEnter (Collision other) {
        IObstacle obstacle = other.gameObject.GetComponent<IObstacle> ();
        ItemType itemType = obstacle.GetItemType ();
        if (itemType == ItemType.ENEMY) {
            ModuleManager.instance.enemyManager.recycleEnemy (obstacle as BaseEnemy);
        }
    }
}