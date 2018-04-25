using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

namespace Dialog
{
    public class DialogManager : MonoBehaviour
    {
        private DialogResponsePanel dialogResponsePanel;
        private DialogTextPanel dialogTextPanel;

        private Queue<string> sentences;

        public bool IsDialogSystemActive { get; private set; }

        // Use this for initialization
        void Start()
        {
            dialogResponsePanel = transform.root.GetComponentInChildren<DialogResponsePanel>();
            dialogTextPanel = transform.root.GetComponentInChildren<DialogTextPanel>();

            IsDialogSystemActive = false;
        }

        public void SetDialogNode(DialogNode dialogNode)
        {
            dialogResponsePanel.SetOpen(true);
            dialogTextPanel.SetOpen(true);
            IsDialogSystemActive = true;

            sentences.Clear();

            foreach (string sentence in dialogNode.dialogSentences)
            {
                sentences.Enqueue(sentence);
            }

            dialogTextPanel.SetSpeakerAvatar(dialogNode.speakerAvatar);
            dialogTextPanel.SetSpeakerName(dialogNode.speakerName);

            dialogResponsePanel.SetAnswerOptions(dialogNode.responses, this);
            dialogResponsePanel.SetSpeakerAvatar(dialogNode.responses[0].speakerAvatar);
            dialogResponsePanel.SetSpeakerName(dialogNode.responses[0].speakerName);

            DisplayNextSentence();
        }

        public void DisplayNextSentence ()
        {
            if (sentences.Count == 0)
            {
                EndDialog();
                return;
            }

            string sentence = sentences.Dequeue();

            dialogTextPanel.SetText(sentence);
        }

        public void EndDialog ()
        {
            dialogResponsePanel.SetOpen(false);
            dialogTextPanel.SetOpen(false);
            IsDialogSystemActive = false;
        }
    }
}
