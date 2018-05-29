﻿using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using RTS;
using Events;
using Formation;
using Dialog;
using Menu;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Animator))]
public class HUD : MonoBehaviour
{
    public GUISkin resourceSkin, orderSkin, selectBoxSkin;
    public GUISkin mouseCursorSkin;
    public GUISkin playerDetailsSkin;

    public GUIStyle singleModeStyle;
    public GUIStyle multiModeStyle;

    public Texture2D activeCursor;
    public Texture2D selectCursor, leftCursor, rightCursor, upCursor, downCursor;
    public Texture2D[] moveCursors, attackCursors, harvestCursors;
    public Texture2D rallyPointCursor;

    public Texture2D buttonHover, buttonClick;
    public Texture2D smallButtonHover, smallButtonClick;

    public Texture2D healthy, damaged, critical;

    private CursorState activeCursorState;
    private int currentFrame = 0;

    private const int ORDERS_BAR_WIDTH = 150, RESOURCE_BAR_HEIGHT = 40;
    private const int SELECTION_NAME_HEIGHT = 15;
    private const int BUILD_IMAGE_WIDTH = 64, BUILD_IMAGE_HEIGHT = 64;
    private const int BUTTON_SPACING = 7;
    private const int SCROLL_BAR_WIDTH = 22;

    private int buildAreaHeight = 0;

    private CursorState previousCursorState;

    private WorldObject lastSelection;
    private float sliderValue;

    private Player player;
    // private Camera uiCamera;
    private TargetManager targetManager;
    private FormationManager formationManager;
    private DialogManager dialogManager;

    // audio
    public AudioClip clickSound;
    public float clickVolume = 1.0f;

    private AudioElement audioElement;

    private Canvas canvas;
    private PlayerUnitBar playerUnitBar;
    private EnemyUnitBar enemyUnitBar;
    private SelectionIndicator selectionIndicator;
    private AbilityBar abilityBar;
    private UpperBar upperBar;
    private Menu.PauseMenu pauseMenu;
    private UnitPanelWrapper unitPanelWrapper;
    private List<PlayerUnitPanel> playerUnitPanels;

    private Animator animator;

    // cursor input
    public Vector3 cursorPosition;
    private float joystickCursorSpeed = 50.0f;
    private float mouseCursorSpeed = 7.0f;
    private Vector3 previousMousePosition = Vector3.zero;
    private Vector3 lastCursorPosition;

    // Use this for initialization
    IEnumerator Start()
    {
        playerUnitPanels = new List<PlayerUnitPanel>();

        canvas = GetComponentInChildren<Canvas>();
        player = transform.root.GetComponent<Player>();
        targetManager = player.GetComponentInChildren<TargetManager>();
        formationManager = player.GetComponentInChildren<FormationManager>();
        dialogManager = player.GetComponentInChildren<DialogManager>();
        playerUnitBar = GetComponentInChildren<PlayerUnitBar>();
        enemyUnitBar = GetComponentInChildren<EnemyUnitBar>();
        selectionIndicator = GetComponentInChildren<SelectionIndicator>();
        abilityBar = GetComponentInChildren<AbilityBar>();
        upperBar = GetComponentInChildren<UpperBar>();
        unitPanelWrapper = GetComponentInChildren<UnitPanelWrapper>();

        animator = GetComponent<Animator>();

        if (player && player.human)
        {
            while (!animator || !player || !targetManager || !formationManager)
            {
                yield return new WaitForSeconds(0.1f);
            }
        }

        ResourceManager.StoreSelectBoxItems(selectBoxSkin, healthy, damaged, critical);

        SetCursorState(CursorState.Select);

        buildAreaHeight = Screen.height - RESOURCE_BAR_HEIGHT - SELECTION_NAME_HEIGHT - 2 * BUTTON_SPACING;

        List<AudioClip> sounds = new List<AudioClip>();
        List<float> volumes = new List<float>();
        sounds.Add(clickSound);
        volumes.Add(clickVolume);
        audioElement = new AudioElement(sounds, volumes, "HUD", null);

        Cursor.lockState = CursorLockMode.Locked;
        cursorPosition = new Vector3(Screen.width / 2, Screen.height / 2);
        lastCursorPosition = new Vector3(Screen.width / 2, Screen.height / 2);
    }

