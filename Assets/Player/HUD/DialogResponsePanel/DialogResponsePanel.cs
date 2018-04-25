using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Dialog;
using RTS;

public class DialogResponsePanel : MonoBehaviour {
    private Animator animator;

    private VerticalLayoutGroup responseOptionWrapper;
    private Image speakerAvatar;
    private Text speakerName;

    private void Awake()
    {
        responseOptionWrapper = GetComponentInChildren<VerticalLayoutGroup>();
        speakerName = GetComponentInChildren<SpeakerName>().GetComponent<Text>();
        speakerAvatar = GetComponentInChildren<SpeakerAvatar>().GetComponent<Image>();

        animator = GetComponent<Animator>();
    }

    public void SetOpen (bool open)
    {
        animator.SetBool("IsOpen", open);
    }

    public void SetAnswerOptions(DialogNode[] responses, DialogManager dialogManager)
    {
        var existingButtons = responseOptionWrapper.GetComponentsInChildren<ResponseButton>();

        foreach (var button in existingButtons)
        {
            Destroy(button);
        }

        foreach (var dialogNode in responses)
        {
            var responseButtonObject = ResourceManager.GetUIElement("ResponseButton");
            var responseButton = responseButtonObject.GetComponent<Button>();
            var responseButtonText = responseButtonObject.GetComponentInChildren<Text>();

            responseButton.transform.parent = responseOptionWrapper.transform;
            responseButtonText.text = dialogNode.displayedOptionText;
            
            responseButton.onClick.AddListener(() => dialogManager.SetDialogNode(dialogNode));
        }
    }

    public void SetSpeakerName(string name)
    {
        speakerName.text = name;
    }

    public void SetSpeakerAvatar(Sprite avatar)
    {
        speakerAvatar.sprite = avatar;
    }
}
