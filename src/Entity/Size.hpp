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