    void OnEnable()
    {
        EventManager.StartListening("HideHUD", Hide);
        EventManager.StartListening("ShowHUD", Show);
    }

    void OnDisable()
    {
        EventManager.StopListening("HideHUD", Hide);
        EventManager.StopListening("ShowHUD", Show);
    }

    // Update is called once per frame
    void Update()
    {
        if (player && player.human)
        {
            // DrawOrdersBar();
            /*
            if (playerUnitBar)
            {
                DrawUnitsBar(playerUnitBar, player.GetUnits().Cast<WorldObject>().ToList(), "PlayerIndicator");
            }
            */
            if (enemyUnitBar)
            {
                var observedEmenies = UnitManager.GetPlayerVisibleEnemies(player);
                    DrawUnitsBar(
                    enemyUnitBar,
                    observedEmenies,
                    "EnemyIndicator"
                );
            }

            DrawPlayerUnitPanels();
            /*       
            if (selectionIndicator)
            {
                DrawSelectionIndicator();
            }

            if (abilityBar)
            {
                DrawAbilityBar();
            }
            */
            if (upperBar)
            {
                DrawUpperBar();
            }
        }
    }

    private void OnGUI()
    {
        if (player && player.human && dialogManager && !dialogManager.BlockGameplay)
        {
            if (!pauseMenu)
            {
                GUI.depth = 1;

                HandleCursorPositionUpdate();
                DrawMouseCursor();
            }
        }
    }

    public bool CursorInBounds()
    {
        //Screen coordinates start in the lower-left corner of the screen
        //not the top-left of the screen like the drawing coordinates do
        Vector3 mousePos = cursorPosition;
        bool insideWidth = mousePos.x >= 0 && mousePos.x <= Screen.width;
        bool insideHeight = mousePos.y >= 0 && mousePos.y <= Screen.height;
        return insideWidth && insideHeight;
    }

    public Rect GetPlayingArea()
    {
        return new Rect(0, RESOURCE_BAR_HEIGHT, Screen.width - ORDERS_BAR_WIDTH, Screen.height - RESOURCE_BAR_HEIGHT);
    }

    public CursorState GetPreviousCursorState()
    {
        return previousCursorState;
    }

    public CursorState GetCursorState()
    {
        return activeCursorState;
    }

    public void TogglePauseMenu()
    {
        if (pauseMenu)
        {
            pauseMenu.DestroySelf();
        }
        else if (canvas)
        {
            Time.timeScale = 0.0f;
            var pauseMenuObject = GameObject.Instantiate(ResourceManager.GetUIElement("PauseMenu"));
            pauseMenu = pauseMenuObject.GetComponent<PauseMenu>();

            pauseMenu.transform.SetParent(canvas.transform, false);

            var loadButton = pauseMenu.GetComponentInChildren<LoadButton>();
            loadButton.relatedSceneName = SceneManager.GetActiveScene().name;
        }
    }

    public bool IsPauseMenuOpen ()
    {
        return pauseMenu;
    }

    public void SetCursorState(CursorState newState)
    {
        if (activeCursorState != newState) previousCursorState = activeCursorState;
        activeCursorState = newState;
        switch (newState)
        {
            case CursorState.Select:
                activeCursor = selectCursor;
                break;
            case CursorState.Attack:
                currentFrame = (int)Time.time % attackCursors.Length;
                activeCursor = attackCursors[currentFrame];
                break;
            case CursorState.Harvest:
                currentFrame = (int)Time.time % harvestCursors.Length;
                activeCursor = harvestCursors[currentFrame];
                break;
            case CursorState.Move:
                currentFrame = (int)Time.time % moveCursors.Length;
                activeCursor = moveCursors[currentFrame];
                break;
            case CursorState.PanLeft:
                activeCursor = leftCursor;
                break;
            case CursorState.PanRight:
                activeCursor = rightCursor;
                break;
            case CursorState.PanUp:
                activeCursor = upCursor;
                break;
            case CursorState.PanDown:
                activeCursor = downCursor;
                break;
            case CursorState.RallyPoint:
                activeCursor = rallyPointCursor;
                break;
            default: break;
        }
    }

