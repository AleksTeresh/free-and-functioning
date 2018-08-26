using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RTS;
using Persistence;

public class Building : WorldObject {
    public Texture2D rallyPointImage;

    protected LocalUI localUI;

    public new BuildingStateController stateController;

    public float maxBuildProgress;

    protected Queue<string> buildQueue;
    protected Vector3 rallyPoint;

    private float currentBuildProgress = 0.0f;
    protected Vector3 spawnPoint;

    // audio
    public AudioClip finishedJobSound;
    public float finishedJobVolume = 1.0f;

    public new BuildingStateController GetStateController()
    {
        return stateController;
    }

    protected override void Awake()
    {
        base.Awake();

        buildQueue = new Queue<string>();
        float spawnX = selectionBounds.center.x + transform.forward.x * selectionBounds.extents.x + transform.forward.x * 20;
        float spawnZ = selectionBounds.center.z + transform.forward.z + selectionBounds.extents.z + transform.forward.z * 20;
        spawnPoint = new Vector3(spawnX, 0.0f, spawnZ);

        rallyPoint = spawnPoint;

        stateController = GetComponent<BuildingStateController>();
    }

    protected override void Start()
    {
        base.Start();

        // instantiate localUI
        var localUIObject = Instantiate(ResourceManager.GetUIElement("LocalUI"), transform);
        localUI = localUIObject.GetComponent<LocalUI>();
    }

    protected override void Update()
    {
        base.Update();

        ProcessBuildQueue();
    }

    protected override void InitialiseAudio()
    {
        base.InitialiseAudio();
        if (finishedJobVolume < 0.0f) finishedJobVolume = 0.0f;
        if (finishedJobVolume > 1.0f) finishedJobVolume = 1.0f;
        List<AudioClip> sounds = new List<AudioClip>();
        List<float> volumes = new List<float>();
        sounds.Add(finishedJobSound);
        volumes.Add(finishedJobVolume);
        audioElement.Add(sounds, volumes);
    }

    protected void CreateUnit(string unitName)
    {
        buildQueue.Enqueue(unitName);
    }

    protected void ProcessBuildQueue()
    {
        if (buildQueue.Count > 0)
        {
            currentBuildProgress += Time.deltaTime * ResourceManager.BuildSpeed;
            if (currentBuildProgress > maxBuildProgress)
            {
                if (player)
                {
                    if (audioElement != null) audioElement.Play(finishedJobSound);

                    player.AddUnit(buildQueue.Dequeue(), spawnPoint, rallyPoint, transform.rotation, this);
                }

                currentBuildProgress = 0.0f;
            }
        }
    }

    public override bool CanAddStatus()
    {
        return true;
    }

    public string[] getBuildQueueValues()
    {
        string[] values = new string[buildQueue.Count];
        int pos = 0;
        foreach (string unit in buildQueue) values[pos++] = unit;
        return values;
    }

    public float getBuildPercentage()
    {
        return currentBuildProgress / maxBuildProgress;
    }

    public bool hasSpawnPoint()
    {
        return spawnPoint != ResourceManager.InvalidPosition && rallyPoint != ResourceManager.InvalidPosition;
    }

    public virtual void SetSpawnPoint(Vector3 spawnPoint)
    {
        this.spawnPoint = spawnPoint;
    }

    public override void SetSelection(bool selected, Rect playingArea)
    {
        base.SetSelection(selected, playingArea);
        if (player)
        {
            RallyPoint flag = player.GetComponentInChildren<RallyPoint>();
            if (selected)
            {
                if (flag && player.human && spawnPoint != ResourceManager.InvalidPosition && rallyPoint != ResourceManager.InvalidPosition)
                {
                    flag.transform.localPosition = rallyPoint;
                    flag.transform.forward = transform.forward;
                    flag.Enable();
                }
            }
            else
            {
                if (flag && player.human) flag.Disable();
            }
        }
    }

    public override void SetHoverState(GameObject hoverObject)
    {
        base.SetHoverState(hoverObject);
        //only handle input if owned by a human player and currently selected
        if (player && player.human && currentlySelected)
        {
            if (WorkManager.ObjectIsGround(hoverObject))
            {
                if (hud.GetPreviousCursorState() == CursorState.RallyPoint) hud.SetCursorState(CursorState.RallyPoint);
            }
        }
    }

    public void SetRallyPoint(Vector3 position)
    {
        rallyPoint = position;
        if (player && player.human && currentlySelected)
        {
            RallyPoint flag = player.GetComponentInChildren<RallyPoint>();
            if (flag) flag.transform.localPosition = rallyPoint;
        }
    }

    public new BuildingData GetData()
    {
        var baseData = base.GetData();

        return new BuildingData(
            baseData,
            stateController ? stateController.GetData() : null,
            buildQueue,
            rallyPoint,
            currentBuildProgress,
            spawnPoint
        );
    }

    public void SetData(BuildingData data)
    {
        base.SetData(data);
        Start();

        spawnPoint = data.spawnPoint;
        currentBuildProgress = data.currentBuildProgress;
        rallyPoint = data.rallyPoint;
        buildQueue = data.buildQueue;
        if (data.buildingStateController != null)
        {
            stateController.SetData(data.buildingStateController);
            stateController.building = this;
        }
    }
}
