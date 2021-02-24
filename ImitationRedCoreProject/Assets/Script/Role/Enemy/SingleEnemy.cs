/*
 * @Author: l hy 
 * @Date: 2021-02-22 14:07:18 
 * @Description: 单炮筒炮台
 */
using UnityEngine;
public class SingleEnemy : BaseEnemy {

    public Transform barreTrans;

    private float aimTimer = 0;
    private float aimInterval = 0.8f;

    public void localUpdate (float dt) {
        base.localUpdate (dt);

        this.aim (dt);
    }

    private void aim (float dt) {
        this.aimTimer += dt;
        if (this.aimTimer < this.aimInterval) {
            return;
        }

        this.aimTimer = 0;
        this.attack ();
    }

    private void attack () {

    }
}