using System;


public interface IUserAction
{
	public void Restart();
    public bool GetIsGameOver();
    public bool GetWinFlag();
}