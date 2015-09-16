#pragma once

#include "../StandardInclude.hpp"
#include "../EntityInclude.hpp"

// 入力についての操作を提供します
class InputHelper
{
public: static InputHelper &Instance() { static InputHelper instance; return instance; }
private: InputHelper() { }

public:
	// キーボードの入力時間
	int Key[256];

	// マウス左ボタン
	int MouseLeft;

	// マウス右ボタン
	int MouseRight;

	// マウス回転量
	int MouseWheel;

	// マウスポインタ座標
	Point MousePos;

	// キーボードの入力時間を
	void UpdateKeyInputTime()
	{
		char state[256];
		GetHitKeyStateAll(state);

		for (int i = 0; i < 256; i++)
		{
			if (state[i] == 1)
				Key[i]++;
			else
				Key[i] = 0;
		}
	}

	// マウスの入力状態
	void UpdateMouseInputTime()
	{
		int buf = GetMouseInput();

		if ((buf & MOUSE_INPUT_LEFT) != 0)
			MouseLeft++;
		else
			MouseLeft = 0;

		if ((buf & MOUSE_INPUT_RIGHT) != 0)
			MouseRight++;
		else
			MouseRight = 0;

		int temp[2];
		GetMousePoint(&temp[0], &temp[1]);
		MousePos = Point(temp[0], temp[1]);

		// オーバーフロー防止処理
		if (MouseWheel > 2147483600 || MouseWheel < -2147483600)
			MouseWheel = (int)(MouseWheel / fabs((float)MouseWheel));

		// 逆向きに動かしたらリセット（途中で向きを変えたときに必要な移動量を等しくするため）
		auto wheelRotVol = GetMouseWheelRotVol();

		if ((MouseWheel > 0 && wheelRotVol < 0) || (MouseWheel < 0 && wheelRotVol > 0))
			MouseWheel = 0;

		MouseWheel += wheelRotVol;
	}
};