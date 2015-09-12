// À•W‚ğ•\‚µ‚Ü‚·
class Point
{
private:
	int X;
	int Y;

public:
	Point(int x, int y)
	{
		X = x;
		Y = y;
	}

	Point operator + (Point& a)
	{
		return Point(X + a.GetX(), Y + a.GetY());
	}

	Point operator - (Point& a)
	{
		return Point(X - a.GetX(), Y - a.GetY());
	}

	Point operator += (Point& a)
	{
		X += a.GetX();
		Y += a.GetY();
		return *this;
	}

	int GetX()
	{
		return X;
	}
	void SetX(int value)
	{
		X = value;
	}

	int GetY()
	{
		return Y;
	}
	void SetY(int value)
	{
		Y = value;
	}
};