    public void SetCursorLock(bool locked)
    {
        if (locked)
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
        else
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }

    void Hide()
    {
        if (!player || !player.human) return;

        if (!animator) animator = GetComponent<Animator>();

        animator.SetBool("IsVisible", false);
    }

    void Show()
    {
        if (!player || !player.human) return;

        if (!animator) animator = GetComponent<Animator>();

        animator.SetBool("IsVisible", true);
    }

    private void HandleCursorPositionUpdate()
    {
        cursorPosition += mouseCursorSpeed * new Vector3(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));

        float joystickDeltaX = Gamepad.GetAxis("Joystick Cursor X");
        float joystickDeltaY = Gamepad.GetAxis("Joystick Cursor Y");

        cursorPosition = cursorPosition + joystickCursorSpeed * new Vector3(joystickDeltaX, joystickDeltaY);

        if (cursorPosition.x < 0.0f)
        {
            cursorPosition.x = 0.0f;
        }

        if (cursorPosition.x > Screen.width)
        {
            cursorPosition.x = Screen.width;
        }

        if (cursorPosition.y < 0)
        {
            cursorPosition.y = 0;
        }

        if (cursorPosition.y > Screen.height)
        {
            cursorPosition.y = Screen.height;
        }
    }

    private void DrawAbilityBar(AbilityBar abilityBar, Unit unit)
    {
        var abilitySlots = abilityBar.AbilitySlots;
        var selection = player.SelectedObject;

        // var selectedUnit = (Unit)selection;
        var abilities = unit.GetAbilityAgent().abilities;
        var multiAbilities = unit.GetAbilityAgent().abilitiesMulti;

        abilityBar.DrawAbilities(
            abilities,
            multiAbilities,
            targetManager.InMultiMode,
            targetManager.InMultiMode
                ? player.selectedAlliesTargettingAbility
                : player.selectedAllyTargettingAbility
        );
    }

    private void DrawPlayerUnitPanels ()
    {
        var units = player.GetUnits().Where(unit => unit && unit.hitPoints > 0).ToList();

        if (units.Count != playerUnitPanels.Count)
        {
            playerUnitPanels.ForEach(panel => DestroyImmediate(panel.gameObject));
            playerUnitPanels = new List<PlayerUnitPanel>();

            units.ForEach(unit =>
            {
                var panelObj = Instantiate(ResourceManager.GetUIElement("PlayerUnitPanel"));
                var panel = panelObj.GetComponent<PlayerUnitPanel>();
                panel.Start();
                var unitIndicator = panel.GetIndicator();
                unitIndicator.Init(unit);

                var rectTransform = panelObj.GetComponent<RectTransform>();
                rectTransform.SetParent(unitPanelWrapper.transform, false);
                rectTransform.anchoredPosition = new Vector2(
                    rectTransform.anchoredPosition.x + 350 * units.IndexOf(unit),
                    rectTransform.anchoredPosition.y
                );

                playerUnitPanels.Add(panel);
            });
        }

        for (int i = 0; i < playerUnitPanels.Count; i++)
        {
            if (!playerUnitPanels[i] || !units[i]) return;

            var abilityBar = playerUnitPanels[i].GetAblityBar();

            DrawAbilityBar(abilityBar, units[i]);
        }
    }

    private void DrawSelectionIndicator()
    {
        var selection = player.SelectedObject;

        if (selection)
        {
            // TODO: add more avatar manipulation here
            selectionIndicator.Avatar.color = Color.yellow;

            selectionIndicator.NameField.text = selection.objectName;
            if (selection is Unit && ((Unit) selection).holdingPosition)
            {
                selectionIndicator.NameField.text += " (holding position)";

                selectionIndicator.Avatar.color = Color.blue;
            }

            selectionIndicator.HealthSlider.maxValue = selection.maxHitPoints;
            selectionIndicator.HealthSlider.value = selection.hitPoints; 
        }
        else
        {
            selectionIndicator.NameField.text = "Select Somebody";

            selectionIndicator.HealthSlider.maxValue = 100;
            selectionIndicator.HealthSlider.value = 0;

            // TODO: add avatar manipulation here
        }

    }

    private void DrawUnitsBar(UnitBar unitBar, List<WorldObject> indicatedObjects, string indicatorName)
    {
        indicatedObjects = indicatedObjects
            .Where(p => ((p is Unit) && ((Unit)p).IsMajor()) || (p is BossPart) || (p is Building))
            .ToList();

        var indicators = unitBar.GetIndicators();
        var indicatedUnits = indicators.Select(p => p.GetIndicatedObject()).ToList();

        int newIndicatorsCounter = 0;

        for (int i = 0; i < indicatedUnits.Count; i++)
        {
            if (
                indicatedObjects.Count <= i ||
                !indicatedObjects[i] ||
                !indicatedUnits[i] ||
                indicatedObjects[i].ObjectId != indicatedUnits[i].ObjectId
            )
            {
                indicatedUnits = new List<WorldObject>();
                var indicatorsWrapper = unitBar.GetIndicatorsWrapper();
                new List<Indicator>(
                    indicatorsWrapper.transform
                        .GetComponentsInChildren<Indicator>()
                )
                .ForEach(s => Destroy(s.gameObject));
                break;
            }
        }

        indicatedObjects
            // .Where(p => ((p is Unit) && ((Unit) p).IsMajor()) || (p is BossPart) || (p is Building))
            .ToList().ForEach(p =>
        {
            if (!indicatedUnits.Contains(p))
            {
                var indicatorsWrapper = unitBar.GetIndicatorsWrapper();
                var newIndicatorObject = GameObject.Instantiate(ResourceManager.GetUIElement(indicatorName));
                var newIndicator = newIndicatorObject.GetComponent<Indicator>();

                if (newIndicator)
                {
                    newIndicator.Init(p);
                    newIndicator.transform.parent = indicatorsWrapper.transform;

                    var rectTransform = newIndicatorObject.GetComponent<RectTransform>();
                    rectTransform.anchoredPosition = new Vector2(0, -100 - 100 * (indicatedUnits.Count() + newIndicatorsCounter));
                    rectTransform.sizeDelta = new Vector2(0, 100);

                    newIndicatorsCounter++;
                }
            }
        });

        if (newIndicatorsCounter > 0)
        {
            unitBar.Update();
        }
    }

    private void DrawUpperBar()
    {
        if (targetManager)
        {
            upperBar.SetAttackMode(targetManager.InMultiMode);
        }

        if (formationManager)
        {
            upperBar.SetFormationMode(formationManager.CurrentFormationType);
        }

        var observedEmenies = UnitManager.GetPlayerVisibleEnemies(player);
        upperBar.SetEnemyCount(observedEmenies.Count);
    }

    private void DrawMouseCursor()
    {
        //bool mouseOverHud = !MouseInBounds() && activeCursorState != CursorState.PanRight && activeCursorState != CursorState.PanUp;

        //if (mouseOverHud)
        //{
        //    Cursor.visible = true;
        //}
        //else
        //{
        Cursor.visible = false;
        GUI.skin = mouseCursorSkin;
        GUI.BeginGroup(new Rect(0, 0, Screen.width, Screen.height));
        UpdateCursorAnimation();
        Rect cursorPosition = GetCursorDrawPosition();
        GUI.Label(cursorPosition, activeCursor);
        GUI.EndGroup();
        //}
    }

    private void UpdateCursorAnimation()
    {
        //sequence animation for cursor (based on more than one image for the cursor)
        //change once per second, loops through array of images
        if (activeCursorState == CursorState.Move)
        {
            currentFrame = (int)Time.time % moveCursors.Length;
            activeCursor = moveCursors[currentFrame];
        }
        else if (activeCursorState == CursorState.Attack)
        {
            currentFrame = (int)Time.time % attackCursors.Length;
            activeCursor = attackCursors[currentFrame];
        }
        else if (activeCursorState == CursorState.Harvest)
        {
            currentFrame = (int)Time.time % harvestCursors.Length;
            activeCursor = harvestCursors[currentFrame];
        }
    }

    private Rect GetCursorDrawPosition()
    {
        //set base position for custom cursor image
        float leftPos = cursorPosition.x;
        float topPos = Screen.height - cursorPosition.y; //screen draw coordinates are inverted
                                                         //adjust position base on the type of cursor being shown

        if (!activeCursor)
        {
            return new Rect();
        }

        if (activeCursorState == CursorState.PanRight) leftPos = Screen.width - activeCursor.width;
        else if (activeCursorState == CursorState.PanDown) topPos = Screen.height - activeCursor.height;
        else if (activeCursorState == CursorState.Move || activeCursorState == CursorState.Select || activeCursorState == CursorState.Harvest)
        {
            topPos -= activeCursor.height / 2;
            leftPos -= activeCursor.width / 2;
        }
        else if (activeCursorState == CursorState.RallyPoint) topPos -= activeCursor.height;

        return new Rect(leftPos, topPos, activeCursor.width, activeCursor.height);
    }

    private int MaxNumRows(int areaHeight)
    {
        return areaHeight / BUILD_IMAGE_HEIGHT;
    }

    private Rect GetButtonPos(int row, int column)
    {
        int left = SCROLL_BAR_WIDTH + column * BUILD_IMAGE_WIDTH;
        float top = row * BUILD_IMAGE_HEIGHT - sliderValue * BUILD_IMAGE_HEIGHT;
        return new Rect(left, top, BUILD_IMAGE_WIDTH, BUILD_IMAGE_HEIGHT);
    }

    private void DrawSlider(int groupHeight, float numRows)
    {
        //slider goes from 0 to the number of rows that do not fit on screen
        sliderValue = GUI.VerticalSlider(GetScrollPos(groupHeight), sliderValue, 0.0f, numRows - MaxNumRows(groupHeight));
    }

    private Rect GetScrollPos(int groupHeight)
    {
        return new Rect(BUTTON_SPACING, BUTTON_SPACING, SCROLL_BAR_WIDTH, groupHeight - 2 * BUTTON_SPACING);
    }

    private void DrawStandardBuildingOptions(Building building)
    {
        GUIStyle buttons = new GUIStyle();
        buttons.hover.background = smallButtonHover;
        buttons.active.background = smallButtonClick;
        GUI.skin.button = buttons;
        int leftPos = BUILD_IMAGE_WIDTH + SCROLL_BAR_WIDTH + BUTTON_SPACING;
        int topPos = buildAreaHeight - BUILD_IMAGE_HEIGHT / 2;
        int width = BUILD_IMAGE_WIDTH / 2;
        int height = BUILD_IMAGE_HEIGHT / 2;
        if (building.hasSpawnPoint())
        {
            if (GUI.Button(new Rect(leftPos, topPos, width, height), building.rallyPointImage))
            {
                PlayClick();
                if (activeCursorState != CursorState.RallyPoint && previousCursorState != CursorState.RallyPoint)
                {
                    SetCursorState(CursorState.RallyPoint);
                }
                else
                {
                    //dirty hack to ensure toggle between RallyPoint and not works ...
                    SetCursorState(CursorState.PanRight);
                    SetCursorState(CursorState.Select);
                }
            }
        }
    }

    private void PlayClick()
    {
        if (audioElement != null) audioElement.Play(clickSound);
    }
}
