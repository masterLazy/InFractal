// STD C++
#define _CRT_SECURE_NO_WARNINGS
#include <vector>

// Image IO
#define STB_IMAGE_WRITE_IMPLEMENTATION
#include <stb_image_write.h>

// C++/CLR
#include <msclr/marshal_cppstd.h>
using namespace System;

#include "BaseRenderer.hpp"

namespace CppCLI {
	/* CPU ��Ⱦ�� */
	public ref class CpuRenderer : BaseRenderer {
		// ÿ����Ⱦ��Ĵ�С
		int chunkSize;
		String^ lastError;

	public:
		CpuRenderer(int _chunkSize) {
			chunkSize = _chunkSize;
		}
	};
}