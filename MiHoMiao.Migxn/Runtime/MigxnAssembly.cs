// using System.Diagnostics.CodeAnalysis;
// using System.Reflection;
// using System.Reflection.Emit;
// using Mono.Cecil;
// using Mono.Cecil.Cil;
//
// namespace MiHoMiao.Migxn.Runtime;
//
// public record MigxnAssembly(string Name): IDisposable
// {
//     [field: AllowNull, MaybeNull]
//     internal PersistedAssemblyBuilder AssemblyBuilder => field ??= new PersistedAssemblyBuilder(
//         new AssemblyName(Name), typeof(object).Assembly
//     );
//     
//     [field: AllowNull, MaybeNull]
//     internal ModuleBuilder ModuleBuilder => field ??= AssemblyBuilder.DefineDynamicModule("MainModule");
//
//     internal MemoryStream? MemoryStream { get; private set; }
//
//     public MigxnModule CreateModule(string name) => new MigxnModule(name, ModuleBuilder);
//
//     public void Save()
//     {
//         MemoryStream = new MemoryStream();
//         AssemblyBuilder.Save(MemoryStream);
//         using var fs = new FileStream(@$".\output\{Name}.dll", FileMode.Create, FileAccess.Write, FileShare.Read);
//         MemoryStream.Position = 0;
//         MemoryStream.CopyTo(fs);
//     }
//
//     public void PrintMethodInfo(MigxnModule module, string methodName)
//     {
//         MemoryStream.Position = 0;
//
//         AssemblyDefinition? asmDef = AssemblyDefinition.ReadAssembly(MemoryStream);
//         TypeDefinition? typeDef = asmDef.MainModule.GetType(module.ModuleName);
//         MethodDefinition? methodDef = typeDef.Methods.First(m => m.Name == methodName);
//
//         Console.WriteLine($"Method: {methodDef.FullName}");
//         foreach (Instruction? ins in methodDef.Body.Instructions)
//         {
//             Console.WriteLine($"{ins.Offset:X4}: {ins.OpCode} {ins.Operand}");
//         }
//     }
//
//     ~MigxnAssembly() => Dispose();
//     
//     public void Dispose()
//     {
//         MemoryStream?.Dispose();
//         MemoryStream = null;
//     }
// }