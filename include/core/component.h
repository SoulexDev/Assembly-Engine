#pragma once
#include "camera.h"
#include "transform.h"

#include <memory>

class Component {
public:
	Transform* objectTransform;

	virtual void init();
	virtual void update();
	virtual void late_update();
	virtual void fixed_update();
	virtual void pre_draw();
	virtual void on_draw();
	virtual void post_draw();
	virtual void on_enable();
	virtual void on_disable();
	virtual void destroy();
};