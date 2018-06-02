using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogTextPanel : MonoBehaviour {
    private Animator animator;

    private Text dialogText;
    private Image speakerAvatar;
    private Text speakerName;

    public void Start()
    {
        dialogText = GetComponentInChildren<DialogText>().GetComponent<Text>();
        speakerName = GetComponentInChildren<SpeakerName>().GetComponent<Text>();
        speakerAvatar = GetComponentInChildren<SpeakerAvatar>().GetComponent<Image>();

        animator = GetComponent<Animator>();
    }

    public void SetOpen(bool open)
    {
        animator.SetBool("IsOpen", open);
    }

    public bool IsOpen()
    {
        return animator.GetBool("IsOpen");
    }

    public void SetText(string text)
    {
        StopAllCoroutines();
        StartCoroutine(TypeSentence(text));
    }

    public string GetText ()
    {
        return dialogText.text;
    }

    public void SetSpeakerName (string name)
    {
        speakerName.text = name;
    }

    public void SetSpeakerAvatar (Sprite avatar)
    {
        // TODO uncomment when avatars are set for the dialog nodes
//        speakerAvatar.sprite = avatar;
    }

    IEnumerator<int> TypeSentence(string sentence)
    {
        dialogText.text = "";

        foreach (char letter in sentence.ToCharArray())
        {
            dialogText.text += letter;
            yield return 0;
        }
    }
}
