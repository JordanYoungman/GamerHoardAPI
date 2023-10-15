using System;
using System.Collections.Generic;

namespace GamerHoardAPI.Models;

public partial class TbGameHeader
{
    public int AppId { get; set; }

    public string? Name { get; set; }

    public string? ReleaseDate { get; set; }

    public string? HeaderImage { get; set; }

    public string? Developers { get; set; }

    public string? Publishers { get; set; }

    public string? Categories { get; set; }

    public string? Genres { get; set; }

    public string? Tags { get; set; }
}
