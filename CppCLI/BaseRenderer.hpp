// C++/CLR
#include <msclr/marshal_cppstd.h>
using namespace System;

namespace CppCLI {
	/* 渲染器基类 */
	public ref class BaseRenderer {
	public:
		int maxIteration = 100;	// 最大迭代次数
		float infRange = 3.0f;	// 无穷大判定阈值
		float begin_r = 0.0f;	// 迭代起点
		float begin_i = 0.0f;
		String^ code = "z**2+c";
		virtual void RenderMandelbrot(int zoom, int idxX, int idxY) = 0;
		virtual void RenderJulia(int zoom, int idxX, int idxY) = 0;
	};
}