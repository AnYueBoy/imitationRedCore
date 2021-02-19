/*
 * @Author: l hy 
 * @Date: 2021-02-19 10:37:08 
 * @Description: 数据管理
 */

public class DataManager {

    private static DataManager _instance;

    public static DataManager instance {
        get {
            if (_instance == null) {
                _instance = new DataManager ();
            }
            return _instance;
        }
    }

    public InSideData inSideData = null;

    public void init () {
        inSideData = new InSideData ();
    }
}