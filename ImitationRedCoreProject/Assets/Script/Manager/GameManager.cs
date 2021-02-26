/*
 * @Author: l hy 
 * @Date: 2021-02-26 09:24:20 
 * @Description: 游戏业务层管理
 */
using UFrameWork.Application;

public class GameManager : IModule {
    public void init () {
        ModuleManager.instance.inputManager.init ();
        ModuleManager.instance.ballManager.init ();
        ModuleManager.instance.cameraManager.init ();

        ModuleManager.instance.dataManager = new DataManager ();
        ModuleManager.instance.dataManager.init ();

        ModuleManager.instance.bulletManager = new BulletManager ();
        ModuleManager.instance.bulletManager.init ();

        ModuleManager.instance.enemyManager.init ();
    }

    public void localUpdate (float dt) {
        ModuleManager.instance.inputManager.localUpdate (dt);

        ModuleManager.instance.dataManager.localUpdate (dt);

        float gameSpeed = ModuleManager.instance.dataManager.inSideData.gameSpeed;

        ModuleManager.instance.ballManager.localUpdate (dt * gameSpeed);
        ModuleManager.instance.cameraManager.localUpdate (dt);
        ModuleManager.instance.enemyManager.localUpdate (dt * gameSpeed);
        ModuleManager.instance.bulletManager.localUpdate (dt);
    }

    public void localLateUpdate (float dt) {
        ModuleManager.instance.cameraManager.localLateUpdate (dt);
    }
}