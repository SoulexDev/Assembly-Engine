#pragma once

class Time {
protected:
	static float lastFrameTime;
public:
	/// <summary>
	/// Time since program start
	/// </summary>
	static float time;

	/// <summary>
	/// Time since last frame
	/// </summary>
	static float deltaTime;
	//TODO: implement
	static float fixedDeltaTime;
	static void update();
};