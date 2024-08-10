public class EntityStateMachine
{
    protected IState currentState;

    public Entity Entity;

    public void ChangeState(IState state)
    {
        currentState?.Exit();
        currentState = state;
        currentState?.Enter();
    }

    public void Update()
    {
        currentState?.Update();
    }

    public bool IsEntityState(IState state)
    {
        return currentState == state;
    }
}
