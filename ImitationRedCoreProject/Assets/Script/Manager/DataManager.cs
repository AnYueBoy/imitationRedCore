/*
 * @Author: l hy 
 * @Date: 2021-02-19 10:37:08 
 * @Description: 数据管理
 */

using UFrameWork.Application;

public class DataManager : IModule {

    public InSideData inSideData = null;

    public void init () {
        inSideData = new InSideData ();
    }

    public void localUpdate (float dt) {

    }
}