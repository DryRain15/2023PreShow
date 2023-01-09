namespace Proto.Interfaces
{
    public interface IState
    {
        string StateName { get; set; }
        public IState YieldState { get; set; }
        bool IsYield { get; set; }
        bool IsStarted { get; set; }

        void OnStartState();

        void OnState();

        void OnEndState();
    }
}