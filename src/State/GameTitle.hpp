#pragma once

#include "../StandardInclude.hpp"
#include "../EntityInclude.hpp"

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

class GameTitle : public StateInterface
{
public: static GameTitle &GetInstance(void) { static auto instance = GameTitle(); return instance; }
private: GameTitle() { }

public:
	bool IsInitial = true;
	int LogoHandle;
	int FontHandle;
	vector<Ripple> Ripples;

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
		}

		random_device r;

		if ((int)((double)r() / 0xffffffff * 1000) < 5)
		{
			int x = (int)((double)r() / 0xffffffff * 1280);
			int y = (int)((double)r() / 0xffffffff * 720);

			Ripples.push_back(Ripple(Point(x, y), 0));
		}

		for (auto it = Ripples.begin(); it != Ripples.end(); it++)
		{
			(*it).Radius += 3;

			if ((*it).Radius > 1280 * 1.42)
				it = Ripples.erase(it);
		}
	}

	// 描画(常時)
	void Draw(StateEventArgs e)
	{
		if (e.IsActive())
		{
			for (auto ripple : Ripples)
				ripple.Draw();

			DrawFormatString(0, 0, 0xffffff, "波紋数: %d", Ripples.size());

			auto screenRightBottom = Core::GetInstance().ScreenSize.GetWidthHeightAsPoint();
			auto screenCenter = screenRightBottom / 2;

			int imageSize[2];
			GetGraphSize(LogoHandle, &imageSize[0], &imageSize[1]);
			auto logoRightBottom = Point(imageSize[0], imageSize[1]);
			auto logoCenter = logoRightBottom / 2;

			// logo
			auto logoLocation = screenCenter - logoCenter;
			logoLocation.AddY(-150);
			DrawGraph(logoLocation.GetX(), logoLocation.GetY(), LogoHandle, 1);

			// menu
			auto itemCenter = screenCenter;
			Size boxSize(screenRightBottom.GetX() / 3, 40);

			itemCenter.AddY(100);
			DrawMenuItem(itemCenter, boxSize, "Start");

			itemCenter.AddY(60);
			DrawMenuItem(itemCenter, boxSize, "Setting");

			itemCenter.AddY(60);
			DrawMenuItem(itemCenter, boxSize, "End");
		}
	}

	void DrawMenuItem(Point centerPosition, Size boxSize, const char *text)
	{
		SetDrawBlendMode(DX_BLENDMODE_ALPHA, (int)(255 * 0.7));
		boxSize = boxSize / 2;
		DrawBox(centerPosition.GetX() - boxSize.GetWidth(), centerPosition.GetY() - boxSize.GetHeight(), centerPosition.GetX() + boxSize.GetWidth(), centerPosition.GetY() + boxSize.GetHeight(), 0xffffff, 0);
		SetDrawBlendMode(DX_BLENDMODE_NOBLEND, 0);
		DrawStringToHandle(centerPosition.GetX() - GetDrawStringWidthToHandle(text, strlen(text), FontHandle) / 2, centerPosition.GetY() - 25 / 2, text, 0xffffff, FontHandle);
	}
};