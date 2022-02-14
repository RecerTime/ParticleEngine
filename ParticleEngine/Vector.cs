using System;
using System.Drawing;

public class Vector
{
    public Vector(double x, double y)
    {
        X = x;
        Y = y;
    }

    public Vector()
    {

    }


    public double X { get; set; }
    public double Y { get; set; }

    public double Magnitude()
    {
        return Math.Sqrt(X * X + Y * Y);
    }

    public override string ToString()
    {
        return "X: " + X.ToString() + ", Y: " + Y.ToString();
    }

    public void AddVector(Vector vector)
    {
        this.X += vector.X;
        this.Y += vector.Y;
    }

    public bool IsOutside(Size bounds)
    {
        bool inside = false;

        if (this.X > bounds.Width)
        {
            inside = true;
        }
        else if(this.X < 0)
        {
            inside = true;
        }

        if (this.Y > bounds.Width)
        {
            inside = true;
        }
        else if (this.Y < 0)
        {
            inside = true;
        }

        return inside;
    }
}
