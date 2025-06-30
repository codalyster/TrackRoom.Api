using System.ComponentModel.DataAnnotations;

namespace TrackRoom.DataAccess.Models
{
    public class ModelBase
    {
        [Key]
        public int Id { get; set; }
    }
}
