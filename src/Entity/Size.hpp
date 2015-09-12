// サイズを表します
class Size
{
private:
	int Width;
	int Height;

public:
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

	int GetWidth()
	{
		return Width;
	}
	void SetWidth(int value)
	{
		Width = value;
	}

	int GetHeight()
	{
		return Height;
	}
	void SetHeight(int value)
	{
		Height = value;
	}
};