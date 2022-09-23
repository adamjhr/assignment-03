using System.ComponentModel.DataAnnotations;

namespace Assignment3.Entities;

public class Tag
{
    public int Id { get; set; }
    
    //Mangler unique
    [StringLength(50),Required]
    public string Name { get; set; }
    
    public virtual ICollection<Task> Tasks { get; set; }
}
