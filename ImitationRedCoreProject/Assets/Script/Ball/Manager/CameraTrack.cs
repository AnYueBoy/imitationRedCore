/*
 * @Author: l hy 
 * @Date: 2021-02-04 15:30:34 
 * @Description: 相机跟踪
 */

using UnityEngine;
public class CameraTrack : MonoBehaviour {

    // FIXME: 临时
    public Transform targetTrans = null;

    private Vector3 offset = Vector3.zero;

    private readonly float trackSpeed = 5.0f;

    public void init (Transform targetTrans, Vector3 cameraPos) {
        this.targetTrans = targetTrans;
        this.transform.position = cameraPos;
        offset = this.transform.position - this.targetTrans.position;
    }

    private void Start() {
        // 临时，带删除
        offset = this.transform.position - this.targetTrans.position; 
    }

    public void localLateUpdate () {
        if (!targetTrans) {
            return;
        }

        Vector3 targetPos = offset + this.targetTrans.position;
        this.transform.position = Vector3.Lerp (transform.position, targetPos, Time.deltaTime * trackSpeed);
    }
}