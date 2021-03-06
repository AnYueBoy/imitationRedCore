/*
 * @Author: l hy 
 * @Date: 2021-02-04 10:50:53 
 * @Description: 输入管理
 */

using UFramework.GameCommon;
using UFrameWork.Application;
using UnityEngine;
using UnityEngine.UI;
public class InputManager : MonoBehaviour, IModule {
    [Header ("外环")]
    public Image outCircle = null;

    [Header ("内环")]
    public Image insideCircle = null;

    private Vector2 touchStartPos = Vector2.zero;
    private Vector2 touchEndPos = Vector2.zero;
    private Vector2 curMoveDir = Vector2.zero;

    private float halfScreenWidth = 0;
    private float halfScreenHeight = 0;

    public void init () {
        this.halfScreenWidth = Screen.width / 2;
        this.halfScreenHeight = Screen.height / 2;
    }

    public void localUpdate (float dt) {
        this.checkTouch ();
    }

    private void checkTouch () {
        if (Input.touchCount <= 0) {
            return;
        }

        Touch touch = Input.touches[0];
        if (touch.phase == TouchPhase.Began) {
            this.touchStart (touch);
        }

        if (touch.phase == TouchPhase.Moved) {
            this.touchMove (touch);
        }

        if (touch.phase == TouchPhase.Ended) {
            this.touchEnd ();
        }

        if (touch.phase == TouchPhase.Canceled) {
            this.touchCancel ();
        }
    }

    private void touchStart (Touch touch) {
        this.outCircle.gameObject.SetActive (true);

        this.touchStartPos = touch.position;

        // 环坐标设置
        Vector2 circlePos = new Vector2 (this.touchStartPos.x - halfScreenWidth, this.touchStartPos.y - halfScreenHeight);
        this.outCircle.rectTransform.localPosition = circlePos;

        // 游戏速率设置
        ModuleManager.instance.dataManager.inSideData.gameSpeed = ConstValue.slowGameSpeed;
    }

    private void touchMove (Touch touch) {
        if (this.touchStartPos == Vector2.zero) {
            return;
        }

        Vector2 touchMoveVec = touch.position - touchStartPos;
        float touchMoveDis = touchMoveVec.magnitude;

        // 圆环ui显示相关
        Vector2 insideCircleEndPos = Vector2.zero;
        if (touchMoveDis > ConstValue.joyStickMaxDis) {
            insideCircleEndPos = touchMoveVec.normalized * ConstValue.joyStickMaxDis;
        } else {
            insideCircleEndPos = touchMoveVec.normalized * touchMoveDis;
        }

        this.insideCircle.rectTransform.localPosition = insideCircleEndPos;

        // 实际操作相关
        if (touchMoveDis < ConstValue.moveMinDis) {
            return;
        }

        this.touchEndPos = touch.position;
    }

    private void touchEnd () {
        Vector2 touchMoveVec = this.touchEndPos - touchStartPos;
        float touchMoveDis = touchMoveVec.magnitude;
        if (touchMoveDis < ConstValue.moveMinDis) {
            this.curMoveDir = Vector2.zero;
        } else {
            this.curMoveDir = touchMoveVec;
        }

        this.touchStartPos = Vector2.zero;
        this.insideCircle.rectTransform.localPosition = Vector2.zero;
        this.outCircle.gameObject.SetActive (false);

        // FIXME: 事件的触发方式，有待商榷
        ListenerManager.instance.trigger (EventEnum.refreshPathList);

        // 游戏速率恢复
        ModuleManager.instance.dataManager.inSideData.gameSpeed = ConstValue.normalGameSpeed;
    }

    private void touchCancel () {
        Vector2 touchMoveVec = this.touchEndPos - touchStartPos;
        float touchMoveDis = touchMoveVec.magnitude;
        if (touchMoveDis < ConstValue.moveMinDis) {
            this.curMoveDir = Vector2.zero;
        } else {
            this.curMoveDir = touchMoveVec;
        }

        this.touchStartPos = Vector2.zero;
        this.insideCircle.rectTransform.localPosition = Vector2.zero;
        this.outCircle.gameObject.SetActive (false);

        // FIXME: 事件的触发方式，有待商榷
        ListenerManager.instance.trigger (EventEnum.refreshPathList);

        // 游戏速率恢复
        ModuleManager.instance.dataManager.inSideData.gameSpeed = ConstValue.normalGameSpeed;
    }

    public Vector3 aimDir {
        get {
            Vector3 aimDir = this.touchEndPos - this.touchStartPos;
            aimDir = new Vector3 (aimDir.x, 0, aimDir.y);
            return aimDir.normalized;
        }
    }

    public bool isTouch {
        get {
            return this.touchStartPos != Vector2.zero;
        }
    }
}