#pragma once

#include "../StandardInclude.hpp"
#include "../EntityInclude.hpp"
#include "../HelperInclude.hpp"

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
		if (IsInitial)
		{
			IsInitial = false;

			LogoHandle = LoadGraph("Image/logo.png", 1);
			FontHandle = CreateFontToHandle("メイリオ", 25, 5, DX_FONTTYPE_ANTIALIASING_8X8);

			// メニュー
			auto itemCenter = (Core::GetInstance().ScreenSize / 2).GetWidthHeightAsPoint();

			itemCenter.AddY(80);
			MenuItemStart = MenuItem::BuildMenuItem(itemCenter, Size((Core::GetInstance().ScreenSize / 3).GetWidth(), 40), "Start", FontHandle);

			itemCenter.AddY(60);
			MenuItemSetting = MenuItem::BuildMenuItem(itemCenter, Size((Core::GetInstance().ScreenSize / 3).GetWidth(), 40), "Setting", FontHandle);

			itemCenter.AddY(60);
			MenuItemEnd = MenuItem::BuildMenuItem(itemCenter, Size((Core::GetInstance().ScreenSize / 3).GetWidth(), 40), "End", FontHandle);
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
};