public class Tanker : MeleeUnit {

	public override bool CanAttack()
	{
		return true;
	}

    public override bool IsMajor()
    {
        return true;
    }
}
