
public class BuildingAction : Action
{
    public override void Act(StateController baseController)
    {
        if (!(baseController is BuildingStateController)) return;

        var controller = (BuildingStateController)baseController;

        DoAction(controller);
    }

    protected virtual void DoAction(BuildingStateController controller)
    {
        // the method is to be overriden
    }
}