// C++/CLR
#include <msclr/marshal_cppstd.h>
using namespace System;

namespace CppCLI {
	/* ��Ⱦ������ */
	public ref class BaseRenderer {
	public:
		int maxIteration = 100;	// ����������
		float infRange = 3.0f;	// ������ж���ֵ
		float begin_r = 0.0f;	// �������
		float begin_i = 0.0f;
		String^ code = "z**2+c";
		virtual void RenderMandelbrot(int zoom, int idxX, int idxY) = 0;
		virtual void RenderJulia(int zoom, int idxX, int idxY) = 0;
	};
}