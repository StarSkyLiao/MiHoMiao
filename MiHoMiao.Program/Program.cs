
using System.Globalization;

string[] inputs = { 
    "123", 
    "9223372036854775807", // long.MaxValue
    "-9223372036854775808", // long.MinValue
    "0b111",
    "123,456",
    "0b111",
    "9223372036854775808", // 溢出
    "abc" // 无效格式
};

foreach (var input in inputs)
{
    if (long.TryParse(input, NumberStyles.AllowThousands
            , null, out long result))
    {
        Console.WriteLine($"解析成功: {input} -> {result}");
    }
    else
    {
        Console.WriteLine($"解析失败: {input}");
    }
}
