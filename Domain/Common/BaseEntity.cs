using System.ComponentModel.DataAnnotations;

namespace Domain.Common;

public class BaseEntity
{
    [Key] public int Id { get; set; }

    public DateTime CreatedDate { get; set; }
    public DateTime ModifiedDate { get; set; }
}