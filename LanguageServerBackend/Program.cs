using System.Text.Json;
using ExecutionContext = SusLang.ExecutionContext;

string arg = args[0];

if (File.Exists(arg))
{
    ExecutionContext context = SusLang.Compiler.CreateAst(File.ReadAllText(arg), true);

    string diagnoses = JsonSerializer.Serialize(context.Diagnoses);

    Console.WriteLine(diagnoses);
}