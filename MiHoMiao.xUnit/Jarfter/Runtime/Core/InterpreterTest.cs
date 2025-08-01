using MiHoMiao.Jarfter.Exception;
using MiHoMiao.Jarfter.Runtime.Collection;
using MiHoMiao.Jarfter.Runtime.Core;

namespace MiHoMiao.xUnit.Jarfter.Runtime.Core;

public class InterpreterTest
{
    [Fact]
    public void VarMethod()
    {
        JarfterInterpreter jarfterInterpreter = new JarfterInterpreter();
        jarfterInterpreter.Run("var item 500");
        Assert.Equal("500", jarfterInterpreter.JarfterContext.JarfterSymbolTable.LoadVariable("item")!.ToString());
        Assert.Throws<VariableMulDeclaredException>(() => jarfterInterpreter.Run("var item 500"));
    }
    
    [Fact]
    public void LongNameMethod()
    {
        JarfterInterpreter jarfterInterpreter = new JarfterInterpreter();
        jarfterInterpreter.Run("main.var item.my 500");
        Assert.Equal("500", jarfterInterpreter.JarfterContext.JarfterSymbolTable.LoadVariable("item.my")!.ToString());
        Assert.Throws<VariableMulDeclaredException>(() => jarfterInterpreter.Run("main.var item.my 500"));
    }
    
    [Fact]
    public void LoadMethod()
    {
        JarfterInterpreter jarfterInterpreter = new JarfterInterpreter();
        jarfterInterpreter.Run("var item 500");
        jarfterInterpreter.Run("load item");
        Assert.Equal("500", jarfterInterpreter.JarfterContext.CalculationStack.Pop()!.ToString());
        Assert.Throws<VariableNotDeclaredException>(() => jarfterInterpreter.Run("load other"));
    }
    
    [Fact]
    public void AtGrammar()
    {
        JarfterInterpreter jarfterInterpreter = new JarfterInterpreter();
        jarfterInterpreter.Run("var item 500");
        jarfterInterpreter.Run("var copy @item");
        Assert.Equal("500", jarfterInterpreter.JarfterContext.JarfterSymbolTable.LoadVariable("copy")!.ToString());
    }
    
    [Fact]
    public void LetMethod()
    {
        JarfterInterpreter jarfterInterpreter = new JarfterInterpreter();
        jarfterInterpreter.Run("var item 500");
        Assert.Equal("500", jarfterInterpreter.JarfterContext.JarfterSymbolTable.LoadVariable("item")!.ToString());
        jarfterInterpreter.Run("let item 100");
        Assert.Equal("100", jarfterInterpreter.JarfterContext.JarfterSymbolTable.LoadVariable("item")!.ToString());
        Assert.Throws<VariableNotDeclaredException>(() => jarfterInterpreter.Run("let copy 100"));
    }
    
    [Fact]
    public void BlockMethod()
    {
        JarfterInterpreter jarfterInterpreter = new JarfterInterpreter();
        jarfterInterpreter.Run("var item 500");
        jarfterInterpreter.Run(
            """
            block[ { var item 100 },]
            """
        );
        Assert.Equal("500", jarfterInterpreter.JarfterContext.JarfterSymbolTable.LoadVariable("item")!.ToString());
        
        jarfterInterpreter.Run(
            """
            block[ { let item 100 },]
            """
        );
        Assert.Equal("100", jarfterInterpreter.JarfterContext.JarfterSymbolTable.LoadVariable("item")!.ToString());
        
        jarfterInterpreter.Run(
            """
            block[ { var other 100 },]
            """
        );
        Assert.Throws<VariableNotDeclaredException>(() => jarfterInterpreter.Run("let other 100"));
    }
    
