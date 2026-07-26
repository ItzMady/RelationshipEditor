using System;
using System.Linq;
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
        NPC? npc = Utility.getAllCharacters()
            .Where(n => n.CanSocialize)
            .FirstOrDefault(n =>
                n.Name.Equals(npcName, StringComparison.OrdinalIgnoreCase));

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

        this.monitor.Log($"Successfully set {npc.Name}'s friendship to {hearts} hearts.", LogLevel.Info);

        return true;
    }

    public void SetAllHearts(int hearts)
    {
        int updated = 0;

        foreach (NPC npc in Utility.getAllCharacters()
                     .Where(n => n.CanSocialize))
        {
            if (!Game1.player.friendshipData.TryGetValue(npc.Name, out Friendship? friendship))
            {
                friendship = new Friendship();
                Game1.player.friendshipData.Add(npc.Name, friendship);
            }

            friendship.Points = hearts * 250;
            updated++;
        }

        this.monitor.Log($"Successfully updated {updated} NPCs to {hearts} hearts.", LogLevel.Info);
    }

    public void ListNPCs()
    {
        this.monitor.Log("Available NPCs:", LogLevel.Info);

        foreach (NPC npc in Utility.getAllCharacters()
                     .Where(n => n.CanSocialize)
                     .OrderBy(n => n.Name))
        {
            this.monitor.Log($"- {npc.Name}", LogLevel.Info);
        }
    }
}
