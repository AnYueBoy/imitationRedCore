/*
 * @Author: l hy 
 * @Date: 2021-02-26 07:36:39 
 * @Description: 子弹
 * @Last Modified by: l hy
 * @Last Modified time: 2021-02-27 14:14:52
 */

using UFramework.GameCommon;
using UFrameWork.Application;
using UnityEngine;
public class Bullet : MonoBehaviour, IGameObject {
    public ItemType GetItemType () {
        return ItemType.BULLET;
    }

    public void localUpdate (float dt) {
        this.move (dt);
    }

    private void move (float dt) {
        this.transform.Translate (Vector3.forward * dt * ConstValue.bulletSpeed);
    }

    private void OnTriggerEnter (Collider other) {
        if (other == null) {
            return;
        }
        IGameObject colliderNode = other.transform.GetComponent<IGameObject> ();
        ItemType itemType = colliderNode.GetItemType ();
        if (itemType == ItemType.INDESTRUCTIBLE) {
            ObjectPool.instance.returnInstance (gameObject);
            return;
        }

        if (itemType == ItemType.BALL) {
            // game over
            ModuleManager.instance.ballManager.recycleBall ();
            ModuleManager.instance.dataManager.inSideData.curGameState = GameState.GAME_OVER;
        }
    }
}