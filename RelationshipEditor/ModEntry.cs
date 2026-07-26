using RelationshipEditor.Commands;
using StardewModdingAPI;

namespace RelationshipEditor;

public class ModEntry : Mod
{
    public override void Entry(IModHelper helper)
    {
        Monitor.Log("Relationship Editor loaded successfully.", LogLevel.Info);

        RegisterCommands(helper);
    }

    private void RegisterCommands(IModHelper helper)
    {
        RelationshipCommand relationshipCommand = new(Monitor);

        helper.ConsoleCommands.Add(
            "social",
            "Usage: relationship <NPC> <hearts>",
            relationshipCommand.Execute
        );
    }
}
