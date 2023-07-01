using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICommand
{
    void Execute(Vector3Int pos);
    void Undo();
    void Redo();
}

