using System;

public struct T_GirdPosition
{
    public int x;
    public int z;

    public T_GirdPosition(int x, int z)
    {
        this.x = x;
        this.z = z;
    }

    public override bool Equals(object obj)
    {
        return obj is T_GirdPosition position &&
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

    public static bool operator ==(T_GirdPosition a, T_GirdPosition b)
    {
        return a.x == b.x && a.z == b.z;
    }

    public static bool operator !=(T_GirdPosition a, T_GirdPosition b)
    {
        return !(a == b);
    }

    public static T_GirdPosition operator +(T_GirdPosition a, T_GirdPosition b)
    {
        return new T_GirdPosition(a.x + b.x, a.z + b.z);
    }
    public static T_GirdPosition operator -(T_GirdPosition a, T_GirdPosition b)
    {
        return new T_GirdPosition(a.x - b.x, a.z - b.z);
    }
}
