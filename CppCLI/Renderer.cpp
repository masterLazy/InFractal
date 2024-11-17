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
	/* ��Ⱦ�� (�ӿ�) */
	public ref class Renderer {
		// ÿ����Ⱦ��Ĵ�С
		const int chunkSize = 512;
		Renderer^* renderer;
	public:
		// ��ȡ���� OpenCL ƽ̨
		static array<CLDevice^>^ GetDevices(CLDeviceType DeviceType) {
			// ��ȡ OpenCL ƽ̨
			std::vector<cl::Platform> platforms;
			cl::Platform::get(&platforms);
			if (platforms.empty()) {
				return gcnew array<CLDevice^>(0);
			}
			// ��ȡ�����豸
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
			// ת��������
			auto res = gcnew array<CLDevice^>(devices.size());
			for (int i = 0; i < devices.size(); i++) {
				res[i] = gcnew CLDevice(devices[i]);
			}
			return res;
		}
		// OpenCL �Ƿ����
		static bool IsOpenclAvailable() {
			std::vector<cl::Platform> platforms;
			cl::Platform::get(&platforms);
			return platforms.size() > 0;
		}
	};
}