using Gameplay.PlayerScripts;
using Utils.StateMachine;

namespace Gameplay.EnemyScripts.States
{
    public class StayIdle : IState
    {
        private readonly EnemyCharacter _enemyBase;
        private readonly CharacterView _enemyView;

        public StayIdle(EnemyCharacter enemyBase, CharacterView enemyView)
        {
            _enemyBase = enemyBase;
            _enemyView = enemyView;
        }

        public void Tick()
        {
        }

        public void OnEnter()
        {
            _enemyView.PlayIdle();
        }

        public void OnExit()
        {
        }
    }
}