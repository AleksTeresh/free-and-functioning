using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Abilities;
using RTS;

public class AbilityBar : MonoBehaviour {

    private readonly static Color BACKGROUND_COLOR = new Color(62 / (float)255, 21 / (float)255, 61 / (float)255, 255 / (float)255);
    private readonly static float SLOT_WIDTH = 70;

    public List<AbilitySlot> AbilitySlots { get; private set; }

    public void ClearSlots()
    {
        AbilitySlots.ForEach(p =>
        {
            // p.Image.sprite = null;
            p.Image.color = BACKGROUND_COLOR;
            p.Shade.sizeDelta = new Vector2(-SLOT_WIDTH, p.Shade.sizeDelta.y);
            p.Shade.anchoredPosition = new Vector2(-SLOT_WIDTH / 2, p.Shade.anchoredPosition.y);
            p.Name.text = "";
        });
    }

    public void DrawAbilities(Ability[] abilities, Ability[] multiAbilities, bool isInMultiMode)
    {
        Ability[] abilitiesToDraw = isInMultiMode ? multiAbilities : abilities;

        for (int i = 0; i < abilitiesToDraw.Length; i++)
        {
            if (AbilitySlots.Count > i)
            {
                // abilitySlots[i].Image.sprite = abilities[i].sprite;
                // TODO uncomment the line below once abilities have their icons

                float cooldownRation = AbilityUtils.GetAbilitySlotCooldownRation(abilities, multiAbilities, i);

                AbilitySlots[i].Image.color = Color.cyan;
                AbilitySlots[i].Name.text = abilitiesToDraw[i].abilityName;
                AbilitySlots[i].Shade.sizeDelta = new Vector2(
                    -(cooldownRation) * SLOT_WIDTH,
                    AbilitySlots[i].Shade.sizeDelta.y
                );
                AbilitySlots[i].Shade.anchoredPosition = new Vector2(
                    -SLOT_WIDTH / 2 + (1 - cooldownRation) * SLOT_WIDTH / 2,
                    AbilitySlots[i].Shade.anchoredPosition.y
                );
            }
        }
    }

    void Awake()
    {
        AbilitySlots = new List<AbilitySlot>(GetComponentsInChildren<AbilitySlot>());
    }
}