    [Fact]
    public void ExecuteMethod()
    {
        JarfterInterpreter jarfterInterpreter = new JarfterInterpreter();
        jarfterInterpreter.Run("execute true { var item 500 }");
        Assert.Equal("500", jarfterInterpreter.JarfterContext.JarfterSymbolTable.LoadVariable("item")!.ToString());
        
        jarfterInterpreter.Run("execute false { let item 100 }   ");
        Assert.Equal("500", jarfterInterpreter.JarfterContext.JarfterSymbolTable.LoadVariable("item")!.ToString());
        
        jarfterInterpreter.Run("execute false { let item 100 } else { let item 400 }");
        Assert.Equal("400", jarfterInterpreter.JarfterContext.JarfterSymbolTable.LoadVariable("item")!.ToString());
        
        jarfterInterpreter.Run("var boolean true ");
        jarfterInterpreter.Run("execute @boolean { let item 300 } else { let item 600 }");
        Assert.Equal("300", jarfterInterpreter.JarfterContext.JarfterSymbolTable.LoadVariable("item")!.ToString());
        
        jarfterInterpreter.Run("execute true { block[ { let boolean 100 },] }");
        Assert.Equal("100", jarfterInterpreter.JarfterContext.JarfterSymbolTable.LoadVariable("boolean")!.ToString());
        
    }
    
    [Fact]
    public void MathMethod()
    {
        JarfterInterpreter jarfterInterpreter = new JarfterInterpreter();
        JarfterSymbolTable<JarfterObject> symbols = jarfterInterpreter.JarfterContext.JarfterSymbolTable;
        jarfterInterpreter.Run("var item 0");
        
        jarfterInterpreter.Run("let item @{ add @item 1 }");
        Assert.Equal("1", symbols.LoadVariable("item")!.ToString());
        
        jarfterInterpreter.Run("let item @{ sub @item -1 }");
        Assert.Equal("2", symbols.LoadVariable("item")!.ToString());
        
        jarfterInterpreter.Run("let item @{ mul @item 3 }");
        Assert.Equal("6", symbols.LoadVariable("item")!.ToString());
        
        jarfterInterpreter.Run("let item @{ div @item 2 }");
        Assert.Equal("3", symbols.LoadVariable("item")!.ToString());
        
        jarfterInterpreter.Run("let item @{ mod @item 2 }");
        Assert.Equal("1", symbols.LoadVariable("item")!.ToString());
        
    }
    
    [Fact]
    public void LoopMethod()
    {
        JarfterInterpreter jarfterInterpreter = new JarfterInterpreter();
        JarfterSymbolTable<JarfterObject> symbols = jarfterInterpreter.JarfterContext.JarfterSymbolTable;
        
        jarfterInterpreter.Run("var item 0");
        Assert.Equal("0", symbols.LoadVariable("item")!.ToString());
        
        jarfterInterpreter.Run("loop 5 { let item @{ add @item 1 } }   ");
        Assert.Equal("5", symbols.LoadVariable("item")!.ToString());
        
    }
    
    [Fact]
    public void RepeatMethod()
    {
        JarfterInterpreter jarfterInterpreter = new JarfterInterpreter();
        JarfterSymbolTable<JarfterObject> symbols = jarfterInterpreter.JarfterContext.JarfterSymbolTable;
        
        jarfterInterpreter.Run("var item 0");
        Assert.Equal("0", symbols.LoadVariable("item")!.ToString());
        
        jarfterInterpreter.Run("repeat 5 [ { let item @{ add @item @i } } ]   ");
        Assert.Equal("10", symbols.LoadVariable("item")!.ToString());
        
    }
    
    [Fact]
    public void GotoMethod()
    {
        JarfterInterpreter jarfterInterpreter = new JarfterInterpreter();
        JarfterSymbolTable<JarfterObject> symbols = jarfterInterpreter.JarfterContext.JarfterSymbolTable;
        
        jarfterInterpreter.Run("var n 3");
        jarfterInterpreter.Run(
            """
            block [
            { var i 1 },
            { var result 1 },
            { label loop.start },
            { execute @{ lese @i @n } { 
                block [
                    { let result @{ mul @result @i } },
                    { let i @{ add @i 1 } },
                    { goto loop.start },
                ]
            } },
            { let n @result },
            ]
            """
        );

        Assert.Equal("6", symbols.LoadVariable("n")!.ToString());
        
    }
    
