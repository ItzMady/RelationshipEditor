using System;
using System.Collections.Generic;
using System.Linq;
using StardewModdingAPI;
using StardewValley;

namespace RelationshipEditor.Services;

internal class RelationshipService
{
    private const int PointsPerHeart = 250;

    private readonly IMonitor monitor;

    public RelationshipService(IMonitor monitor)
    {
        this.monitor = monitor;
    }

    public bool SetHearts(string npcName, int hearts)
    {
        NPC? npc = FindNpc(npcName);

        if (npc is null)
        {
            monitor.Log($"NPC '{npcName}' was not found.", LogLevel.Warn);
            return false;
        }

        Friendship friendship = GetFriendship(npc.Name);
        friendship.Points = hearts * PointsPerHeart;

        monitor.Log($"{npc.Name} is now at {hearts} hearts.", LogLevel.Info);

        return true;
    }

    public void SetAllHearts(int hearts)
    {
        int updated = 0;

        foreach (NPC npc in Utility.getAllCharacters())
        {
            if (!npc.CanSocialize)
                continue;

            Friendship friendship = GetFriendship(npc.Name);
            friendship.Points = hearts * PointsPerHeart;

            updated++;
        }

        monitor.Log($"Updated friendship for {updated} NPCs.", LogLevel.Info);
    }

    public void ListNPCs()
    {
        monitor.Log("Available NPCs:", LogLevel.Info);

        foreach (NPC npc in Utility.getAllCharacters()
                     .Where(n => n.CanSocialize)
                     .OrderBy(n => n.Name))
        {
            monitor.Log($"- {npc.Name}", LogLevel.Info);
        }
    }

    private NPC? FindNpc(string name)
    {
        foreach (NPC npc in Utility.getAllCharacters())
        {
            if (!npc.CanSocialize)
                continue;

            if (npc.Name.Equals(name, StringComparison.OrdinalIgnoreCase))
                return npc;
        }

        return null;
    }

    private Friendship GetFriendship(string npcName)
    {
        if (Game1.player.friendshipData.TryGetValue(npcName, out Friendship? friendship))
            return friendship;

        friendship = new Friendship();
        Game1.player.friendshipData.Add(npcName, friendship);

        return friendship;
    }
}
