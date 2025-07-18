// using MiHoMiao.Jarfter.Core;
// using MiHoMiao.Jarfter.Core.Collection;
// using MiHoMiao.Jarfter.Func.Internal.Call;
//
// namespace MiHoMiao.xUnit.Jarfter;
//
// public class MainTest
// {
//     [Fact]
//     public void CallMethod()
//     {
//         CallHelper.Register("add'2", (int a, int b) => a + b);
//         CallHelper.Register("add'3", (int a, int b, int c) => a + b + c);
//         
//         JarfterInterpreter jarfterInterpreter = new JarfterInterpreter();
//         jarfterInterpreter.Run("call add'2 [ 7, 42 ]");
//         jarfterInterpreter.Run("internal.call add'3 [ 7, 42,51 ]");
//         Assert.Equal("100", jarfterInterpreter.CalculationStack.Pop()!.ToString());
//         Assert.Equal("49", jarfterInterpreter.CalculationStack.Pop()!.ToString());
//     }
//     
//     [Fact]
//     public void AddMethod()
//     {
//         JarfterInterpreter jarfterInterpreter = new JarfterInterpreter();
//         jarfterInterpreter.Run("add 1 3");
//         Assert.Equal("4", jarfterInterpreter.CalculationStack.Pop()!.ToString());
//         
//         jarfterInterpreter.Run("add.float 1.0 3.3");
//         Assert.Equal("4.3", jarfterInterpreter.CalculationStack.Pop()!.ToString());
//         
//         jarfterInterpreter.Run("add.string 123 123");
//         Assert.Equal("123123", jarfterInterpreter.CalculationStack.Pop()!.ToString());
//     }
//     
//     [Fact]
//     public void NestedMethod()
//     {
//         JarfterInterpreter jarfterInterpreter = new JarfterInterpreter();
//         jarfterInterpreter.Run("add.list [1, 3, @{add 1 3}]");
//         Assert.Equal("8", jarfterInterpreter.CalculationStack.Pop()!.ToString());
//     }
//     
//     [Fact]
//     public void RepeatMethod()
//     {
//         JarfterInterpreter jarfterInterpreter = new JarfterInterpreter();
//         jarfterInterpreter.Run("repeat 3 {add 4 4}");
//         Assert.Equal("8", jarfterInterpreter.CalculationStack.Pop()!.ToString());
//         Assert.Equal("8", jarfterInterpreter.CalculationStack.Pop()!.ToString());
//         Assert.Equal("8", jarfterInterpreter.CalculationStack.Pop()!.ToString());
//     }
//     
//     [Fact]
//     public void ExecuteMethod()
//     {
//         JarfterInterpreter jarfterInterpreter = new JarfterInterpreter();
//         jarfterInterpreter.Run("execute true {add 4 4} {add 1 1}");
//         jarfterInterpreter.Run("execute false {add 4 4} {add 1 1}");
//         Assert.Equal("2", jarfterInterpreter.CalculationStack.Pop()!.ToString());
//         Assert.Equal("8", jarfterInterpreter.CalculationStack.Pop()!.ToString());
//     }
//     
//     [Fact]
//     public void LetLoadMethod()
//     {
//         JarfterInterpreter jarfterInterpreter = new JarfterInterpreter();
//         jarfterInterpreter.Run("var result @{add 4 4}");
//         jarfterInterpreter.Run("var item @{load result}");
//         Assert.Equal("8", jarfterInterpreter.SymbolTable.LoadVariable("result")!.ToString());
//         Assert.Equal("8", jarfterInterpreter.SymbolTable.LoadVariable("item")!.ToString());
//     }
//     
//     [Fact]
//     public void LetLoadAddMethod()
//     {
//         JarfterInterpreter jarfterInterpreter = new JarfterInterpreter();
//         jarfterInterpreter.Run("var result @{add 4 4}");
//         jarfterInterpreter.Run("var item @{load result}");
//         jarfterInterpreter.Run("let result @{add @{load item} @{load result} }");
//         Assert.Equal("16", jarfterInterpreter.SymbolTable.LoadVariable("result")!.ToString());
//         Assert.Equal("8", jarfterInterpreter.SymbolTable.LoadVariable("item")!.ToString());
//     }
//     
//     [Fact]
//     public void BlockMethod()
//     {
//         JarfterInterpreter jarfterInterpreter = new JarfterInterpreter();
//         jarfterInterpreter.Run("var result 0");
//         jarfterInterpreter.Run("var item 0");
//         jarfterInterpreter.Run("block [\n{ let result @{ add 4 4 } },\n{ let item @{ load result } },\n]");
//         jarfterInterpreter.Run("let result @{add @{load item} @{load result} }");
//         Assert.Equal("16", jarfterInterpreter.SymbolTable.LoadVariable("result")!.ToString());
//         Assert.Equal("8", jarfterInterpreter.SymbolTable.LoadVariable("item")!.ToString());
//     }
//     
//     [Fact]
//     public void ArrayTest()
//     {
//         JarfterArray<int> array = JarfterArray<int>.Parse("[1,2,3]", null);
//         Assert.Equal(1, array.Items[0]);
//         Assert.Equal(2, array.Items[1]);
//         Assert.Equal(3, array.Items[2]);
//     }
//     
//     [Fact]
//     public void JumpTest()
//     {
//         JarfterInterpreter jarfterInterpreter = new JarfterInterpreter();
//         jarfterInterpreter.SymbolTable.DeclareVariable("result", 0);
//         jarfterInterpreter.Run(
//             """
//             block [
//             { label here },
//             { let result @{ add @result 1 } },
//             { cmp @result 4 },
//             { jl here },
//             ]
//             """
//         );
//         Assert.Equal("4", jarfterInterpreter.SymbolTable.LoadVariable("result")!.ToString());
//     }
//     
//     [Fact]
//     public void CompareTest()
//     {
//         JarfterInterpreter jarfterInterpreter = new JarfterInterpreter();
//         jarfterInterpreter.SymbolTable.DeclareVariable("result", 0);
//         jarfterInterpreter.Run(
//             """
//             block [
//             { label here },
//             { let result @{ add @result 1 } },
//             { jt @{less @result 4  }  here },
//             ]
//             """
//         );
//         Assert.Equal("4", jarfterInterpreter.SymbolTable.LoadVariable("result")!.ToString());
//     }
//     
//     [Fact]
//     public void MultilineTest()
//     {
//         JarfterInterpreter jarfterInterpreter = new JarfterInterpreter();
//         jarfterInterpreter.SymbolTable.DeclareVariable("result", 0);
//         jarfterInterpreter.Run(
//             [
//                 "label label_here",
//                 "let result @{ add @result 1 }",
//                 "jt @{less @result 4  }  label_here",
//             ]
//         );
//         Assert.Equal("4", jarfterInterpreter.SymbolTable.LoadVariable("result")!.ToString());
//     }
//     
// }