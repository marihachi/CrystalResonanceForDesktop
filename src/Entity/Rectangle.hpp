#include "../StandardInclude.hpp"
#include "../EntityInclude.hpp"

// 矩形を表します
class Rectangle
{
private:
	Size _Size;
	Point _Location;

public:
	// 新しいインスタンスを初期化します
	Rectangle(Point location, Size size)
	{
		_Location = location;
		_Size = size;
	}

	// 矩形の左上の座標を取得します
	Point GetLocationLeftTop()
	{
		return _Location;
	}

	// 矩形の右下の座標を取得します
	Point GetLocationRightBottom()
	{
		return Point(
			_Location.GetX() + _Size.GetWidth(),
			_Location.GetY() + _Size.GetHeight());
	}

	// 矩形のサイズを取得します
	Size GetSize()
	{
		return _Size;
	}

	// 矩形を描画します
	void Draw(unsigned int color, bool fillFlag)
	{
		Point location1 = GetLocationLeftTop();
		Point location2 = GetLocationRightBottom();

		DrawBox(
			location1.GetX(), location1.GetY(),
			location2.GetX(), location2.GetY(),
			color,
			fillFlag ? 1 : 0);
	}
};