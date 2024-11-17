// STD C++
#include <stack>
using namespace std;

// C++/CLR
#include <msclr/marshal_cppstd.h>
using namespace System;

namespace CppCLI {
	static public ref class CodeSolver {
		static String^ validChar = "0123456789zcrie()+-*/^";
	public:
		// 翻译为后缀表达式
		static String^ Translate(String^ code) {
			// 去除空格
			// 把 ** 翻译成 ^
			code = code->Trim()->Replace("**", "^");
			// 检查是否有非法字符
			for (int i = 0; i < code->Length; i++) {
				if (validChar->Contains(code[i])) continue;
				return "";
			}
			// 开始翻译
			stack<char> stackNum, stackSbl;
			for (int i = 0; i < code->Length; i++) {
				if (code[i] >= 0 && code[i] <= 9) {

				} else {
					
				}
			}
		}
	};
}