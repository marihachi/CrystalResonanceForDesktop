#pragma once

// ƒQ[ƒ€‚Ìê–Ê‚ğ•\‚µ‚Ü‚·
class IState
{
public:
	virtual ~IState() {}
	virtual void Update() = 0;
	virtual void Draw() = 0;
};