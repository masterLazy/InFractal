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
		// ����Ϊ��׺���ʽ
		static String^ Translate(String^ code) {
			// ȥ���ո�
			// �� ** ����� ^
			code = code->Trim()->Replace("**", "^");
			// ����Ƿ��зǷ��ַ�
			for (int i = 0; i < code->Length; i++) {
				if (validChar->Contains(code[i])) continue;
				return "";
			}
			// ��ʼ����
			stack<char> stackNum, stackSbl;
			for (int i = 0; i < code->Length; i++) {
				if (code[i] >= 0 && code[i] <= 9) {

				} else {
					
				}
			}
		}
	};
}