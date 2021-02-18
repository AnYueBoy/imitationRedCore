/*
 * @Author: l hy 
 * @Date: 2021-01-25 15:34:12 
 * @Description: 程序入口
 */
namespace UFrameWork.Application {
    using UFrameWork.Develop;
    using UnityEngine;

    public class ApplicationManager : MonoBehaviour {

        public AppMode appMode = AppMode.Developing;

        private GUIConsole guiConsole = new GUIConsole ();
        public CameraTrack cameraTrack = null;
        public BallManager ballManager = null;

        public InputManager inputManager = null;

        #region  程序生命周期函数
        private void Awake () {
            appLaunch ();
        }

        private void OnEnable () {

        }

        private void Start () {
            this.gameStart ();
        }

        private void Update () {
            this.gameUpdate (Time.deltaTime);
        }

        private void LateUpdate () {
            this.gameLateUpdate (Time.deltaTime);
        }

        private void OnGUI () {
            this.gameOnGUI ();
        }

        #endregion

        #region 启动与初始化

        private void appLaunch () {
            setResourceLoadType ();

            if (appMode != AppMode.Release) {
                // 图形控制面板初始化
                guiConsole.init ();
            }

            this.inputManager.init ();

            this.ballManager.init ();

            this.cameraTrack.init (ballManager.currentBall.transform, Vector3.zero);
        }

        private void gameStart () { }

        private void gameUpdate (float dt) {
            if (guiConsole != null) {
                guiConsole.localUpdate ();
            }

            this.inputManager.localUpdate ();
            this.ballManager.localUpdate (dt);
        }

        private void gameLateUpdate (float dt) {
            this.cameraTrack.localLateUpdate ();
        }

        private void gameOnGUI () {
            if (guiConsole != null) {
                guiConsole.drawGUI ();
            }
        }

        private void onApplicationQuit () {
            // 程序退出逻辑 
            this.guiConsole.quit ();
        }
        #endregion

        private void setResourceLoadType () {
            // TODO: 资源加载方式
        }

        public void hotUpdateCompleted () { }

    }
}