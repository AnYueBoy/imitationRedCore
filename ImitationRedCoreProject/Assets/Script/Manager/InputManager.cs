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
        this.outCircle.transform.position = this.touchStartPos;
    }

    private void touchMove (Touch touch) {
        if (this.touchStartPos == Vector3.zero) {
            return;
        }

        Vector3 moveEndPos = new Vector3 (touch.position.x, 0, touch.position.y);

        Vector3 moveLimited = moveEndPos - touchStartPos;
        if (moveLimited.magnitude < 0.2f) {
            return;
        }

        this.touchMovePos = moveEndPos;

        if (moveLimited.magnitude > ConstValue.joyStickMaxDis) {
            this.outCircle.transform.position = moveLimited.normalized * ConstValue.joyStickMaxDis;
        } else {
            this.outCircle.transform.position = moveLimited;
        }
    }

    private void touchEnd () {
        this.curMoveDir = this.touchMovePos - touchStartPos;
        this.touchStartPos = Vector3.zero;
        this.outCircle.transform.position = Vector3.zero;
    }

    private void touchCancel () {
        this.curMoveDir = this.touchMovePos - touchStartPos;
        this.touchStartPos = Vector3.zero;
        this.outCircle.transform.position = Vector3.zero;
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