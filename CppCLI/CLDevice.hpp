#pragma once

#define CL_TARGET_OPENCL_VERSION 300
#include <CL/opencl.hpp>

#include <msclr/marshal_cppstd.h>
using namespace System;

namespace CppCLI {
	public enum class CLDeviceType {
		ALL, CPU, GPU
	};

	public ref class CLDevice :Object {
		cl::Device* d;
	public:
		CLDevice() :d(nullptr) {}
		CLDevice(cl::Device device) {
			d = new cl::Device();
			*d = device;
		}
		~CLDevice() {
			this->!CLDevice();
		}
		!CLDevice() {
			delete d;
		}

		virtual String^ ToString() override {
			if (d == nullptr) {
				return "Null";
			}
			if (d->getInfo<CL_DEVICE_TYPE>() & CL_DEVICE_TYPE_GPU) {
				return msclr::interop::marshal_as<System::String^>(d->getInfo<CL_DEVICE_NAME>() + " (GPU)");
			} else if (d->getInfo<CL_DEVICE_TYPE>() & CL_DEVICE_TYPE_CPU) {
				return msclr::interop::marshal_as<System::String^>(d->getInfo<CL_DEVICE_NAME>() + " (CPU)");
			} else {
				return msclr::interop::marshal_as<System::String^>(d->getInfo<CL_DEVICE_NAME>());
			}
		}

		bool IsDoubleFpAvailable() {
			return d->getInfo<CL_DEVICE_DOUBLE_FP_CONFIG>();
		}
	};
}