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

	Size operator + (Size& a)
	{
		return Size(Width + a.GetWidth(), Width + a.GetHeight());
	}

	Size operator - (Size& a)
	{
		return Size(Width - a.GetWidth(), Height - a.GetHeight());
	}

	Size operator += (Size& a)
	{
		Width += a.GetWidth();
		Height += a.GetHeight();
		return *this;
	}

	Size operator -= (Size& a)
	{
		Width -= a.GetWidth();
		Height -= a.GetHeight();
		return *this;
	}

	// 幅を取得します
	int GetWidth()
	{
		return Width;
	}

	// 幅を設定します
	void SetWidth(int value)
	{
		Width = value;
	}

	// 高さを設定します
	int GetHeight()
	{
		return Height;
	}

	// 高さを設定します
	void SetHeight(int value)
	{
		Height = value;
	}
};