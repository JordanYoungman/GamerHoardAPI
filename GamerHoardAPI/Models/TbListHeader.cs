using System;
using System.Collections.Generic;

namespace GamerHoardAPI.Models;

public partial class TbListHeader
{
    public long Id { get; set; }

    public string? Name { get; set; }

    public bool? IsPublic { get; set; }

    public long? UserId { get; set; }

    public TimeOnly? DateAdded { get; set; }

    public string? ImageUrl { get; set; }
}
