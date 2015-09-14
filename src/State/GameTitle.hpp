#pragma once

#include "../StandardInclude.hpp"
#include "../EntityInclude.hpp"

#include "../StateInterface.hpp"

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
		auto core = Core::GetInstance();
		auto random = RandomHelper::GetInstance();
		auto input = InputHelper::GetInstance();

		// 初期化
		if (IsInitial)
		{
			IsInitial = false;

			LogoHandle = LoadGraph("Image/logo.png", 1);
			FontHandle = CreateFontToHandle("メイリオ", 25, 5, DX_FONTTYPE_ANTIALIASING_8X8);

			// メニュー

			auto itemCenter = (core.ScreenSize / 2).GetWidthHeightAsPoint();

			itemCenter.AddY(80);
			MenuItemStart = MenuItem::BuildMenuItem(itemCenter, Size((core.ScreenSize / 3).GetWidth(), 40), "Game Start", FontHandle);

			itemCenter.AddY(60);
			MenuItemSetting = MenuItem::BuildMenuItem(itemCenter, Size((core.ScreenSize / 3).GetWidth(), 40), "Setting", FontHandle);

			itemCenter.AddY(60);
			MenuItemEnd = MenuItem::BuildMenuItem(itemCenter, Size((core.ScreenSize / 3).GetWidth(), 40), "Close", FontHandle);
		}

		if ((input.MouseState[0] == 1 || random.Next(0, 1000) < 4) && Ripples.size() <= 6)
		{
			int x = random.Next(0, 1280);
			int y = random.Next(0, 720);

			Ripples.push_back(Ripple(Point(x, y)));
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

		if (MenuItemStart.VerifyOnMouse() && input.MouseState[0] == 1)
		{
			DrawString(0, 0, "Game Start", 0xffffff);
		}

		if (MenuItemSetting.VerifyOnMouse() && input.MouseState[0] == 1)
		{
			DrawString(0, 0, "Setting", 0xffffff);
		}

		if (MenuItemEnd.VerifyOnMouse() && input.MouseState[0] == 1)
		{
			DrawString(0, 0, "Close", 0xffffff);
		}
	}

	// 描画(常時)
	void Draw(StateEventArgs e)
	{
		auto core = Core::GetInstance();

		if (e.IsActive())
		{
			for (auto ripple : Ripples)
				ripple.Draw();

			auto screenRightBottom = core.ScreenSize.GetWidthHeightAsPoint();
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
			if (!MenuItemStart.VerifyOnMouse())
				MenuItemStart.Draw(0xffffff, false, 0xffffff, FontHandle);
			else
				MenuItemStart.Draw(0xffffff, true, GetColor(82, 195, 202), FontHandle);

			if (!MenuItemSetting.VerifyOnMouse())
				MenuItemSetting.Draw(0xffffff, false, 0xffffff, FontHandle);
			else
				MenuItemSetting.Draw(0xffffff, true, GetColor(82, 195, 202), FontHandle);

			if (!MenuItemEnd.VerifyOnMouse())
				MenuItemEnd.Draw(0xffffff, false, 0xffffff, FontHandle);
			else
				MenuItemEnd.Draw(0xffffff, true, GetColor(82, 195, 202), FontHandle);
		}
	}
};