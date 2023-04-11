using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using SusLang;
using SusLang.Expressions;
using ExecutionContext = SusLang.ExecutionContext;

string apiKey = File.ReadAllText("apiKey.txt");

Dictionary<ulong, ExecutionContext> channels = new();

var client = new DiscordClient(new DiscordConfiguration()
{
    Token = apiKey,
    TokenType = TokenType.Bot,
    Intents = DiscordIntents.AllUnprivileged | DiscordIntents.MessageContents
});
client.MessageCreated += MessageReceived;

Compiler.Logging.ThrowRuntimeError = true;


await client.ConnectAsync();

await Task.Delay(-1);


Task Log(DiscordClient msg, MessageCreateEventArgs args)
{
    Console.WriteLine(msg.ToString());
    return Task.CompletedTask;
}

bool ExecuteExpressionWithTimeout(Expression exp, float seconds = 5) {
    Action<object> longMethod = (monitorSync) =>
    {
        exp.Execute();
        lock (monitorSync) {
            Monitor.Pulse(monitorSync);
        }
    };
    object monitorSync = new();
    bool timedOut;
    lock (monitorSync) {
        longMethod.BeginInvoke(monitorSync, null, null);
        timedOut = !Monitor.Wait(monitorSync, TimeSpan.FromSeconds(seconds)); // waiting 30 secs
        Console.WriteLine("Monitorus lool");
    }

    if (timedOut)
    {
        Console.WriteLine("Fuck you");
    }

    return timedOut;
}

async Task MessageReceived(DiscordClient _, MessageCreateEventArgs args)
{
    Console.WriteLine($"Received message {args.Message.Content}");
    ulong channelId = args.Channel.Id;
    
    if (args.Message.Content == "clear sussily" && channels.ContainsKey(channelId))
    {
        channels.Remove(channelId);
        await args.Message.CreateReactionAsync(DiscordEmoji.FromName(client, ":white_check_mark:"));
        return;
    }

    // Ensure that there is an execution context for the channel of the message
    if (!channels.ContainsKey(channelId))
    {
        channels[channelId] = new(
            Array.Empty<Expression>(),
            new(Compiler.StandardCrewmates)
        );
    }

    string content = args.Message.Content;

    content = content.Trim('`');

    async void OnOutput(string output)
    {
        await client.SendMessageAsync(args.Channel, output);
    }

    try
    {
        ExecutionContext ast = Compiler.CreateAst(content);

        Compiler.Logging.OnOutput += OnOutput;

        Expression[] expressions = ast.Expressions.ToArray();
        
        foreach (Expression expression in expressions)
        {
            expression.SetContextRecursively(channels[channelId]);
            if (ExecuteExpressionWithTimeout(expression))
            {
                // If the method timed out
                await client.SendMessageAsync(args.Channel, "That timed out. Fuck you");
            }
        }
        Compiler.Logging.OnOutput -= OnOutput;

        await args.Message.CreateReactionAsync(DiscordEmoji.FromName(client, ":white_check_mark:"));
    }
    catch
    {
        // ignore loool
    }

}