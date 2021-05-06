using System;

[Serializable]
public class Player
{
    public string Username;
    public string Password;

    public Player() 
    {

    }

    public Player(string Username, string Password)
    {
        this.Username = Username;
        this.Password = Password;
    }
}
