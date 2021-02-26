/*
 * @Author: l hy 
 * @Date: 2021-02-22 09:16:57 
 * @Description: 模块管理
 */

namespace UFrameWork.Application {
    using UFramework.GameCommon;
    using UFrameWork.Develop;
    using UnityEngine;
    public class ModuleManager : MonoBehaviour {
        public static ModuleManager _instance;

        public static ModuleManager instance {
            get {
                return _instance;
            }
        }

        public AppMode appMode = AppMode.Developing;

        #region Mono 模块
        public InputManager inputManager;
        public BallManager ballManager;
        public CameraManager cameraManager;
        public EnemyManager enemyManager;

        #endregion

        #region 非Mono模块
        [HideInInspector]
        public GUIConsole guiConsole;

        [HideInInspector]
        public AssetsManager assetsManager;

        [HideInInspector]
        public DataManager dataManager;

        [HideInInspector]
        public BulletManager bulletManager;

        public GameManager gameManager;
        #endregion

        private void Awake () {
            _instance = this;
            if (appMode != AppMode.Release) {
                guiConsole = new GUIConsole ();
                // 图形控制面板初始化
                guiConsole.init ();
            }

            assetsManager = new AssetsManager ();

            gameManager = new GameManager ();
            gameManager.init ();
        }

        private void Update () {
            float dt = Time.deltaTime;
            if (guiConsole != null) {
                guiConsole.localUpdate (dt);
            }
            gameManager.localUpdate (dt);
        }

        private void LateUpdate () {
            float dt = Time.deltaTime;
            gameManager.localLateUpdate (dt);

        }

        private void OnGUI () {
            if (guiConsole != null) {
                guiConsole.drawGUI ();
            }
        }

        private void gameOnQuit () {
            if (guiConsole != null) {
                guiConsole.quit ();
            }
        }
    }

}