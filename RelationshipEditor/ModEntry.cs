using RelationshipEditor.Commands;
using StardewModdingAPI;

namespace RelationshipEditor;

public class ModEntry : Mod
{
    public override void Entry(IModHelper helper)
    {
        helper.ConsoleCommands.Add(
            "social",
            "Edit NPC friendship.",
            new RelationshipCommand(Monitor).Execute);

        Monitor.Log("Relationship Editor loaded.", LogLevel.Info);
    }
}
