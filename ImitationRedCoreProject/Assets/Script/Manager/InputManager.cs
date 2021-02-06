/*
 * @Author: l hy 
 * @Date: 2021-02-04 10:50:53 
 * @Description: 输入管理
 */

using System.Collections.Generic;
using UFramework.FrameUtil;
using UFrameWork.Application;
using UnityEngine;
public class InputManager : MonoBehaviour {

    private Vector3 touchStartPos = Vector3.zero;
    private Vector3 touchMovePos = Vector3.zero;
    private Vector3 curMoveDir = Vector3.zero;

    // FIXME: 测试逻辑
    public LayerMask layerMask;

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

        List<Vector3> pathList = new List<Vector3> ();

        // FIXME: 测试逻辑 
        ApplicationManager.instance.ballManager.getReflectPath (this.aimDir, 15.0f, pathList, layerMask);
        // FIXME: 测试逻辑
        for (int i = 0; i < pathList.Count - 1; i++) {
            Vector3 drawStartPos = pathList[i];
            Vector3 drawEndPos = pathList[i + 1];
            CommonUtil.drawLine (drawStartPos, drawEndPos, Color.red);
        }
    }

    private void touchEnd () {
        this.curMoveDir = this.touchMovePos - touchStartPos;
        this.touchStartPos = Vector3.zero;
    }

    private void touchCancel () {
        this.curMoveDir = this.touchMovePos - touchStartPos;
        this.touchStartPos = Vector3.zero;
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