using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandStack : MonoBehaviour
{
    private Stack<ICommand> commandStack = new Stack<ICommand>();
    private Stack<ICommand> undoStack = new Stack<ICommand>();

    public void ExecuteCommand(ICommand command)
    {
        command.Execute();
        commandStack.Push(command);
    }

    public void Undo()
    {
        if (commandStack.Count > 0)
        {
            ICommand command = commandStack.Pop();
            command.Undo();
            undoStack.Push(command);
        }
    }

    public void Redo()
    {
        if (undoStack.Count > 0)
        {
            ICommand command = undoStack.Pop();
            command.Execute();
            commandStack.Push(command);
        }
    }
}
