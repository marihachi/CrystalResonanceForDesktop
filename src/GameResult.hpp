#include "DxLib.h"
#include "IState.hpp"

using namespace std;

class GameResult : IState
{
public: static GameResult &GetInstance(void) { static auto instance = GameResult(); return instance; }
private: GameResult() { }

public:
	// XV
	void Update()
	{

	}

	// •`‰æ
	void Draw()
	{

	}
};