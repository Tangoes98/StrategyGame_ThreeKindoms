using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class T_GridData
{
    T_GirdPosition _gridPosition;

    public T_GirdPosition GridPosition {get { return _gridPosition;}}


    public T_GridData(T_GirdPosition gp)
    {
        this._gridPosition = gp;




    }

    public override string ToString()
    {
        return _gridPosition.ToString();
    }

}