    /// <summary>
    ///一个复杂的案例.
    /// 参考字符数: 290
    /// </summary>
    [Fact]
    public void JumpMethod()
    {
        JarfterInterpreter jarfterInterpreter = new JarfterInterpreter();
        JarfterSymbolTable<JarfterObject> symbols = jarfterInterpreter.JarfterContext.JarfterSymbolTable;
        
        jarfterInterpreter.Run("var n 3");
        jarfterInterpreter.Run(
            """
            block [
                { var i 1 },
                { var result 1 },
                { label loop.start },
                { execute @{ lese @i @n } {
                    block [
                        { let result @{ mul @result @i } },
                        { let i @{ add @i 1 } },
                        { execute @{ eql @{ mod @result 2 } 0 } { goto loop.start } 
                          else { execute @{ lese @i @n } {
                            block [
                                { let result @{ mul @result @i } },
                                { let i @{ add @i 1 } },
                                { goto loop.start }
                            ]
                        } }},
                    ]
                } },
                { let n @result }
            ]
            """
        );

        Assert.Equal("6", symbols.LoadVariable("n")!.ToString());
        
    }
    
    /// <summary>
    /// 现在, 使用 $[ ] 来替代 block [ ] 多语句函数.
    /// 参考字符数: 278
    /// </summary>
    [Fact]
    public void SimpleBlockGrammar()
    {
        JarfterInterpreter jarfterInterpreter = new JarfterInterpreter();
        JarfterSymbolTable<JarfterObject> symbols = jarfterInterpreter.JarfterContext.JarfterSymbolTable;
        
        jarfterInterpreter.Run("var n 3");
        jarfterInterpreter.Run(
            """
            $[
                { var i 1 },
                { var result 1 },
                { label loop.start },
                { execute @{ lese @i @n } {
                    $[
                        { let result @{ mul @result @i } },
                        { let i @{ add @i 1 } },
                        { execute @{ eql @{ mod @result 2 } 0 } { goto loop.start } 
                          else { execute @{ lese @i @n } {
                            $[
                                { let result @{ mul @result @i } },
                                { let i @{ add @i 1 } },
                                { goto loop.start }
                            ]
                        } }},
                    ]
                } },
                { let n @result }
            ]
            """
        );
    
        Assert.Equal("6", symbols.LoadVariable("n")!.ToString());
        
    }
    
    /// <summary>
    /// 现在, 使用 ( ) 来替代 @{ } 计算表达式.
    /// 参考字符数: 270
    /// </summary>
    [Fact]
    public void SimpleAtGrammar()
    {
        JarfterInterpreter jarfterInterpreter = new JarfterInterpreter();
        JarfterSymbolTable<JarfterObject> symbols = jarfterInterpreter.JarfterContext.JarfterSymbolTable;
        
        jarfterInterpreter.Run("var n 3");
        jarfterInterpreter.Run(
            """
            $[
                { var i 1 },
                { var result 1 },
                { label loop.start },
                { execute (lese @i @n) 
                    { $[
                        { let result (mul @result @i) },
                        { let i (add @i 1) },
                        { execute (eql (mod @result 2) 0 ) { goto loop.start } 
                          else { execute (lese @i @n) {
                            $[
                                { let result (mul @result @i) },
                                { let i (add @i 1) },
                                { goto loop.start }
                            ]
                        } }},
                    ] }
                },
                { let n @result }
            ]
            """
        );
    
        Assert.Equal("6", symbols.LoadVariable("n")!.ToString());
        
    }
    
