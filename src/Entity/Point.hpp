#pragma once

#include "../EntityInclude.hpp"

// 座標位置を表します
class Point
{
private:
	int _X;
	int _Y;

public:
	Point() { }

	// 新しいインスタンスを初期化します
	Point(int x, int y)
	{
		_X = x;
		_Y = y;
	}

	Point operator + (Point& a)
	{
		return Point(_X + a.X(), _Y + a.Y());
	}

	Point operator - (Point& a)
	{
		return Point(_X - a.X(), _Y - a.Y());
	}

	Point operator * (Point& a)
	{
		return Point(_X * a.X(), _Y * a.Y());
	}

	Point operator / (Point& a)
	{
		return Point(_X / a.X(), _Y / a.Y());
	}

	Point operator * (int a)
	{
		return Point(_X * a, _Y * a);
	}

	Point operator / (int a)
	{
		return Point(_X / a, _Y / a);
	}

	Point operator += (Point& a)
	{
		_X += a.X();
		_Y += a.Y();
		return *this;
	}

	Point operator -= (Point& a)
	{
		_X -= a.X();
		_Y -= a.Y();
		return *this;
	}

	Point operator *= (Point& a)
	{
		_X *= a.X();
		_Y *= a.Y();
		return *this;
	}

	Point operator /= (Point& a)
	{
		_X /= a.X();
		_Y /= a.Y();
		return *this;
	}

	Point operator *= (int a)
	{
		_X *= a;
		_Y *= a;
		return *this;
	}

	Point operator /= (int a)
	{
		_X /= a;
		_Y /= a;
		return *this;
	}

	bool operator >= (Point& a)
	{
		return _X >= a.X() && _Y >= a.Y();
	}

	bool operator <= (Point& a)
	{
		return _X <= a.X() && _Y <= a.Y();
	}

	bool operator == (Point& a)
	{
		return _X == a.X() && _Y == a.Y();
	}

	// _X座標を取得します
	int X()
	{
		return _X;
	}

	// _Y座標を取得します
	int Y()
	{
		return _Y;
	}

	// _X座標を設定します
	void X(int value)
	{
		_X = value;
	}

	// _Y座標を設定します
	void Y(int value)
	{
		_Y = value;
	}

	// _X座標に値を加算します
	void AddX(int value)
	{
		_X += value;
	}

	// _Y座標に値を加算します
	void AddY(int value)
	{
		_Y += value;
	}
};