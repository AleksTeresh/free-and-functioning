using UnityEngine;
using UnityEditor;

namespace Dialog
{
    [CreateAssetMenu(menuName = "Dialog/Node")]
    public class DialogNode : ScriptableObject
    {
        public Sprite speakerAvatar;
        public string speakerName;
        public string displayedOptionText;

        [TextArea(3, 10)]
        public string[] dialogSentences;
        public DialogNode[] responses;
    }
}
