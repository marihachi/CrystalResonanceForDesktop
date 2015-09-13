// 座標を表します
class Point
{
private:
	int X;
	int Y;

public:
	Point();

	// 新しいインスタンスを初期化します
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

	Point operator * (Point& a)
	{
		return Point(X * a.GetX(), Y * a.GetY());
	}

	Point operator / (Point& a)
	{
		return Point(X / a.GetX(), Y / a.GetY());
	}

	Point operator * (int a)
	{
		return Point(X * a, Y * a);
	}

	Point operator / (int a)
	{
		return Point(X / a, Y / a);
	}

	Point operator += (Point& a)
	{
		X += a.GetX();
		Y += a.GetY();
		return *this;
	}

	Point operator -= (Point& a)
	{
		X -= a.GetX();
		Y -= a.GetY();
		return *this;
	}

	// X座標を取得します
	int GetX()
	{
		return X;
	}

	// Y座標を取得します
	int GetY()
	{
		return Y;
	}

	// X座標を設定します
	void SetX(int value)
	{
		X = value;
	}

	// Y座標を設定します
	void SetY(int value)
	{
		Y = value;
	}

	// X座標に値を加算します
	void AddX(int value)
	{
		X += value;
	}

	// Y座標に値を加算します
	void AddY(int value)
	{
		Y += value;
	}
};