using UnityEngine;
using EazyTools.SoundManager;

namespace Events
{
    [CreateAssetMenu(menuName = "Events/Action/PlayBgm")]
    public class PlayBgmAction : Action
    {
        public AudioClip bgm;

        public override void Act(StateController controller)
        {
            if(bgm)
            {
                SoundManager.PlayMusic(bgm, .8f, true, false);
            }
        }
    }
}
