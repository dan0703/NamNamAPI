using System;
using System.Collections.Generic;

namespace NamNamAPI.Models;

public partial class Preference
{
    public string IdPreference { get; set; } = null!;

    public string? IdUser { get; set; }

    public string? IdCategory { get; set; }

    public virtual Category? IdCategoryNavigation { get; set; }

    public virtual User? IdUserNavigation { get; set; }
}
