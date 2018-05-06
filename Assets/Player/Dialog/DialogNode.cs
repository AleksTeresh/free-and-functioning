using UnityEngine;

namespace Dialog
{
    [CreateAssetMenu(menuName = "Dialog/Node")]
    public class DialogNode : ScriptableObject
    {
        public Sprite speakerAvatar;
        public string speakerName;
        public string displayedOptionText;
        public bool blockGameplay;

        [TextArea(3, 10)]
        public string[] dialogSentences;
        public DialogNode[] responses;
    }
}
