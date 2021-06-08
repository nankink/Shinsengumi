public abstract class State
{
    protected Player_Brain character;
    protected StateMachine stateMachine;


    protected State(Player_Brain character, StateMachine stateMachine)
    {
        this.character = character;
        this.stateMachine = stateMachine;
    }

    public virtual void Enter()
    {
        DisplayState(this);
    }

    public virtual void HandleInput()
    {

    }

    public virtual void LogicUpdate()
    {

    }

    public virtual void PhysicsUpdate()
    {

    }

    public virtual void Exit()
    {

    }

    void DisplayState(State enteredState)
    {
       character.Helpers.DisplayText(TextFieldUI.Current_State, enteredState.ToString());
    }
}

