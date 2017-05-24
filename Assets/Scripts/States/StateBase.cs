using UnityEngine;
using System.Collections;

public class StateBase  {
	
	public enum ESubState { Playing, Pause, Menu, LevelWon, LevelLost }; 
	
	public virtual string Name { get{return "Default"; } }
	
	protected ESubState m_CurrentState;
	
	private void Pause()
	{
		Time.timeScale = 0.0f;
	}

	private void Playing()
	{
		Time.timeScale = 1.0f;
		RootController.Instance.EnableControls ();
	}
	private void Menu()
	{

	}
	private void EndScreen()
	{
		
	}

	public virtual void Awake()
	{
		SetAudioListenerVolume();
	}
	
	/// <summary>
	/// Start this instance.
	/// </summary>
	public virtual void Start()
	{
		State = ESubState.Pause;
	}
	
	/// <summary>
	/// Update this instance.
	/// </summary>
	public virtual void Update()
	{

	}

	

	protected virtual void SetCursor(bool cursor)
	{
		Cursor.visible = cursor;
	}
	

	public override string ToString ()
	{
		return string.Format ("[StateBase: Name={0}, State={1}]", Name, State);
	}	

	protected void SetAudioListenerVolume()
	{
		AudioListener.volume = 1f;
	}
	#region properties
	public ESubState State
	{
		get
		{
			return m_CurrentState;
		}
		set
		{
			if ( value == ESubState.Pause )
			{
				Pause();
			}
			else if ( value == ESubState.Playing )
			{
				Playing();
			}
			else if ( value == ESubState.Menu )
			{

			}
			else if ( value == ESubState.LevelWon )
			{

			}
			else if ( value == ESubState.LevelLost )
			{
			}

			m_CurrentState = value;
			
			Debug.Log( this );
		}
	}
	
	#endregion
	
}
