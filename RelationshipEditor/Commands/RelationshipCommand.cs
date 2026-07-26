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

        // Check arguments
        if (args.Length != 2)
        {
            this.monitor.Log("Usage: relationship <NPC> <hearts>", LogLevel.Warn);
            return;
        }

        string npcName = args[0];

        // Check if the heart value is valid
        if (!int.TryParse(args[1], out int hearts))
        {
            this.monitor.Log("Hearts must be a whole number.", LogLevel.Warn);
            return;
        }

        if (hearts < 0 || hearts > 14)
        {
            this.monitor.Log("Hearts must be between 0 and 14.", LogLevel.Warn);
            return;
        }

        // Update the relationship
        this.relationshipService.SetHearts(npcName, hearts);
    }
}