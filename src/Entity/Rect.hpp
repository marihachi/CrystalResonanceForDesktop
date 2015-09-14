#pragma once

#include "../StandardInclude.hpp"
#include "../EntityInclude.hpp"

// 矩形を表します
class Rect
{
private:
	Size _Size;
	Point _Location;

public:
	Rect() { }

	// 新しいインスタンスを初期化します
	Rect(Point location, Size size)
	{
		_Location = location;
		_Size = size;
	}

	// 矩形の左上の座標を取得します
	Point LocationLeftTop()
	{
		return _Location;
	}

	// 矩形の右下の座標を取得します
	Point LocationRightBottom()
	{
		return Point(
			_Location.X() + _Size.Width(),
			_Location.Y() + _Size.Height());
	}

	// 矩形のサイズを取得します
	Size Size()
	{
		return _Size;
	}

	// 矩形を描画します
	void Draw(unsigned int color, bool fillFlag)
	{
		Point location1 = LocationLeftTop();
		Point location2 = LocationRightBottom();

		DrawBox(
			location1.X(), location1.Y(),
			location2.X(), location2.Y(),
			color,
			fillFlag ? 1 : 0);
	}
};