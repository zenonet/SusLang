using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using SusLang;
using ExecutionContext = SusLang.ExecutionContext;
using Expression = SusLang.Expressions.Expression;

string apiKey = File.ReadAllText("apiKey.txt");

Dictionary<ulong, ExecutionContext> channels = new();

var client = new DiscordClient(new()
{
    Token = apiKey,
    TokenType = TokenType.Bot,
    Intents = DiscordIntents.AllUnprivileged | DiscordIntents.MessageContents
});
client.MessageCreated += MessageReceived;

Compiler.Logging.ThrowRuntimeError = true;
Compiler.LoopTimeout = 15;


await client.ConnectAsync();

await Task.Delay(-1);


Task Log(DiscordClient msg, MessageCreateEventArgs args)
{
    Console.WriteLine(msg.ToString());
    return Task.CompletedTask;
}

async Task MessageReceived(DiscordClient _, MessageCreateEventArgs args)
{


    if (args.Author.IsCurrent || args.Channel.Name != "suslang")
        return;
    
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

        Compiler.Logging.OnOutput += OnOutput;

        ExecutionContext ast = Compiler.CreateAst(content);

        channels[channelId].Expressions = new List<Expression>();
        channels[channelId].ExecuteInThisContext(ast);
        /*
        Expression[] expressions = ast.Expressions.ToArray();

        foreach (Expression expression in expressions)
        {
            expression.SetContextRecursively(channels[channelId]);
            if (ExecuteExpressionWithTimeout(expression))
            {
                // If the method timed out
                await client.SendMessageAsync(args.Channel, "That timed out. Fuck you");
            }
        }*/

        await args.Message.CreateReactionAsync(DiscordEmoji.FromName(client, ":white_check_mark:"));
    }
    finally
    {
        Compiler.Logging.OnOutput -= OnOutput;
    }

}