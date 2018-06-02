using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Abilities;
using RTS;

public class AbilityBar : MonoBehaviour {

    private readonly static Color BACKGROUND_COLOR = new Color(62 / (float)255, 21 / (float)255, 61 / (float)255, 255 / (float)255);
    private readonly static float SLOT_WIDTH = 50;

    public List<AbilitySlot> AbilitySlots { get; private set; }

    public void ClearSlots()
    {
        AbilitySlots.ForEach(p =>
        {
            // p.Image.sprite = null;
            p.Frame.color = new Color(p.Frame.color.r, p.Frame.color.g, p.Frame.color.b, 0);
            p.Image.color = BACKGROUND_COLOR;
            p.Shade.sizeDelta = new Vector2(-SLOT_WIDTH, p.Shade.sizeDelta.y);
            p.Shade.anchoredPosition = new Vector2(-SLOT_WIDTH / 2, p.Shade.anchoredPosition.y);
            p.Name.text = "";
        });
    }

    public void DrawAbilities(Ability[] abilities, Ability[] multiAbilities, bool isInMultiMode, Ability pendingAllyTargetingAbility)
    {
        Ability[] abilitiesToDraw = isInMultiMode ? multiAbilities : abilities;

        for (int i = 0; i < abilitiesToDraw.Length; i++)
        {
            if (AbilitySlots.Count > i)
            {
                AbilitySlots[i].Image.sprite = abilities[i].icon;
                // TODO uncomment the line below once abilities have their icons

                float cooldownRation = AbilityUtils.GetAbilitySlotCooldownRation(abilities, multiAbilities, i);
                bool abilityIsWaitingForTargetSelection = abilitiesToDraw[i] &&
                    pendingAllyTargetingAbility &&
                    pendingAllyTargetingAbility.name == abilitiesToDraw[i].name;

                AbilitySlots[i].Frame.color = new Color(
                    AbilitySlots[i].Frame.color.r,
                    AbilitySlots[i].Frame.color.g,
                    AbilitySlots[i].Frame.color.b,
                    abilityIsWaitingForTargetSelection
                        ? 0.5f
                        : 0
                );
//                AbilitySlots[i].Image.color = Color.cyan;
                AbilitySlots[i].Name.text = abilitiesToDraw[i].abilityName;
                AbilitySlots[i].Name.enabled = false;
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
        
        HideEmptySlots(abilitiesToDraw);
    }

    private void HideEmptySlots(Ability[] abilitiesToDraw)
    {
        for (int i = abilitiesToDraw.Length; i < AbilitySlots.Count; i++)
        {
            AbilitySlots[i].Image.enabled = false;
        }
    }

    void Awake()
    {
        AbilitySlots = new List<AbilitySlot>(GetComponentsInChildren<AbilitySlot>());
    }
}
