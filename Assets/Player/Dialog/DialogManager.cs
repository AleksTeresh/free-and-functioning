using UnityEngine;
using System.Collections.Generic;
using Events;
using Persistence;
using RTS;

namespace Dialog
{
    public class DialogManager : MonoBehaviour
    {
        private DialogResponsePanel dialogResponsePanel;
        private DialogTextPanel dialogTextPanel;

        private Queue<string> sentences = new Queue<string>();
        private DialogNode currentDialogNode;

        public bool BlockGameplay { get; private set; }

        // public bool IsDialogSystemActive { get; private set; }

        // Use this for initialization
        void Start()
        {
            dialogResponsePanel = transform.root.GetComponentInChildren<DialogResponsePanel>();
            dialogTextPanel = transform.root.GetComponentInChildren<DialogTextPanel>();

            // IsDialogSystemActive = false;
        }

        public void SetDialogNode(DialogNode dialogNode)
        {
            if (dialogNode.blockGameplay)
            {
                EventManager.TriggerEvent("HideHUD");
            }

            BlockGameplay = dialogNode.blockGameplay;
            dialogTextPanel.SetOpen(true);
            // IsDialogSystemActive = true;

            sentences.Clear();

            foreach (string sentence in dialogNode.dialogSentences)
            {
                sentences.Enqueue(sentence);
            }

            dialogTextPanel.SetSpeakerAvatar(dialogNode.speakerAvatar);
            dialogTextPanel.SetSpeakerName(dialogNode.speakerName);

            if (dialogNode.responses.Length > 1)
            {
                EventManager.TriggerEvent("HideHUD");
            }

            currentDialogNode = dialogNode;
            DisplayNextSentence();
        }

        public void DisplayNextSentence ()
        {
            if (sentences.Count == 0)
            {
                if (currentDialogNode.responses.Length == 1)
                {
                    SetDialogNode(currentDialogNode.responses[0]);
                }
                else
                {
                    EndDialog();
                }

                return;
            }

            string sentence = sentences.Dequeue();
            dialogTextPanel.SetText(sentence);

            if (currentDialogNode.responses.Length > 1 && sentences.Count == 0)
            {
                dialogResponsePanel.SetOpen(true);
                dialogResponsePanel.SetAnswerOptions(currentDialogNode.responses, this);
                dialogResponsePanel.SetSpeakerAvatar(currentDialogNode.responses[0].speakerAvatar);
                dialogResponsePanel.SetSpeakerName(currentDialogNode.responses[0].speakerName);
            }
            else
            {
                dialogResponsePanel.SetOpen(false);
            }
        }

        public void EndDialog ()
        {
            dialogResponsePanel.SetOpen(false);
            dialogTextPanel.SetOpen(false);
            BlockGameplay = false;

            EventManager.TriggerEvent("ShowHUD");
        }

        public DialogResponsePanel GetDialogResponsePanel ()
        {
            return dialogResponsePanel;
        }

        public DialogTextPanel GetDialogTextPanel ()
        {
            return dialogTextPanel;
        }

        public bool IsActive ()
        {
            return dialogResponsePanel.IsOpen() || dialogTextPanel.IsOpen();
        }

        public DialogManagerData GetData()
        {
            var data = new DialogManagerData();

            data.blockGameplay = BlockGameplay;
            data.currentDialogNode = currentDialogNode.name;

            var sentencesToSave = new List<string>();
            if (dialogTextPanel.IsOpen())
            {
                sentencesToSave.Add(dialogTextPanel.GetText());
            }
            sentencesToSave.AddRange(sentences);
            data.sentences = new Queue<string>(sentencesToSave);

            return data;
        }

        public void SetData (DialogManagerData dialogManagerData)
        {
            Start();

            dialogResponsePanel.Start();
            dialogTextPanel.Start();

            BlockGameplay = dialogManagerData.blockGameplay;
            sentences = dialogManagerData.sentences;

            if (dialogManagerData.currentDialogNode != null && dialogManagerData.currentDialogNode != "")
            {
                currentDialogNode = ResourceManager.GetDialogNode(dialogManagerData.currentDialogNode);

                if (sentences.Count != 0)
                {
                    dialogTextPanel.SetOpen(true);
                    DisplayNextSentence();
                }
            }
        }
    }
}
