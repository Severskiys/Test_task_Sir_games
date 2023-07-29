using System;
using System.Collections.Generic;
using UnityEngine;

namespace Utils.StateMachine
{
    public class StateMachine
    {
        private static readonly List<Transition> EmptyTransitions = new(0);
        private readonly List<Transition> _anyTransitions = new();

        private readonly Dictionary<Type, List<Transition>> _transitions = new();

        private List<Transition> _currentTransitions = new();
        public IState CurrentState { get; private set; }

        public void Tick()
        {
            var transition = GetTransition();
            if (transition != null)
                SetState(transition.To);


            CurrentState?.Tick();
        }

        public void Print()
        {
            Debug.Log(CurrentState.GetType().Name);
        }

        public void SetState(IState state)
        {
            if (state == CurrentState)
                return;

            CurrentState?.OnExit();
            CurrentState = state;

            _transitions.TryGetValue(CurrentState.GetType(), out _currentTransitions);
            if (_currentTransitions == null)
                _currentTransitions = EmptyTransitions;

            CurrentState.OnEnter();
        }

        public void AddTransition(IState from, IState to, Func<bool> predicate)
        {
            if (_transitions.TryGetValue(from.GetType(), out var transitions) == false)
            {
                transitions = new List<Transition>();
                _transitions[from.GetType()] = transitions;
            }

            transitions.Add(new Transition(to, predicate));
        }

        public void AddAnyTransition(IState state, Func<bool> predicate)
        {
            _anyTransitions.Add(new Transition(state, predicate));
        }

        private Transition GetTransition()
        {
            foreach (var transition in _currentTransitions)
                if (transition.Condition())
                    return transition;

            foreach (var transition in _anyTransitions)
                if (transition.Condition())
                    return transition;

            return null;
        }

        private class Transition
        {
            public Transition(IState to, Func<bool> condition)
            {
                To = to;
                Condition = condition;
            }

            public Func<bool> Condition { get; }
            public IState To { get; }
        }
    }
}