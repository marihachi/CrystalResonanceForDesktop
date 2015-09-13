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

	// X座標を設定します
	void SetX(int value)
	{
		X = value;
	}

	// Y座標を取得します
	int GetY()
	{
		return Y;
	}
	// Y座標を設定します
	void SetY(int value)
	{
		Y = value;
	}
};