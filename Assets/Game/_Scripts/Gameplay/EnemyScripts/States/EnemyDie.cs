using Gameplay.PlayerScripts;
using Utils.StateMachine;

namespace Gameplay.EnemyScripts.States
{
    public class EnemyDie : IState
    {
        private readonly EnemyCharacter _enemyBase;
        private readonly CharacterView _enemyView;

        public EnemyDie(EnemyCharacter enemyBase, CharacterView enemyView)
        {
            _enemyBase = enemyBase;
            _enemyView = enemyView;
        }

        public void Tick()
        {
            
        }

        public void OnEnter()
        {
            _enemyBase.DoOnDeath();
            _enemyView.PlayDeath();
        }

        public void OnExit()
        {
            
        }
    }
}