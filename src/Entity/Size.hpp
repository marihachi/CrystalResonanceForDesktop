#pragma once

#include "../EntityInclude.hpp"

// 縦横の大きさを表します
class Size
{
private:
	int _Width;
	int _Height;

public:
	Size() { }

	// 新しいインスタンスを初期化します
	Size(int width, int height)
	{
		_Width = width;
		_Height = height;
	}

	Size operator * (Size& a)
	{
		return Size(_Width * a.Width(), _Height * a.Height());
	}

	Size operator / (Size& a)
	{
		return Size(_Width / a.Width(), _Height / a.Height());
	}

	Size operator * (int a)
	{
		return Size(_Width * a, _Height * a);
	}

	Size operator / (int a)
	{
		return Size(_Width / a, _Height / a);
	}

	// 幅を取得します
	int Width()
	{
		return _Width;
	}

	// 高さを設定します
	int Height()
	{
		return _Height;
	}

	// 幅と高さを座標として取得します
	Point WidthHeightAsPoint()
	{
		return Point(_Width, _Height);
	}

	// 幅を設定します
	void Width(int value)
	{
		_Width = value;
	}

	// 高さを設定します
	void Height(int value)
	{
		_Height = value;
	}


	// 幅に値を加算します
	void AddWidth(int value)
	{
		_Width += value;
	}

	// 高さに値を加算します
	void AddHeight(int value)
	{
		_Height += value;
	}
};