    /// <summary>
    /// 现在, 多语句函数中的子语句使用逗号分隔, 且不再需要 { } 包围.
    /// 参考字符数: 249
    /// </summary>
    [Fact]
    public void SimpleMultilineGrammar()
    {
        JarfterInterpreter jarfterInterpreter = new JarfterInterpreter();
        JarfterSymbolTable<JarfterObject> symbols = jarfterInterpreter.JarfterContext.JarfterSymbolTable;
        
        jarfterInterpreter.Run("var n 3");
        jarfterInterpreter.Run(
            """
            $[
                var i 1 ,
                var result 1 ,
                label loop.start,
                execute (lese @i @n) { 
                    $[
                        let result (mul @result @i),
                        let i (add @i 1),
                        execute (eql (mod @result 2) 0 ) { goto loop.start } 
                        else{ execute (lese @i @n) {
                            $[
                                let result (mul @result @i),
                                let i (add @i 1),
                                goto loop.start
                             ] },
                         },
                    ] },
                let n @result,
            ]
            """
        );
    
        Assert.Equal("6", symbols.LoadVariable("n")!.ToString());
        
    }
    
    /// <summary>
    /// 现在, 子语句为 { block } 时, 可以直接创建 ${ } 使用.
    /// 参考字符数: 242
    /// </summary>
    [Fact]
    public void MoreSimpleBlockGrammar()
    {
        JarfterInterpreter jarfterInterpreter = new JarfterInterpreter();
        JarfterSymbolTable<JarfterObject> symbols = jarfterInterpreter.JarfterContext.JarfterSymbolTable;
        
        jarfterInterpreter.Run("var n 3");
        jarfterInterpreter.Run(
            """
            $[
                var i 1 ,
                var result 1 ,
                label loop.start,
                execute (lese @i @n) ${
                    let result (mul @result @i),
                    let i (add @i 1),
                    execute (eql (mod @result 2) 0 ) { goto loop.start } 
                    else { execute (lese @i @n) ${
                        let result (mul @result @i),
                        let i (add @i 1),
                        goto loop.start,
                    }},
                },
                let n @result,
            ]
            """
        );
    
        Assert.Equal("6", symbols.LoadVariable("n")!.ToString());
        
    }
    
    /// <summary>
    /// 现在 { } 样式的子语句, 可以替换为 -> 样式.
    /// 但是 execute 的首条语句需要用 ; 符号做分割.
    /// 参考字符数: 242
    /// </summary>
    [Fact]
    public void LambdaLikeGrammar()
    {
        JarfterInterpreter jarfterInterpreter = new JarfterInterpreter();
        JarfterSymbolTable<JarfterObject> symbols = jarfterInterpreter.JarfterContext.JarfterSymbolTable;
        
        jarfterInterpreter.Run("var n 3");
        jarfterInterpreter.Run(
            """
            ${
                var i 1 ,
                var result 1 ,
                label loop.start,
                execute (lese @i @n) ${ 
                    let result (mul @result @i),
                    let i (add @i 1),
                    execute (eql (mod @result 2) 0 ) -> goto loop.start;
                    else -> execute (lese @i @n) ${
                        let result (mul @result @i),
                        let i (add @i 1),
                        goto loop.start
                    },
                },
                let n @result,
            }
            """
        );
    
        Assert.Equal("6", symbols.LoadVariable("n")!.ToString());
        
    }
    
    [Fact]
    public void MathExMethod()
    {
        JarfterInterpreter jarfterInterpreter = new JarfterInterpreter();
        JarfterSymbolTable<JarfterObject> symbols = jarfterInterpreter.JarfterContext.JarfterSymbolTable;
        jarfterInterpreter.Run("var item 0");
        
        jarfterInterpreter.Run("let item (max [1, 2, 3, 4, 5])");
        Assert.Equal("5", symbols.LoadVariable("item")!.ToString());
        
        jarfterInterpreter.Run("let item (min [1, 2, 3, 4, 5])");
        Assert.Equal("1", symbols.LoadVariable("item")!.ToString());
        
        jarfterInterpreter.Run("let item (avg [1, 2, 3, 4, 5])");
        Assert.Equal("3", symbols.LoadVariable("item")!.ToString());
        
        jarfterInterpreter.Run("let item (sum [1, 2, 3, 4, 5])");
        Assert.Equal("15", symbols.LoadVariable("item")!.ToString());
        
        jarfterInterpreter.Run("let item (round 1.45)");
        Assert.Equal("1", symbols.LoadVariable("item")!.ToString());
        
    }
    
    
}