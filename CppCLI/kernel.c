typedef <float> cfloat;
const int chunkSize = <chunkSize>;
// 复数支持
typedef struct {
	cfloat r, i;
}Complex;

Complex add(const Complex a, const Complex b) {
	return (Complex) {
		a.r + b.r,
			a.i + b.i
	};
}
Complex minus(const Complex a, const Complex b) {
	return (Complex) {
		a.r - b.r,
			a.i - b.i
	};
}
Complex by(const Complex a, const Complex b) {
	return (Complex) {
		(a.r * b.r - a.i * b.i),
			(a.i * b.r + a.r * b.i)
	};
}
Complex div(const Complex a, const Complex b) {
	return (Complex) {
		(a.r * b.r + a.i * b.i) / (b.r * b.r + b.i * b.i),
			(a.i * b.r - a.r * b.i) / (b.r * b.r + b.i * b.i)
	};
}

<fun>

// 插值
uchar interplt(uchar start, uchar end, float rate) {
	return start + round((end - start) * rate);// 后面这一坨不能转换为 uchar！
}
// 颜色映射
void getColor(int iteration, uchar* r, uchar* g, uchar* b) {
	float x = (float)(iteration % 70) / 69.0f;
	if (x < 1.0f / 3) {
		x = x * 3;
		*r = interplt(4, 255, x);
		*g = interplt(41, 255, x);
		*b = interplt(111, 255, x);
	} else if (x < 2.0f / 3) {
		x = (x - 1.0f / 3) * 3;
		*r = interplt(255, 233, x);
		*g = interplt(255, 132, x);
		*b = interplt(255, 10, x);
	} else {
		x = (x - 2.0f / 3) * 3;
		*r = interplt(233, 4, x);
		*g = interplt(132, 41, x);
		*b = interplt(10, 111, x);
	}
}

// 渲染 Mandelbrot 集
__kernel void kernelM(
	__global uchar* output,
	const int zoom,
	const int idxX,
	const int idxY,
	const int maxIteration,
	const float infRange,
	const cfloat z0_r,
	const cfloat z0_i) {
	// 计算 c
	Complex c;
	c.r = pow((cfloat)2, -zoom) * (idxX + (cfloat)get_global_id(0) / chunkSize);
	c.i = pow((cfloat)2, -zoom) * (idxY + 1 - (cfloat)get_global_id(1) / chunkSize); // 反转y轴
	// 开始迭代
	Complex z = { z0_r, z0_i };
	for (int i = 0; i < maxIteration; i++) {
		z = fun(z, c);
		if (fabs(z.r) >= infRange || fabs(z.i) >= infRange) {
			uchar r, g, b;
			getColor(i, &r, &g, &b);
			output[get_global_id(0) * 3 + get_global_id(1) * chunkSize * 3] = r;
			output[get_global_id(0) * 3 + 1 + get_global_id(1) * chunkSize * 3] = g;
			output[get_global_id(0) * 3 + 2 + get_global_id(1) * chunkSize * 3] = b;
			break;
		}
	}
}
// 渲染 Julia 集
__kernel void kernelJ(
	__global uchar* output,
	const int zoom,
	const int idxX,
	const int idxY,
	const int maxIteration,
	const float infRange,
	const cfloat c_r,
	const cfloat c_i) {
	// 计算 z0
	Complex z;
	z.r = pow((cfloat)2, -zoom) * (idxX + (cfloat)get_global_id(0) / chunkSize);
	z.i = pow((cfloat)2, -zoom) * (idxY + 1 - (cfloat)get_global_id(1) / chunkSize); // 反转y轴
	// 开始迭代
	Complex c = { c_r, c_i };
	for (int i = 0; i < maxIteration; i++) {
		z = fun(z, c);
		if (fabs(z.r) >= infRange || fabs(z.i) >= infRange) {
			uchar r, g, b;
			getColor(i, &r, &g, &b);
			output[get_global_id(0) * 3 + get_global_id(1) * chunkSize * 3] = r;
			output[get_global_id(0) * 3 + 1 + get_global_id(1) * chunkSize * 3] = g;
			output[get_global_id(0) * 3 + 2 + get_global_id(1) * chunkSize * 3] = b;
			break;
		}
	}
}