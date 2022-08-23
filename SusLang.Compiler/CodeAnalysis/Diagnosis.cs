using System.Text.Json;

namespace SusLang.CodeAnalysis;

public class Diagnosis
{
    public Diagnosis(string message, InspectionSeverity severity, int lineNumber, int columnNumber = -1)
    {
        Message = message;
        Severity = severity;
        LineNumber = lineNumber;
        ColumnNumber = columnNumber;
    }
    
    public int LineNumber { get; }
    public int ColumnNumber { get; }
    
    public string Message { get; }
    public InspectionSeverity Severity { get; }
    
    public string GetJson()
    {
        return JsonSerializer.Serialize(this);
    }
}