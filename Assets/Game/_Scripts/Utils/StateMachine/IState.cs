namespace Utils.StateMachine
{
    public interface IState
    {
        void Tick();
        void OnEnter();
        void OnExit();
    }
}