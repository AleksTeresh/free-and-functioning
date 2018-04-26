﻿using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using RTS;

namespace Dialog
{
    public class DialogManager : MonoBehaviour
    {
        public DialogNode startDialogNode;

        private DialogResponsePanel dialogResponsePanel;
        private DialogTextPanel dialogTextPanel;

        private Queue<string> sentences = new Queue<string>();
        private DialogNode currentDialogNode;

        public bool BlockGameplay { get; set; }

        // public bool IsDialogSystemActive { get; private set; }

        // Use this for initialization
        void Start()
        {
            dialogResponsePanel = transform.root.GetComponentInChildren<DialogResponsePanel>();
            dialogTextPanel = transform.root.GetComponentInChildren<DialogTextPanel>();

            // IsDialogSystemActive = false;

            // TODO: the lines below are for testing only, REMOVE them
            SetDialogNode(startDialogNode);
            BlockGameplay = true;
        }

        public void SetDialogNode(DialogNode dialogNode)
        {
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
                } else
                {
                    EndDialog();
                    return;
                }
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

        public bool IsDialogSystemActive ()
        {
            return dialogResponsePanel.IsOpen() || dialogTextPanel.IsOpen();
        }
    }
}
