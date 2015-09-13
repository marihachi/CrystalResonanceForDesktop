#pragma once

#include "../StandardInclude.hpp"
#include "../EntityInclude.hpp"

#include "../StateInterface.hpp"

class GameTitle : public StateInterface
{
public: static GameTitle &GetInstance(void) { static auto instance = GameTitle(); return instance; }
private: GameTitle() { }

private:
	bool IsInitial = true;
	int logoHandle;
	int fontHandle;

public:
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

			logoHandle = LoadGraph("Image/logo.png", 1);
			fontHandle = CreateFontToHandle("メイリオ", 25, 5, DX_FONTTYPE_ANTIALIASING_8X8);
		}
	}

	// 描画(常時)
	void Draw(StateEventArgs e)
	{
		if (e.IsActive())
		{
			auto screenRightBottom = Core::GetInstance().ScreenSize.GetWidthHeightAsPoint();
			auto screenCenter = screenRightBottom / 2;

			int imageSize[2];
			GetGraphSize(logoHandle, &imageSize[0], &imageSize[1]);
			auto logoRightBottom = Point(imageSize[0], imageSize[1]);
			auto logoCenter = logoRightBottom / 2;

			// logo
			auto logoLocation = screenCenter - logoCenter;
			logoLocation.AddY(-150);
			DrawGraph(logoLocation.GetX(), logoLocation.GetY(), logoHandle, 1);

			// menu
			auto temp = screenCenter;
			Size boxSize(screenRightBottom.GetX() / 3, 40);

			temp.AddY(100);
			auto str = "Start";
			SetDrawBlendMode(DX_BLENDMODE_ALPHA, (int)(255 * 0.7));
			DrawBox(temp.GetX() - boxSize.GetWidth() / 2, temp.GetY() - boxSize.GetHeight() / 2, temp.GetX() + boxSize.GetWidth() / 2, temp.GetY() + boxSize.GetHeight() / 2, 0xffffff, 0);
			SetDrawBlendMode(DX_BLENDMODE_NOBLEND, 0);
			DrawStringToHandle(temp.GetX() - GetDrawStringWidthToHandle(str, strlen(str), fontHandle) / 2, temp.GetY() - 25 / 2, str, 0xffffff, fontHandle);

			temp.AddY(60);
			str = "Setting";
			SetDrawBlendMode(DX_BLENDMODE_ALPHA, (int)(255 * 0.7));
			DrawBox(temp.GetX() - boxSize.GetWidth() / 2, temp.GetY() - boxSize.GetHeight() / 2, temp.GetX() + boxSize.GetWidth() / 2, temp.GetY() + boxSize.GetHeight() / 2, 0xffffff, 0);
			SetDrawBlendMode(DX_BLENDMODE_NOBLEND, 0);
			DrawStringToHandle(temp.GetX() - GetDrawStringWidthToHandle(str, strlen(str), fontHandle) / 2, temp.GetY() - 25 / 2, str, 0xffffff, fontHandle);

			temp.AddY(60);
			str = "End";
			SetDrawBlendMode(DX_BLENDMODE_ALPHA, (int)(255 * 0.7));
			DrawBox(temp.GetX() - boxSize.GetWidth() / 2, temp.GetY() - boxSize.GetHeight() / 2, temp.GetX() + boxSize.GetWidth() / 2, temp.GetY() + boxSize.GetHeight() / 2, 0xffffff, 0);
			SetDrawBlendMode(DX_BLENDMODE_NOBLEND, 0);
			DrawStringToHandle(temp.GetX() - GetDrawStringWidthToHandle(str, strlen(str), fontHandle) / 2, temp.GetY() - 25 / 2, str, 0xffffff, fontHandle);
		}
	}
};