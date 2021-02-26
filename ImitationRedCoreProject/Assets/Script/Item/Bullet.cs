/*
 * @Author: l hy 
 * @Date: 2021-02-26 07:36:39 
 * @Description: 子弹
 * @Last Modified by: l hy
 * @Last Modified time: 2021-02-26 08:01:43
 */

using UFramework.GameCommon;
using UnityEngine;
public class Bullet : MonoBehaviour, IObstacle {
    public ItemType GetItemType () {
        return ItemType.BULLET;
    }

    public void localUpdate (float dt) {
        this.move (dt);
        this.existTime (dt);
    }

    private void move (float dt) {
        this.transform.Translate (Vector3.forward * dt * ConstValue.bulletSpeed);
    }

    private float existTimer = 0;

    private void existTime (float dt) {
        this.existTimer += dt;
        if (existTimer < ConstValue.bulletExistTime) {
            return;
        }

        ObjectPool.instance.returnInstance (gameObject);
    }

    private void OnCollisionEnter (Collision other) {
        if (other == null) {
            return;
        }
        Ball curBall = other.transform.GetComponent<Ball> ();
        if (curBall == null) {
            return;
        }

        // TODO: game over
    }
}