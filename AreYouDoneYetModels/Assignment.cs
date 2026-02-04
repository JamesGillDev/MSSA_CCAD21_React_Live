using System.ComponentModel.DataAnnotations;

namespace AreYouDoneYetModels;

public class Assignment
{
    public int Id { get; set; }

    [StringLength(255)]
    public string Title { get; set; } = string.Empty;

    [StringLength(500)]
    public string? Description { get; set; }
}
