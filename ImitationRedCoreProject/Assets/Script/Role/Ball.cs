/*
 * @Author: l hy 
 * @Date: 2021-02-06 16:14:00 
 * @Description: 球
 */

using UnityEngine;
public class Ball : MonoBehaviour {

    public Transform node {
        get {
            return this.transform;
        }
    }

    public Transform arrowTransform = null;
}