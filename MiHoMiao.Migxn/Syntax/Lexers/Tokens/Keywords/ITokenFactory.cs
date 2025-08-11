namespace MiHoMiao.Migxn.Syntax.Lexers.Tokens.Keywords;

internal interface ITokenFactory<out T> where T : MigxnToken
{
    
    public static abstract string UniqueName { get; }
    
    public static abstract T Create(int index, (int Line, int Column) position);

}

internal interface IKeywordToken : ITokenFactory<AbstractKeyword>
{

}