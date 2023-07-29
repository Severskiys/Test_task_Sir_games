using Utils.StateMachine;

namespace Gameplay.PlayerScripts.States
{
    public class DeathState : IState
    {
        private readonly Player _player;

        public DeathState(Player player)
        {
            _player = player;
        }

        public void Tick()
        {
        }

        public void OnEnter()
        {
            _player.ProcessDeath();
        }

        public void OnExit()
        {
        }
    }
}