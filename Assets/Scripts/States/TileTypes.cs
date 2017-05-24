using UnityEngine;
using System.Collections;

public class TileTypes  {

    public enum ESubState { Red, Yellow, Blue, Green, Purple, White }; 
	
	public virtual string Name { get{return "Default"; } }
	
	protected ESubState m_type;

	public ESubState State
	{
		get
		{
			return m_type;
		}
		set
		{
            m_type = value;
		}
	}
}
