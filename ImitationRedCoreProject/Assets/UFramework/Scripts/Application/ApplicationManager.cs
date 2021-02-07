/*
 * @Author: l hy 
 * @Date: 2021-01-25 15:34:12 
 * @Description: 程序入口
 */
namespace UFrameWork.Application {
    using UFrameWork.Develop;
    using UnityEngine;

    public class ApplicationManager : MonoBehaviour {

        [Header ("当前程序模式")]
        public AppMode appMode = AppMode.Developing;

        private GUIConsole guiConsole = new GUIConsole ();

        public CameraTrack cameraTrack = null;

        public BallManager ballManager = null;

        #region  程序生命周期函数
        private void Awake () {
            appLaunch ();
        }

        private void Update () {
            if (guiConsole != null) {
                guiConsole.localUpdate ();
            }

            InputManager.instance.localUpdate ();
            ballManager.localUpdate ();
        }

        private void LateUpdate () {
            this.cameraTrack.localLateUpdate ();
        }

        private void OnGUI () {
            if (guiConsole != null) {
                guiConsole.drawGUI ();
            }
        }

        private void onApplicationQuit () {
            // 程序退出逻辑 
            this.guiConsole.quit ();
        }
        #endregion

        private void appLaunch () {
            if (appMode != AppMode.Release) {
                // 图形控制面板初始化
                guiConsole.init ();
            }

            // InputManager.instance.init ();

            ballManager.init ();

            // 初始化
        }
    }
}