using StardewModdingAPI;

namespace RelationshipEditor;

public class ModEntry : Mod
{
    public override void Entry(IModHelper helper)
    {
        Monitor.Log("Relationship Editor loaded successfully.", LogLevel.Info);
    }
}