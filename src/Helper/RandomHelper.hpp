#pragma once

#include "../StandardInclude.hpp"
#include "../EntityInclude.hpp"

// —””­¶‚É‚Â‚¢‚Ä‚Ì‘€ì‚ğ’ñ‹Ÿ‚µ‚Ü‚·
class RandomHelper
{
public: static RandomHelper &GetInstance(void) { static auto instance = RandomHelper(); return instance; }
private: RandomHelper() { }

public:
	int Next(const int min = 0, const int max = 100)
	{
		random_device rand;
		int temp = max - min;
		return (int)((double)rand() / 0xffffffff * temp + min);
	}
};