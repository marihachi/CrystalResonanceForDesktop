#pragma once

#include "../StandardInclude.hpp"

#include "../Entity/Point.hpp"

static class InputHelper
{
public:
	// キーボードの入力時間
	static int KeyState[256];

	// キーボードの入力時間を
	static void UpdateKeyInputTime()
	{
		char state[256];
		GetHitKeyStateAll(state);

		for (int i = 0; i < 256; i++)
		{
			if (state[i] == 1)
				KeyState[i]++;
			else
				KeyState[i] = 0;
		}
	}

	// 0: 左ボタン, 1: 右ボタン, 2: 回転量
	static int MouseState[3];

	static Point MousePos;

	// マウスの入力状態
	static void UpdateMouseInputTime()
	{
		int buf = GetMouseInput();

		if ((buf & MOUSE_INPUT_LEFT) != 0)
			MouseState[0]++;
		else
			MouseState[0] = 0;

		if ((buf & MOUSE_INPUT_RIGHT) != 0)
			MouseState[1]++;
		else
			MouseState[1] = 0;

		int temp[2];
		GetMousePoint(&temp[0], &temp[1]);
		MousePos = Point(temp[0], temp[1]);

		// オーバーフロー防止処理
		if (MouseState[2] > 2147483600 || MouseState[2] < -2147483600)
			MouseState[2] = (int)(MouseState[2] / fabs((float)MouseState[2]));

		// 逆向きに動かしたらリセット（途中で向きを変えたときに必要な移動量を等しくするため）
		auto wheelRotVol = GetMouseWheelRotVol();

		if ((MouseState[2] > 0 && wheelRotVol < 0) || (MouseState[2] < 0 && wheelRotVol > 0))
			MouseState[2] = 0;

		MouseState[2] += wheelRotVol;
	}
};