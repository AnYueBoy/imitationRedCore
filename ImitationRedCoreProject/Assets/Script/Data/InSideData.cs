/*
 * @Author: l hy 
 * @Date: 2021-02-19 10:36:19 
 * @Description: 局内游戏数据
 */
public class InSideData {
    public float gameSpeed = 1;

    public GameState curGameState;
    public InSideData () {
        this.gameSpeed = ConstValue.normalGameSpeed;

        // FIXME: 临时状态为游戏中
        this.curGameState = GameState.GAME_OVER;
    }
}