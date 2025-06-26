using System.Collections;
using UnityEngine;

public class GameStarter : MonoBehaviour
{
    public IEnumerator Start()
    {
        if (SceneContext.isFirstActivate == true)
        {
            SceneContext.isFirstActivate = false;
            yield return new WaitForSeconds(22f);
            SubtitleManager.Instance.StartIntroSequence();
        }else if(SceneContext.returningFromSecondScene == true)
        {
            SceneContext.returningFromSecondScene = false;
            SubtitleManager.Instance.StartFirewallVictorySequence();
        }
    }
}
