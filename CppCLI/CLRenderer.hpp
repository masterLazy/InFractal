#pragma once

// STD C++
#define _CRT_SECURE_NO_WARNINGS
#include <vector>

// OpenCL
#define CL_TARGET_OPENCL_VERSION 300

#include <CL/opencl.hpp>
#pragma comment(lib, "OpenCL.lib")

// Image IO
//#define STB_IMAGE_WRITE_IMPLEMENTATION
//#include <stb_image_write.h>

// C++/CLR
#include <msclr/marshal_cppstd.h>
using namespace System;

#include "CLDevice.hpp"
#include "BaseRenderer.hpp"

namespace CppCLI {
	/* OpenCL 渲染器 */
	public ref class CLRenderer :BaseRenderer {
		// 每个渲染块的大小
		int chunkSize;

		CLDevice^ device;
		cl::Context* ctx;
		cl::CommandQueue* cmdQueue;
		cl::Program* program;
		cl::Kernel* kernelM, * kernelJ;
		String^ lastError;

	public:
		CLRenderer(int _chunkSize, CLDevice^ _device) {
			chunkSize = _chunkSize;
			device = _device;
		}
	};
}