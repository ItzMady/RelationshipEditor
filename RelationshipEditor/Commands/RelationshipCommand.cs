using System;
using RelationshipEditor.Services;
using StardewModdingAPI;

namespace RelationshipEditor.Commands;

internal class RelationshipCommand
{
    private const int MaxHearts = 14;

    private readonly IMonitor monitor;
    private readonly RelationshipService relationship;

    public RelationshipCommand(IMonitor monitor)
    {
        this.monitor = monitor;
        relationship = new(monitor);
    }

    public void Execute(string command, string[] args)
    {
        if (!Context.IsWorldReady)
        {
            monitor.Log("Load a save before using this command.", LogLevel.Warn);
            return;
        }

        if (args.Length == 0)
        {
            monitor.Log("Use 'social help' to see available commands.", LogLevel.Info);
            return;
        }

        switch (args[0].ToLowerInvariant())
        {
            case "help":
                ShowHelp();
                return;

            case "list":
                relationship.ListNPCs();
                return;

            case "all":
                SetAll(args);
                return;

            default:
                SetNpc(args);
                return;
        }
    }

    private void ShowHelp()
    {
        monitor.Log("Available commands:", LogLevel.Info);
        monitor.Log("social <NPC> <0-14>   - Set an NPC's friendship.", LogLevel.Info);
        monitor.Log("social all <0-14>     - Set every NPC's friendship.", LogLevel.Info);
        monitor.Log("social list           - List all NPCs.", LogLevel.Info);
        monitor.Log("social help           - Show this help message.", LogLevel.Info);
    }

    private void SetAll(string[] args)
    {
        if (args.Length != 2)
        {
            monitor.Log("Usage: social all <hearts>", LogLevel.Warn);
            return;
        }

        if (!TryParseHearts(args[1], out int hearts))
            return;

        relationship.SetAllHearts(hearts);
    }

    private void SetNpc(string[] args)
    {
        if (args.Length < 2)
        {
            monitor.Log("Usage: social <NPC> <hearts>", LogLevel.Warn);
            return;
        }

        if (!TryParseHearts(args[^1], out int hearts))
            return;

        string npcName = string.Join(" ", args[..^1]);

        relationship.SetHearts(npcName, hearts);
    }

    private bool TryParseHearts(string value, out int hearts)
    {
        if (!int.TryParse(value, out hearts))
        {
            monitor.Log("Hearts must be a whole number.", LogLevel.Warn);
            return false;
        }

        if (hearts < 0 || hearts > MaxHearts)
        {
            monitor.Log($"Hearts must be between 0 and {MaxHearts}.", LogLevel.Warn);
            return false;
        }

        return true;
    }
}
