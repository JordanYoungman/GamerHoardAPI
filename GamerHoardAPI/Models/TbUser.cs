using System;
using System.Collections.Generic;

namespace GamerHoardAPI.Models;

public partial class TbUser
{
    public long Id { get; set; }

    public DateTime CreatedAt { get; set; }

    public string? Email { get; set; }

    public string? SteamId { get; set; }

    public string? AvatarImg { get; set; }
}
