/*
 * @Author: l hy 
 * @Date: 2021-02-22 15:20:52 
 * @Description: 碰撞信息
 */
using UnityEngine;
public class HitPointInfo {

    public Vector3 hitPos;

    public IGameObject hitObstacle;

    public HitPointInfo (Vector3 hitPos, IGameObject hitObstacle) {
        this.hitPos = hitPos;
        this.hitObstacle = hitObstacle;
    }
}