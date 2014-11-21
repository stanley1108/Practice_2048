using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//
public class FSM {	
	private Dictionary<string, State> m_stateMap = new Dictionary<string, State>();
	
	State m_currState = null;
	State m_nextState = null;
	
	// Update is called once per frame
	public void UpdateFSM () {
		if(m_nextState != null)
		{
			if(m_currState != null)
				m_currState.LeaveState(this);

			m_currState = m_nextState;
			m_currState.EnterState(this);
			m_nextState = null;
		}
		
		m_nextState = m_currState.RunState(this);
		
//		if(m_nextState != null)
//		{
//			m_currState.LeaveState(this);
//		}
	}
	
	public void AddState(State newState)
	{
		m_stateMap.Add(newState.Name, newState);
	}
	
	//allow other class to change state. WARNING: don't use this in the state node which have same entity owner.
	public void ChangeState(State nextState)
	{
		m_nextState = nextState;
	}

	public void ChangeState(string name)
	{
		if(m_stateMap.ContainsKey(name) == false)
		{
			DBG.LogError("ChangeState with wrong state name: " + name);
			return;
		}

		m_nextState = m_stateMap[name];
	}
}
