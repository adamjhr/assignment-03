using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using Assignment3.Core;
namespace Assignment3.Entities;

public class Task
{
    
    public int Id { get; set; }
    
    [StringLength(100),Required]
    public string Title { get; set; } = null!;
    
    [MaxLength]
    public string Description { get; set; } = null!;
    
    [Required]
    public State State { get; set; }

    public virtual ICollection<Tag> Tags { get; set; } = null!;
    
    public DateTime CreatedDate { get; set; }

    public  DateTime StateUpdated { get; set; }
    
    public User AssignedTo { get; set; } = null!;
}
