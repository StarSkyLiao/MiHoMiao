using System.Runtime.InteropServices;

namespace MiHoMiao.Program.Core;

public static class UnsafeString
{
    public static void Run()
    {
        // 创建一个标准 string 对象
        string s = "a";
        Console.WriteLine($"Standard string: '{s}', Length: {s.Length}");


// 打印标准 string 的内存布局（仅用于参考，需 unsafe 访问）
        unsafe
        {
            fixed (char* charPtr = s)
            {
                byte* bytePtr = (byte*)charPtr - 20; // 假设对象头从 [-20] 开始
                Console.WriteLine("\nStandard string memory layout (approximate):");
                for (int i = 0; i < 24; i++)
                {
                    Console.Write($"{bytePtr[i]:X2} ");
                    if ((i + 1) % 4 == 0) Console.WriteLine();
                }
            }
        }

// 尝试手动构造 string 对象
        IntPtr ptr = Marshal.AllocHGlobal(1024);
        try
        {
            unsafe
            {
                byte* rawPtr = (byte*)ptr.ToPointer();
                byte* basePtr = rawPtr;

                // 构造内存布局
                *(int*)rawPtr = 0; // [0, 4) 填充 0
                rawPtr += sizeof(int);

                *(int*)rawPtr = 0; // [4, 8) 填充 0
                rawPtr += sizeof(int);

                *(long*)rawPtr = typeof(string).TypeHandle.Value.ToInt64(); // [8, 16) 类型地址
                rawPtr += sizeof(long);

                *(int*)rawPtr = 1; // [16, 20) 长度 1
                rawPtr += sizeof(int);

                *(char*)rawPtr = 'a'; // [20, 22) 字符 'a'

                // 打印内存布局用于调试
                Console.WriteLine("Constructed memory layout:");
                for (int i = 0; i < 24; i++)
                {
                    Console.Write($"{basePtr[i]:X2} ");
                    if ((i + 1) % 4 == 0) Console.WriteLine();
                }

                // 尝试读取 string
                try
                {
                    // 调整读取位置，使其指向字符 'a'，模拟 string 对象的基址
                    string item = Marshal.PtrToStringUni(new IntPtr(basePtr + 20));
                    Console.WriteLine($"Constructed string: '{item}', Length: {item.Length}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Failed to read string: {ex.Message}");
                }
        
            }
        }
        finally
        {
            Marshal.FreeHGlobal(ptr); // 释放内存
        }
    }

}