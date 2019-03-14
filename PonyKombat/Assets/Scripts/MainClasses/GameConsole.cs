using System;
using System.Collections;
using System.Collections.Generic;

public static class GameConsole
{
    private static string[] UserCommands =
    {
        "g_restore_hp_player",
        "g_restore_hp_ai"
    };

    private static List<string> messages = new List<string>(){};
    public static event Action<string> OnNewMessage;
    public static event Action<string> OnUserCommand;
	
    public static void AddMessage(string message, bool IsUser = false)
    {
        messages.Add(message);
        OnNewMessage?.Invoke(message);
        if(IsUser)
        {
            for(int i = 0; i < UserCommands.Length; i++)
                if(message == UserCommands[i])
                {
                    OnUserCommand?.Invoke(message);
                    return;
                }
            AddMessage("Unknown command");
        }
    }

    public static string GetMessage(int i)
    {
        if(i < 0 || i >= messages.Count)
            throw new ArgumentOutOfRangeException("Incorrect message index");
        return messages[i];
    }

    public static List<string> GetLastMessages(int count)
    {
        if(count < 0)
            throw new ArgumentOutOfRangeException("Less than zero request");
        if(messages.Count < count)
            return messages;
        List<string> output = new List<string>() { };
        for(int i = messages.Count - count; i < messages.Count; i++)
        {
            output.Add(messages[i]);
        }
        return output;
    }

    public static int GetMessagesCount
    { get { return messages.Count; } }
}