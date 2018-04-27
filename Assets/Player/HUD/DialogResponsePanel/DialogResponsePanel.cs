using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Dialog;
using RTS;
using UnityEngine.EventSystems;

public class DialogResponsePanel : MonoBehaviour {
    // public selectedOptionSprite
    private Animator animator;

    private VerticalLayoutGroup responseOptionWrapper;
    private Image speakerAvatar;
    private Text speakerName;

    private List<Button> responseButtons = new List<Button>();
    private int selectedOption = -1;

    private void Start()
    {
        responseOptionWrapper = GetComponentInChildren<VerticalLayoutGroup>();
        speakerName = GetComponentInChildren<SpeakerName>().GetComponent<Text>();
        speakerAvatar = GetComponentInChildren<SpeakerAvatar>().GetComponent<Image>();

        animator = GetComponent<Animator>();
    }

    public void SetOpen (bool open)
    {
        animator.SetBool("IsOpen", open);
        if (!open)
        {
            RemoveCurrentButtons();
        }
    }
    public bool IsOpen ()
    {
        return animator.GetCurrentAnimatorStateInfo(0).IsName("ResponsePanel_Open") ||
            animator.IsInTransition(0);
    }

    public void SetAnswerOptions(DialogNode[] responses, DialogManager dialogManager)
    {
        RemoveCurrentButtons();

        foreach (var dialogNode in responses)
        {
            var responseButtonObject = GameObject.Instantiate(ResourceManager.GetUIElement("ResponseButton"));
            var responseButton = responseButtonObject.GetComponent<Button>();
            var responseButtonText = responseButtonObject.GetComponentInChildren<Text>();

            responseButton.transform.parent = responseOptionWrapper.transform;
            responseButtonText.text = dialogNode.displayedOptionText;
            
            responseButton.onClick.AddListener(() => dialogManager.SetDialogNode(dialogNode));

            responseButtons.Add(responseButton);
        }

        if (responseButtons.Count > 0)
        {
            responseButtons[0].Select();
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

    public void NextOption()
    {
        if (responseButtons.Count > selectedOption + 1)
        {
            Debug.Log("Next dialog option");
            selectedOption += 1;

            responseButtons[selectedOption].transition = Selectable.Transition.ColorTint;
            // responseButtons[selectedOption].Select();
        }
    }

    public void PreviousOption ()
    {
        if (selectedOption > 0 && responseButtons[selectedOption - 1])
        {
            Debug.Log("Prev dialog option");
            selectedOption -= 1;

            responseButtons[selectedOption].transition = Selectable.Transition.ColorTint;
            // responseButtons[selectedOption].Select();
        }
    }

    public void SelectCurrentOption ()
    {
        if (responseButtons[selectedOption])
        {
            Debug.Log("Select current option");
            var action = responseButtons[selectedOption].onClick;

            if (action != null)
            {
                action.Invoke();
            }
        }
    }

    private void RemoveCurrentButtons ()
    {
        var existingButtons = responseOptionWrapper.GetComponentsInChildren<ResponseButton>();

        foreach (var button in existingButtons)
        {
            Destroy(button.gameObject);
        }

        this.responseButtons = new List<Button>();
    }
}
