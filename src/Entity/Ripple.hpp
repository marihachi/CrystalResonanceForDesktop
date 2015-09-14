#pragma once

#include "../StandardInclude.hpp"
#include "../EntityInclude.hpp"

class Ripple
{
public:
	Point Location;
	int Radius;

	// 新しいインスタンスを初期化します
	Ripple(Point location, int radius = 0)
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