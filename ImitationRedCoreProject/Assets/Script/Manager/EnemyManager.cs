/*
 * @Author: l hy 
 * @Date: 2021-02-20 19:23:21 
 * @Description: 敌人管理
 */

using System.Collections.Generic;
using UFrameWork.Application;

public class EnemyManager : IModule {

    private List<BaseEnemy> enemyList;

    public void init () {
        enemyList = new List<BaseEnemy> ();
    }

    public void localUpdate (float dt) {
        foreach (BaseEnemy enemy in enemyList) {
            if (enemy == null) {
                continue;
            }

            enemy.localUpdate (dt);
        }
    }

}