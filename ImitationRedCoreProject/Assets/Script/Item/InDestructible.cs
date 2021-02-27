/*
 * @Author: l hy 
 * @Date: 2021-02-27 13:58:17 
 * @Description: 不可破坏物体
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InDestructible : MonoBehaviour, IGameObject {
    public ItemType GetItemType () {
        return ItemType.INDESTRUCTIBLE;
    }
}