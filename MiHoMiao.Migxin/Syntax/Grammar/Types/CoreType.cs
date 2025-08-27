namespace MiHoMiao.Migxin.Syntax.Grammar.Types;

internal class CoreType
{
    public abstract class Unmanaged
    {
        public abstract Type CoreType { get; }
    }

    public class Float64 : Unmanaged
    {
        public override Type CoreType => typeof(double);
    }

    public class Float32 : Float64
    {
        public override Type CoreType => typeof(float);
    }

    public class Int64 : Float64
    {
        public override Type CoreType => typeof(long);
    }

    public class Int32 : Int64
    {
        public override Type CoreType => typeof(int);
    }

    public class Char : Int64
    {
        public override Type CoreType => typeof(char);
    }
    
}