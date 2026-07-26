using System;
using RelationshipEditor.Services;
using StardewModdingAPI;

namespace RelationshipEditor.Commands;

internal class RelationshipCommand
{
    private readonly IMonitor monitor;
    private readonly RelationshipService relationshipService;

    public RelationshipCommand(IMonitor monitor)
    {
        this.monitor = monitor;
        this.relationshipService = new RelationshipService(monitor);
    }

    public void Execute(string command, string[] args)
    {
        // Check if a save is loaded
        if (!Context.IsWorldReady)
        {
            this.monitor.Log("You must load a save before using this command.", LogLevel.Warn);
            return;
        }

        // hearts help
        if (args.Length == 1 && args[0].Equals("help", StringComparison.OrdinalIgnoreCase))
        {
            this.monitor.Log("Available commands:", LogLevel.Info);
            this.monitor.Log("relationship <NPC> <0-14>   - Set an NPC's friendship.", LogLevel.Info);
            this.monitor.Log("relationship all <0-14>     - Set every NPC's friendship.", LogLevel.Info);
            this.monitor.Log("relationship list           - List all available NPCs.", LogLevel.Info);
            this.monitor.Log("relationship help           - Show this help message.", LogLevel.Info);
            return;
        }

        // hearts list
        if (args.Length == 1 && args[0].Equals("list", StringComparison.OrdinalIgnoreCase))
        {
            this.relationshipService.ListNPCs();
            return;
        }

        // hearts all <hearts>
        if (args.Length == 2 && args[0].Equals("all", StringComparison.OrdinalIgnoreCase))
        {
            if (!int.TryParse(args[1], out int allHearts))
            {
                this.monitor.Log("Hearts must be a whole number.", LogLevel.Warn);
                return;
            }

            if (allHearts < 0 || allHearts > 14)
            {
                this.monitor.Log("Hearts must be between 0 and 14.", LogLevel.Warn);
                return;
            }

            this.relationshipService.SetAllHearts(allHearts);
            return;
        }

        // hearts <NPC> <hearts>
        if (args.Length < 2)
        {
            this.monitor.Log("Usage: hearts <NPC> <hearts>", LogLevel.Warn);
            return;
        }

        if (!int.TryParse(args[^1], out int hearts))
        {
            this.monitor.Log("Hearts must be a whole number.", LogLevel.Warn);
            return;
        }

        if (hearts < 0 || hearts > 14)
        {
            this.monitor.Log("Hearts must be between 0 and 14.", LogLevel.Warn);
            return;
        }

        string npcName = string.Join(" ", args[..^1]);

        this.relationshipService.SetHearts(npcName, hearts);
    }
}
