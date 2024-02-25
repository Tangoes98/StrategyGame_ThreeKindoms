using System;

[Serializable]
public struct T_GridPosition
{
    public int x;
    public int z;

    public T_GridPosition(int x, int z)
    {
        this.x = x;
        this.z = z;
    }

    public override bool Equals(object obj)
    {
        return obj is T_GridPosition position &&
               x == position.x &&
               z == position.z;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(x, z);
    }

    public override string ToString()
    {
        return $"x: {x}, z: {z}";
    }

    public static bool operator ==(T_GridPosition a, T_GridPosition b)
    {
        return a.x == b.x && a.z == b.z;
    }

    public static bool operator !=(T_GridPosition a, T_GridPosition b)
    {
        return !(a == b);
    }

    public static T_GridPosition operator +(T_GridPosition a, T_GridPosition b)
    {
        return new T_GridPosition(a.x + b.x, a.z + b.z);
    }
    public static T_GridPosition operator -(T_GridPosition a, T_GridPosition b)
    {
        return new T_GridPosition(a.x - b.x, a.z - b.z);
    }
}
