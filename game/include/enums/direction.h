#pragma once
#include <cstdint>

enum Direction : std::uint8_t {
	Direction_None = 0,
	Direction_Forward = 1,
	Direction_Right = 2,
	Direction_Back = 4,
	Direction_Left = 8,
	Direction_Up = 16,
	Direction_Down = 32,

	Direction_XAxis = Direction_Right | Direction_Left,
	Direction_YAxis = Direction_Up | Direction_Down,
	Direction_ZAxis = Direction_Forward | Direction_Back,

	Direction_XYAxis = Direction_XAxis | Direction_YAxis,
	Direction_XZAxis = Direction_XAxis | Direction_ZAxis,

	Direction_YZAxis = Direction_YAxis | Direction_Forward,

	Direction_All = Direction_XAxis | Direction_YAxis | Direction_ZAxis
};