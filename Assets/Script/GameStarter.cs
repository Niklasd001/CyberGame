using UnityEngine;

public class GameStarter : MonoBehaviour
{
    void Start()
    {
        if (SceneContext.isFirstActivate == true)
        {
            SceneContext.isFirstActivate = false;
            SubtitleManager.Instance.StartIntroSequence();
        }else if(SceneContext.returningFromSecondScene == true)
        {
            SceneContext.returningFromSecondScene = false;
            SubtitleManager.Instance.StartFirewallVictorySequence();
        }
    }
}
