/*
 * @Author: l hy 
 * @Date: 2021-02-04 10:50:53 
 * @Description: 输入管理
 */

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class InputManager : MonoBehaviour {
    [Header ("外环")]
    public Image outCircle = null;

    [Header ("内环")]
    public Image insideCircle = null;

    private Vector3 touchStartPos = Vector3.zero;
    private Vector3 touchMovePos = Vector3.zero;
    private Vector3 curMoveDir = Vector3.zero;

    private Vector2 ratio = Vector2.zero;

    public void init () {
        this.ratio.x = 640 / Screen.width;
        this.ratio.y = 1136 / Screen.height;
    }

    public void localUpdate () {
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
        this.touchStartPos = new Vector3 (touch.position.x, 0, touch.position.y);

        // 环坐标设置
        Vector2 circlePos = new Vector2 (this.touchStartPos.x - Screen.width / 2, this.touchStartPos.z - Screen.height / 2);
        this.outCircle.rectTransform.localPosition = new Vector2 (circlePos.x * ratio.x, circlePos.y * ratio.y);
    }

    private void touchMove (Touch touch) {
        if (this.touchStartPos == Vector3.zero) {
            return;
        }

        Vector3 moveEndPos = new Vector3 (touch.position.x, 0, touch.position.y);

        Vector3 moveLimited = moveEndPos - touchStartPos;
        if (moveLimited.magnitude < ConstValue.moveMinDis) {
            return;
        }

        this.touchMovePos = moveEndPos;

        Vector3 inSideCircleEndPos = Vector3.zero;
        if (moveLimited.magnitude > ConstValue.joyStickMaxDis) {
            inSideCircleEndPos = moveLimited.normalized * ConstValue.joyStickMaxDis;
        } else {
            inSideCircleEndPos = moveLimited;
        }

        this.insideCircle.rectTransform.localPosition = new Vector2 (inSideCircleEndPos.x - Screen.width / 2, inSideCircleEndPos.z + Screen.height / 2);
    }

    private void touchEnd () {
        Vector3 moveLimited = this.touchMovePos - touchStartPos;
        if (moveLimited.magnitude < ConstValue.moveMinDis) {
            this.curMoveDir = Vector3.zero;
        } else {
            this.curMoveDir = moveLimited;
        }
        this.touchStartPos = Vector3.zero;

        this.outCircle.rectTransform.localPosition = Vector2.zero;
        this.insideCircle.rectTransform.localPosition = Vector2.zero;
    }

    private void touchCancel () {
        Vector3 moveLimited = this.touchMovePos - touchStartPos;
        if (moveLimited.magnitude < ConstValue.moveMinDis) {
            this.curMoveDir = Vector3.zero;
        } else {
            this.curMoveDir = moveLimited;
        }
        this.touchStartPos = Vector3.zero;

        this.outCircle.rectTransform.localPosition = Vector2.zero;
        this.insideCircle.rectTransform.localPosition = Vector2.zero;
    }

    public Vector3 moveDir {
        get {
            return this.curMoveDir.normalized;
        }
    }

    public bool isTouch {
        get {
            return this.touchStartPos != Vector3.zero;
        }
    }

    public Vector3 aimDir {
        get {
            return (this.touchMovePos - touchStartPos).normalized;
        }
    }
}