using System.Text.Json;
using System.Text.Json.Serialization;

namespace SusLang.CodeAnalysis;

public readonly struct Diagnosis
{
    public Diagnosis(ExecutionContext context, string message, InspectionSeverity severity, int lineNumber, int columnNumber = -1)
    {
        Message = message;
        Severity = severity;
        LineNumber = lineNumber;
        Context = context;
        ColumnNumber = columnNumber;

        context.Diagnoses.Add(this);
    }

    [JsonIgnore]
    public ExecutionContext Context { get; }
    public int LineNumber { get; }
    public int ColumnNumber { get; }

    public string Message { get; }
    public InspectionSeverity Severity { get; }

    public string GetJson()
    {
        return JsonSerializer.Serialize(this);
    }
}