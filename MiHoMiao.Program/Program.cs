using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

// ConsoleProgram.Run();
unsafe
{
    string input = "1111";
    ReadOnlySpan<byte> readOnlySpan = MemoryMarshal.AsBytes(input.AsSpan());
    ref byte reference = ref MemoryMarshal.GetReference(readOnlySpan);
    void* pointer = Unsafe.AsPointer(ref Unsafe.AddByteOffset(ref reference, new IntPtr(-4)));
    *(int*)pointer = 1;
    Console.Write(input);
}

