#include "../EntityInclude.hpp"

// サイズを表します
class Size
{
private:
	int Width;
	int Height;

public:
	Size();

	// 新しいインスタンスを初期化します
	Size(int width, int height)
	{
		Width = width;
		Height = height;
	}

	Size operator * (Size& a)
	{
		return Size(Width * a.GetWidth(), Height * a.GetHeight());
	}

	Size operator / (Size& a)
	{
		return Size(Width / a.GetWidth(), Height / a.GetHeight());
	}

	Size operator * (int a)
	{
		return Size(Width * a, Height * a);
	}

	Size operator / (int a)
	{
		return Size(Width / a, Height / a);
	}

	// 幅を取得します
	int GetWidth()
	{
		return Width;
	}

	// 高さを設定します
	int GetHeight()
	{
		return Height;
	}

	// 座標として取り出します
	Point ToPoint()
	{
		return Point(Width, Height);
	}

	// 幅を設定します
	void SetWidth(int value)
	{
		Width = value;
	}

	// 高さを設定します
	void SetHeight(int value)
	{
		Height = value;
	}


	// 幅に値を加算します
	void AddWidth(int value)
	{
		Width += value;
	}

	// 高さに値を加算します
	void AddHeight(int value)
	{
		Height += value;
	}
};