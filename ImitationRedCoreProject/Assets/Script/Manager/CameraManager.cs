/*
 * @Author: l hy 
 * @Date: 2021-02-04 15:30:34 
 * @Description: 相机跟踪
 */

using UFrameWork.Application;
using UnityEngine;
public class CameraManager : MonoBehaviour, IModule {

    private Transform targetTrans = null;

    private Vector3 offset = Vector3.zero;

    private readonly float trackSpeed = 5.0f;

    public void init () {
        this.targetTrans = ModuleManager.instance.ballManager.currentBall.transform;
        // FIXME: 等待修改
        // this.transform.position = cameraPos;
        offset = this.transform.position - this.targetTrans.position;
    }

    public void localUpdate (float dt) {

    }

    public void localLateUpdate (float dt) {
        if (!targetTrans) {
            return;
        }

        Vector3 targetPos = offset + this.targetTrans.position;
        this.transform.position = Vector3.Lerp (transform.position, targetPos, Time.deltaTime * trackSpeed);
    }
}