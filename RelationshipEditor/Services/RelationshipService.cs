using StardewModdingAPI;
using StardewValley;

namespace RelationshipEditor.Services;

internal class RelationshipService
{
    private readonly IMonitor monitor;

    public RelationshipService(IMonitor monitor)
    {
        this.monitor = monitor;
    }

    public bool SetHearts(string npcName, int hearts)
    {
        NPC? npc = Game1.getCharacterFromName(npcName);

        if (npc is null)
        {
            this.monitor.Log($"NPC '{npcName}' was not found.", LogLevel.Warn);
            return false;
        }

        if (!Game1.player.friendshipData.TryGetValue(npc.Name, out Friendship? friendship))
        {
            friendship = new Friendship();
            Game1.player.friendshipData.Add(npc.Name, friendship);
        }

        friendship.Points = hearts * 250;

        this.monitor.Log(
            $"Successfully set {npc.Name}'s friendship to {hearts} hearts.",
            LogLevel.Info
        );

        return true;
    }
}