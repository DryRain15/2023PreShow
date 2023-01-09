using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Proto.Interfaces
{
    public interface IStateContainer
    {
        IState CurrentState { get; }

        void SetState(IState state, bool doOverride = false);

        void OnChangeState();
    }
}