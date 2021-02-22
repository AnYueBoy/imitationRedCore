/*
 * @Author: l hy 
 * @Date: 2021-02-22 09:17:58 
 * @Description: 模块接口
 */

namespace UFrameWork.Application {
    public interface IModule {
        void init ();
        void localUpdate (float dt);
    }

}