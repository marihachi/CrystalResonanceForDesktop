#pragma once

#include "../StandardInclude.hpp"
#include "../EntityInclude.hpp"
#include "../HelperInclude.hpp"

// ボタンのスタイルを表します
class ButtonStyle
{
private:
	int _Color;
	bool _IsFill;
	int _TextColor;

public:
	ButtonStyle() { }

	// 新しいインスタンスを初期化します
	ButtonStyle(int color, bool isFill, int textColor)
	{
		_Color = color;
		_IsFill = isFill;
		_TextColor = textColor;
	}

	int Color()
	{
		return _Color;
	}

	bool IsFill()
	{
		return _IsFill;
	}

	int TextColor()
	{
		return _TextColor;
	}
};

// 四角形と文字列から構成されるボタンを表します
class Button
{
private:
	Rect _Box;
	string _Text;
	Point _TextLocation;
	int _FontHandle;
	ButtonStyle _NormalStyle;
	ButtonStyle _HoverStyle;

public:
	Button() { }

	// 新しいインスタンスを初期化します
	Button(Rect box, string text, Point textLocation, int fontHandle, ButtonStyle normalStyle, ButtonStyle hoverStyle)
	{
		_Box = box;
		_Text = text;
		_TextLocation = textLocation;
		_FontHandle = fontHandle;
		_NormalStyle = normalStyle;
		_HoverStyle = hoverStyle;
	}

	// ボタンを描画します
	void Draw()
	{
		ButtonStyle style;

		if (!IsOnMouse())
			style = _NormalStyle;
		else
			style = _HoverStyle;

		SetDrawBlendMode(DX_BLENDMODE_ALPHA, (int)(255 * 0.7));
		_Box.Draw(style.Color(), style.IsFill());
		SetDrawBlendMode(DX_BLENDMODE_NOBLEND, 0);

		DrawStringToHandle(_TextLocation.X(), _TextLocation.Y(), _Text.c_str(), style.TextColor(), _FontHandle);
	}

	// 項目上にマウスポインタがあるかどうかを検証します
	bool IsOnMouse()
	{
		auto mp = InputHelper::GetInstance().MousePos;
		auto p1 = _Box.LocationLeftTop();
		auto p2 = _Box.LocationRightBottom();

		return mp >= p1 && mp <= p2;
	}

	// ボタンを構築してインスタンスを生成します
	static Button BuildButton(Point centerPosition, Size boxSize, const char *text, int fontHandle, ButtonStyle normalStyle, ButtonStyle hoverStyle)
	{
		auto rect = Rect(centerPosition - boxSize.GetWidthHeightAsPoint() / 2, boxSize);
		auto textSize = Size(GetDrawStringWidthToHandle(text, strlen(text), fontHandle), 25);
		auto textLocation = centerPosition - textSize.GetWidthHeightAsPoint() / 2;

		return Button(rect, text, textLocation, fontHandle, normalStyle, hoverStyle);
	}
};