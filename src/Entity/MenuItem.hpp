#pragma once

#include "../StandardInclude.hpp"
#include "../EntityInclude.hpp"

class MenuItem
{
public:
	// 矩形です
	Rect Box;

	// 文字列です
	string Text;

	// 文字列の位置です
	Point TextLocation;

	MenuItem() { }

	// 新しいインスタンスを初期化します
	MenuItem(Rect box, string text, Point textRelativeLocation)
	{
		Box = box;
		Text = text;
		TextLocation = textRelativeLocation;
	}

	void Draw(int boxColor, int textColor, int fontHandle)
	{
		SetDrawBlendMode(DX_BLENDMODE_ALPHA, (int)(255 * 0.7));
		Box.Draw(boxColor, false);
		SetDrawBlendMode(DX_BLENDMODE_NOBLEND, 0);

		DrawStringToHandle(TextLocation.GetX(), TextLocation.GetY(), Text.c_str(), textColor, fontHandle);
	}

	// メニュー項目を構築します
	static MenuItem BuildMenuItem(Point centerPosition, Size boxSize, const char *text, int fontHandle)
	{
		Rect rect(centerPosition - boxSize.GetWidthHeightAsPoint() / 2, boxSize);
		Size textSize(GetDrawStringWidthToHandle(text, strlen(text), fontHandle), 25);
		auto textLocation = centerPosition - textSize.GetWidthHeightAsPoint() / 2;
		return MenuItem(rect, text, textLocation);
	}
};