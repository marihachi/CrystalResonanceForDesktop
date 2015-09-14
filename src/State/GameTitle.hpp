#pragma once

#include "../StandardInclude.hpp"
#include "../EntityInclude.hpp"

#include "../IState.hpp"

// タイトル画面の場面を表します
class GameTitle : public IState
{
public: static GameTitle &GetInstance(void) { static auto instance = GameTitle(); return instance; }
private: GameTitle() { }

private:
	bool _IsInitial = true;
	int _LogoHandle;
	int _FontHandle;
	vector<Ripple> _Ripples;
	Button _StartButton;
	Button _SettingButton;
	Button _CloseButton;

public:
	// 場面名を取得します
	string StateName()
	{
		return "Title";
	}

	// 更新(ターゲット時のみ)
	void Update()
	{
		auto &core = Core::GetInstance();
		auto &random = RandomHelper::GetInstance();
		auto &input = InputHelper::GetInstance();

		// 初期化
		if (_IsInitial)
		{
			_IsInitial = false;

			_LogoHandle = LoadGraph("Image/logo.png", 1);
			_FontHandle = CreateFontToHandle("メイリオ", 25, 5, DX_FONTTYPE_ANTIALIASING_8X8);

			// メニュー

			auto itemCenter = (core.ScreenSize() / 2).GetWidthHeightAsPoint();

			auto buttonSize = Size((core.ScreenSize() / 3).Width(), 40);
			auto normalStyle = ButtonStyle(0xffffff, false, 0xffffff);
			auto hoverStyle = ButtonStyle(0xffffff, true, GetColor(82, 195, 202));

			itemCenter.AddY(80);
			_StartButton = Button::BuildButton(itemCenter, buttonSize, "Game Start", _FontHandle, normalStyle, hoverStyle);

			itemCenter.AddY(60);
			_SettingButton = Button::BuildButton(itemCenter, buttonSize, "Setting", _FontHandle, normalStyle, hoverStyle);

			itemCenter.AddY(60);
			_CloseButton = Button::BuildButton(itemCenter, buttonSize, "Quit", _FontHandle, normalStyle, hoverStyle);
		}

		if ((input.MouseLeft == 1 || random.Next(0, 1000) < 4) && _Ripples.size() <= 6)
		{
			int x = random.Next(0, 1280);
			int y = random.Next(0, 720);

			_Ripples.push_back(Ripple(Point(x, y)));
		}

		auto it = _Ripples.begin();
		while (it != _Ripples.end())
		{
			(*it).AddRadius(2);

			if ((*it).Radius() > 1280 * 1.42)
				it = _Ripples.erase(it);
			else
				it++;
		}

		if (_StartButton.IsOnMouse() && input.MouseLeft == 1)
		{
			//core.SetNowStateName("MusicSelect");
			core.SetNowStateName("Main");
		}

		if (_SettingButton.IsOnMouse() && input.MouseLeft == 1)
		{
			core.SetNowStateName("Setting");
		}

		if (_CloseButton.IsOnMouse() && input.MouseLeft == 1)
		{
			core.SetNowStateName("Quit");
		}
	}

	// 描画(常時)
	void Draw(StateEventArgs e)
	{
		auto core = Core::GetInstance();

		if (e.IsActive())
		{
			for (auto ripple : _Ripples)
				ripple.Draw();

			auto screenRightBottom = core.ScreenSize().GetWidthHeightAsPoint();
			auto screenCenter = screenRightBottom / 2;

			int imageSize[2];
			GetGraphSize(_LogoHandle, &imageSize[0], &imageSize[1]);
			auto logoRightBottom = Point(imageSize[0], imageSize[1]);
			auto logoCenter = logoRightBottom / 2;

			// ロゴ
			auto logoLocation = screenCenter - logoCenter;
			logoLocation.AddY(-150);
			DrawGraph(logoLocation.X(), logoLocation.Y(), _LogoHandle, 1);

			// メニュー
			_StartButton.Draw();
			_SettingButton.Draw();
			_CloseButton.Draw();
		}
	}
};