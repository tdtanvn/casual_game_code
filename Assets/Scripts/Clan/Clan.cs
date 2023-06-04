using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clan
{
    public string id;
    public string name;
    public string description;
    public uint requiredLevel;
    public uint maxMember;
    public List<Member> members;

    public class Member
    {
        public string id;
        public string role;
    }

    public Clan(ClanInfo clanInfo)
    {
        if (clanInfo == null)
        {
            return;
        }

        id = clanInfo.ClanId;
        name = clanInfo.Name;
        description = clanInfo.Description;
        requiredLevel = clanInfo.JoinCondition.RequiredLevel;
        maxMember = clanInfo.MaxMember;
        members = new List<Member>();
        foreach (var m in clanInfo.Members)
        {
            members.Add(new Member
            {
                id = m.Id,
                role = m.Role,
            });
        }
    }

    public Clan(CreateClanOutput clanInfo)
    {
        if (clanInfo == null)
        {
            return;
        }

        id = clanInfo.ClanId;
        name = clanInfo.Name;
        description = clanInfo.Description;
        requiredLevel = clanInfo.JoinCondition.RequiredLevel;
        maxMember = clanInfo.MaxMember;
        members = new List<Member>();
        foreach (var m in clanInfo.Members)
        {
            members.Add(new Member
            {
                id = m.Id,
                role = m.Role,
            });
        }
    }

    public enum State { In, Out }
}
