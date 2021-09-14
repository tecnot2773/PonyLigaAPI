using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace PonyLigaAPI.Models
{
    public class Team
    {
        public int id { get; set; }
        public string club { get; set; }
        public string name { get; set; }
        public int? place { get; set; }
        public string consultor { get; set; }
        public int teamSize { get; set; }
        public int? groupId { get; set; }
        public Group group { get; set; }

        [NotMapped]
        public int? ponyId { get; set; }
        [NotMapped]
        public int? totalScore { get; set; } = 0;

        public ICollection<Pony> ponies { get; set; }
        public ICollection<TeamMember> teamMembers { get; set; }
        public ICollection<Result> results { get; set; }
        public ICollection<TeamPony> teamPonies { get; set; }
    }
}
