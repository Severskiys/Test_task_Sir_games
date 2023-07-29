using _ScriptableObjects.Classes;
using UnityEngine;
using UnityEngine.AI;
using Utils.InputClasses;
using Utils.StateMachine;

namespace Gameplay.PlayerScripts.States
{
    public class MoveState : IState
    {
        private readonly CharacterView _characterView;
        private readonly Player _player;

        public MoveState(Player player,  CharacterView characterView)
        {
            _player = player;
            _characterView = characterView;

        }

        public void Tick()
        {
            
        }
        
        public void OnEnter()
        {
            _characterView.HandsDown();
        }
        
        public void OnExit()
        {

        }
        
        
       
    }
}