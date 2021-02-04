/*
 * @Author: l hy 
 * @Date: 2021-02-04 10:50:53 
 * @Description: 输入管理
 */

using UnityEngine;
public class InputManager : MonoBehaviour {

    private Vector2 touchStartPos = Vector2.zero;
    private Vector2 touchMovePos = Vector2.zero;
    private Vector2 curMoveDir = Vector2.zero;

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
        this.touchStartPos = touch.position;
    }

    private void touchMove (Touch touch) {
        if (this.touchStartPos == Vector2.zero) {
            return;
        }

        this.touchMovePos = touch.position - touchStartPos;
        if (this.touchMovePos.magnitude < 0.2f) {
            return;
        }

        this.touchMovePos = touch.position;
    }

    private void touchEnd () {
        this.curMoveDir = this.touchMovePos - touchStartPos;
        this.touchStartPos = Vector2.zero;
    }

    private void touchCancel () {
        this.curMoveDir = this.touchMovePos - touchStartPos;
        this.touchStartPos = Vector2.zero;
    }

    public Vector2 moveDir {
        get {
            return this.curMoveDir.normalized;
        }
    }

    public bool isTouch {
        get {
            return this.touchStartPos != Vector2.zero;
        }
    }

    public Vector2 aimDir {
        get {
            return (this.touchMovePos - touchStartPos).normalized;
        }
    }
}