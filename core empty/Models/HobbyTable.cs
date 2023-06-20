using System;
using System.Collections.Generic;

namespace DAL.Models;

public partial class HobbyTable
{
    public int HobbyId { get; set; }

    public string HobbyName { get; set; } = null!;

    public virtual ICollection<Stunthobby> Stunthobbies { get; set; } = new List<Stunthobby>();
}
