/*
 * @Author: l hy 
 * @Date: 2021-02-04 15:30:34 
 * @Description: 相机跟踪
 */

using UnityEngine;
public class CameraTrack : MonoBehaviour {

    private Transform targetTrans = null;

    private Vector3 offset = Vector3.zero;

    private readonly float trackSpeed = 5.0f;

    private static CameraTrack _instance = null;

    private void Awake () {
        _instance = this;
    }

    public static CameraTrack instance {
        get {
            return _instance;
        }
    }

    public void init (Transform targetTrans, Vector3 cameraPos) {
        this.targetTrans = targetTrans;
        // FIXME: 等待修改
        // this.transform.position = cameraPos;
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