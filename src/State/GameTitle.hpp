#pragma once

#include "../StandardInclude.hpp"
#include "../EntityInclude.hpp"
#include "../HelperInclude.hpp"

#include "../StateInterface.hpp"

class Ripple
{
public:
	Point Location;
	int Radius;

	// 新しいインスタンスを初期化します
	Ripple(Point location, int radius)
	{
		Location = location;
		Radius = radius;
	}

	void Draw()
	{
		SetDrawBlendMode(DX_BLENDMODE_ALPHA, (int)(255 * 0.5));
		DrawCircle(Location.GetX(), Location.GetY(), Radius, 0xffffff, 0);
		SetDrawBlendMode(DX_BLENDMODE_NOBLEND, 0);
	}
};

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
};

class GameTitle : public StateInterface
{
public: static GameTitle &GetInstance(void) { static auto instance = GameTitle(); return instance; }
private: GameTitle() { }

public:
	bool IsInitial = true;
	int LogoHandle;
	int FontHandle;
	vector<Ripple> Ripples;
	MenuItem MenuItemStart;
	MenuItem MenuItemSetting;
	MenuItem MenuItemEnd;

	// 場面名を取得します
	string StateName()
	{
		return "Title";
	}

	// 更新(ターゲット時のみ)
	void Update()
	{
		if (IsInitial)
		{
			IsInitial = false;

			LogoHandle = LoadGraph("Image/logo.png", 1);
			FontHandle = CreateFontToHandle("メイリオ", 25, 5, DX_FONTTYPE_ANTIALIASING_8X8);

			// メニュー
			auto itemCenter = (Core::GetInstance().ScreenSize / 2).GetWidthHeightAsPoint();

			itemCenter.AddY(100);
			MenuItemStart = BuildMenuItem(itemCenter, Size((Core::GetInstance().ScreenSize / 3).GetWidth(), 40), "Start");

			itemCenter.AddY(60);
			MenuItemSetting = BuildMenuItem(itemCenter, Size((Core::GetInstance().ScreenSize / 3).GetWidth(), 40), "Setting");

			itemCenter.AddY(60);
			MenuItemEnd = BuildMenuItem(itemCenter, Size((Core::GetInstance().ScreenSize / 3).GetWidth(), 40), "End");
		}

		random_device r;

		int rVal = (int)((double)r() / 0xffffffff * 1000);
		if ((rVal < 3 && Ripples.size() < 4) || (rVal < 100 && Ripples.size() == 0))
		{
			int x = (int)((double)r() / 0xffffffff * 1280);
			int y = (int)((double)r() / 0xffffffff * 720);

			Ripples.push_back(Ripple(Point(x, y), 0));
		}

		auto it = Ripples.begin();
		while (it != Ripples.end())
		{
			(*it).Radius += 2;

			if ((*it).Radius > 1280 * 1.42)
				it = Ripples.erase(it);
			else
				it++;
		}

		auto input = InputHelper::GetInstance();
	}

	// 描画(常時)
	void Draw(StateEventArgs e)
	{
		if (e.IsActive())
		{
			for (auto ripple : Ripples)
				ripple.Draw();

			auto screenRightBottom = Core::GetInstance().ScreenSize.GetWidthHeightAsPoint();
			auto screenCenter = screenRightBottom / 2;

			int imageSize[2];
			GetGraphSize(LogoHandle, &imageSize[0], &imageSize[1]);
			auto logoRightBottom = Point(imageSize[0], imageSize[1]);
			auto logoCenter = logoRightBottom / 2;

			// ロゴ
			auto logoLocation = screenCenter - logoCenter;
			logoLocation.AddY(-150);
			DrawGraph(logoLocation.GetX(), logoLocation.GetY(), LogoHandle, 1);

			// メニュー
			MenuItemStart.Draw(0xffffff, 0xffffff, FontHandle);
			MenuItemSetting.Draw(0xffffff, 0xffffff, FontHandle);
			MenuItemEnd.Draw(0xffffff, 0xffffff, FontHandle);
		}
	}

	MenuItem BuildMenuItem(Point centerPosition, Size boxSize, const char *text)
	{
		Rect rect(centerPosition - boxSize.GetWidthHeightAsPoint() / 2, boxSize);
		Size textSize(GetDrawStringWidthToHandle(text, strlen(text), FontHandle), 25);
		auto textLocation = centerPosition - textSize.GetWidthHeightAsPoint() / 2;
		return MenuItem(rect, text, textLocation);
	}
};