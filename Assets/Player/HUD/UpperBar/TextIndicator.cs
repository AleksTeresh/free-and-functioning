using UnityEngine;
using UnityEngine.UI;

public class TextIndicator : MonoBehaviour {

    private Image background;
    private Text text;

	// Use this for initialization
	void Start () {
        background = GetComponentInChildren<Image>();
        text = GetComponentInChildren<Text>();
    }
	
	public void SetColor (Color color)
    {
        background.color = color;
    }

    public void SetSprite (Sprite sprite)
    {
        background.sprite = sprite;
    }

    public void SetText(string text)
    {
        this.text.text = text;
    }
}
