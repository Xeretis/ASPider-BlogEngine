using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Data.Queries;

[NotMapped]
public class DepthQuery
{
    public int Id { get; set; }
    public int Depth { get; set; }
}