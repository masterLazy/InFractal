// STD C++
#define _CRT_SECURE_NO_WARNINGS
#include <vector>

// OpenCL
#define CL_TARGET_OPENCL_VERSION 300
#include <CL/opencl.hpp>
#pragma comment(lib, "OpenCL.lib")

// Image IO
#define STB_IMAGE_WRITE_IMPLEMENTATION
#include <stb_image_write.h>

// C++/CLR
#include <msclr/marshal_cppstd.h>
using namespace System;

#include "CpuRenderer.hpp"
#include "CLRenderer.hpp"

namespace CppCLI {
	/* 渲染器 (接口) */
	public ref class Renderer {
		// 每个渲染块的大小
		const int chunkSize = 512;
		Renderer^* renderer;
	public:
		// 获取所有 OpenCL 平台
		static array<CLDevice^>^ GetDevices(CLDeviceType DeviceType) {
			// 获取 OpenCL 平台
			std::vector<cl::Platform> platforms;
			cl::Platform::get(&platforms);
			if (platforms.empty()) {
				return gcnew array<CLDevice^>(0);
			}
			// 获取所有设备
			std::vector<cl::Device> temp, devices;
			cl_device_type type;
			if (DeviceType == CLDeviceType::CPU) {
				type = CL_DEVICE_TYPE_CPU;
			} else if (DeviceType == CLDeviceType::GPU) {
				type = CL_DEVICE_TYPE_GPU;
			} else {
				type = CL_DEVICE_TYPE_ALL;
			}
			for (auto i : platforms) {
				i.getDevices(type, &temp);
				devices.insert(devices.end(), temp.begin(), temp.end());
			}
			// 转换并返回
			auto res = gcnew array<CLDevice^>(devices.size());
			for (int i = 0; i < devices.size(); i++) {
				res[i] = gcnew CLDevice(devices[i]);
			}
			return res;
		}
		// OpenCL 是否可用
		static bool IsOpenclAvailable() {
			std::vector<cl::Platform> platforms;
			cl::Platform::get(&platforms);
			return platforms.size() > 0;
		}
	};
}