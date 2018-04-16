using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using RTS;

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

    // audio
    public AudioClip clickSound;
    public float clickVolume = 1.0f;

    private AudioElement audioElement;

    private PlayerUnitBar playerUnitBar;
    private EnemyUnitBar enemyUnitBar;
    private SelectionIndicator selectionIndicator;
    private AbilityBar abilityBar;

    // cursor input
    public Vector3 cursorPosition;
    private float joystickCursorSpeed = 50.0f;
    private float mouseCursorSpeed = 7.0f;
    private Vector3 previousMousePosition = Vector3.zero;
    private Vector3 lastCursorPosition;

    // Use this for initialization
    void Start()
    {
        // uiCamera = FindObjectOfType<UICamera>().GetComponent<Camera>();
        // uiCamera.enabled = false;

        player = transform.root.GetComponent<Player>();
        targetManager = player.GetComponentInChildren<TargetManager>();
        playerUnitBar = GetComponentInChildren<PlayerUnitBar>();
        enemyUnitBar = GetComponentInChildren<EnemyUnitBar>();
        selectionIndicator = GetComponentInChildren<SelectionIndicator>();
        abilityBar = GetComponentInChildren<AbilityBar>();

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

    // Update is called once per frame
    void Update()
    {
        if (player && player.human)
        {
            // DrawOrdersBar();
            DrawUnitsBar(playerUnitBar, player.GetUnits(), "PlayerIndicator");

            var observedEmenies = UnitManager.GetPlayerVisibleEnemies(player);
            DrawUnitsBar(enemyUnitBar, observedEmenies, "EnemyIndicator");
            DrawSelectionIndicator();
            DrawAbilityBar();
        }
    }

    private void OnGUI()
    {
        if (player && player.human)
        {
            GUI.depth = 1;

            HandleCursorPositionUpdate();
            DrawPlayerDetails();

            var observedEmenies = UnitManager.GetPlayerVisibleEnemies(player);
            DrawUpperBar(observedEmenies.Count);
            DrawMouseCursor();
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

    private void DrawAbilityBar()
    {
        var abilitySlots = abilityBar.AbilitySlots;
        var selection = player.SelectedObject;

        abilityBar.ClearSlots();

        if (selection && selection is Unit)
        {
            var selectedUnit = (Unit)selection;
            var abilities = selectedUnit.abilities;
            var multiAbilities = selectedUnit.abilitiesMulti;

            abilityBar.DrawAbilities(abilities, multiAbilities, targetManager.InMultiMode);
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

    private void DrawUnitsBar(UnitBar unitBar, List<Unit> units, string indicatorName)
    {
        var indicators = unitBar.GetIndicators();
        var indicatedUnits = indicators.Select(p => p.GetUnit()).ToList();

        int newIndicatorsCounter = 0;

        for (int i = 0; i < indicatedUnits.Count; i++)
        {
            if (!units.Contains(indicatedUnits[i]))
            {
                indicatedUnits = new List<Unit>();
                var indicatorsWrapper = unitBar.GetIndicatorsWrapper();
                new List<Indicator>(
                    indicatorsWrapper.transform
                        .GetComponentsInChildren<Indicator>()
                )
                .ForEach(s => Destroy(s.gameObject));
                break;
            }
        }

        units.Where(p => p.IsMajor()).ToList().ForEach(p =>
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

    private void DrawOrdersBar()
    {
        GUI.skin = orderSkin;
        GUI.BeginGroup(new Rect(Screen.width - ORDERS_BAR_WIDTH, RESOURCE_BAR_HEIGHT, ORDERS_BAR_WIDTH, Screen.height - RESOURCE_BAR_HEIGHT));
        GUI.Box(new Rect(0, 0, ORDERS_BAR_WIDTH, Screen.height - RESOURCE_BAR_HEIGHT), "");

        string selectionName = "";
        if (player.SelectedObject)
        {
            selectionName = player.SelectedObject.objectName;

            if (player.SelectedObject.IsOwnedBy(player))
            {
                //reset slider value if the selected object has changed
                if (lastSelection && lastSelection != player.SelectedObject) sliderValue = 0.0f;
                DrawActions(player.SelectedObject.GetActions());
                //store the current selection
                lastSelection = player.SelectedObject;

                Building selectedBuilding = lastSelection.GetComponent<Building>();
                if (selectedBuilding)
                {
                    // DrawBuildQueue(selectedBuilding.getBuildQueueValues(), selectedBuilding.getBuildPercentage());
                    DrawStandardBuildingOptions(selectedBuilding);
                }
            }
        }
        if (!selectionName.Equals(""))
        {
            int topPos = buildAreaHeight + BUTTON_SPACING;
            GUI.Label(new Rect(0, topPos, ORDERS_BAR_WIDTH, SELECTION_NAME_HEIGHT), selectionName);
        }

        GUI.EndGroup();
    }

    private void DrawUpperBar(int enemyCount)
    {
        GUI.skin = resourceSkin;
        GUI.BeginGroup(new Rect(0, 0, Screen.width, RESOURCE_BAR_HEIGHT));
        GUI.Box(new Rect(0, 0, Screen.width, RESOURCE_BAR_HEIGHT), "");

        int padding = 7;
        int buttonWidth = ORDERS_BAR_WIDTH - 2 * padding - SCROLL_BAR_WIDTH;
        int buttonHeight = RESOURCE_BAR_HEIGHT - 2 * padding;
        int leftPos = Screen.width - ORDERS_BAR_WIDTH / 2 - buttonWidth / 2 + SCROLL_BAR_WIDTH / 2;
        Rect menuButtonPosition = new Rect(leftPos, padding, buttonWidth, buttonHeight);

        if (GUI.Button(menuButtonPosition, "Menu"))
        {
            PlayClick();

            Time.timeScale = 0.0f;
            PauseMenu pauseMenu = GetComponent<PauseMenu>();
            if (pauseMenu) pauseMenu.enabled = true;
            UserInput userInput = player.GetComponent<UserInput>();
            if (userInput) userInput.enabled = false;
        }

        Rect attackModeIndicatorPos = new Rect(20, 10, buttonWidth, buttonHeight);
        if (targetManager.InMultiMode)
        {
            GUI.TextArea(attackModeIndicatorPos, "Mutli Attack", multiModeStyle);
        }
        else
        {
            GUI.TextArea(attackModeIndicatorPos, "Single Attack", singleModeStyle);
        }

        Rect enemyCounterPos = new Rect(150, 10, buttonWidth, buttonHeight);
        GUI.TextArea(enemyCounterPos, "Enemy count: " + enemyCount.ToString());

        GUI.EndGroup();
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

    private void DrawActions(string[] actions)
    {
        GUIStyle buttons = new GUIStyle();
        buttons.hover.background = buttonHover;
        buttons.active.background = buttonClick;
        GUI.skin.button = buttons;
        int numActions = actions.Length;
        //define the area to draw the actions inside
        GUI.BeginGroup(new Rect(0, 0, ORDERS_BAR_WIDTH, buildAreaHeight));
        //draw scroll bar for the list of actions if need be
        if (numActions >= MaxNumRows(buildAreaHeight)) DrawSlider(buildAreaHeight, numActions / 2.0f);
        //display possible actions as buttons and handle the button click for each
        for (int i = 0; i < numActions; i++)
        {
            int column = i % 2;
            int row = i / 2;
            Rect pos = GetButtonPos(row, column);
            Texture2D action = ResourceManager.GetBuildImage(actions[i]);
            if (action)
            {
                //create the button and handle the click of that button
                if (GUI.Button(pos, action))
                {
                    if (player.SelectedObject)
                    {
                        PlayClick();
                        player.SelectedObject.PerformAction(actions[i]);
                    }
                }
            }
        }
        GUI.EndGroup();
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

    private void DrawPlayerDetails()
    {
        GUI.skin = playerDetailsSkin;
        GUI.BeginGroup(new Rect(0, 0, Screen.width, Screen.height));
        float height = ResourceManager.TextHeight;
        float leftPos = ResourceManager.Padding;
        float topPos = Screen.height - height - ResourceManager.Padding;
        Texture2D avatar = PlayerManager.GetPlayerAvatar();
        if (avatar)
        {
            //we want the texture to be drawn square at all times
            GUI.DrawTexture(new Rect(leftPos, topPos, height, height), avatar);
            leftPos += height + ResourceManager.Padding;
        }
        float minWidth = 0, maxWidth = 0;
        string playerName = PlayerManager.GetPlayerName();
        playerDetailsSkin.GetStyle("label").CalcMinMaxWidth(new GUIContent(playerName), out minWidth, out maxWidth);
        GUI.Label(new Rect(leftPos, topPos, maxWidth, height), playerName);
        GUI.EndGroup();
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
