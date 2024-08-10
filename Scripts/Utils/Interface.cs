public interface IState
{
    public void Enter();
    public void Exit();
    public void Update();
}

public interface IStatus
{
    public void Apply(StatusHandler handler, SpecialAttack specialAttack, Enemy target);
}

public interface ISkill
{
    public void UseSkill();
    public void EndSkill();
}