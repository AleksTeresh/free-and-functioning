
public class UnitAction : Action
{
    public override void Act(StateController baseController)
    {
        if (!(baseController is UnitStateController)) return;

        var controller = (UnitStateController)baseController;

        DoAction(controller);
    }

    protected virtual void DoAction(UnitStateController controller)
    {
        // the method is to be overriden
    }
}