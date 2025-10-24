using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookingService.Model.Entity;

public class Review
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    
    [Required]
    [Column]
    public int Grade { get; set; }
    
    [Required]
    [Column]
    public string RaterUsername { get; set; }
    
    [Required]
    [Column]
    public ReviewFor ReviewFor { get; set; }
    
    [Required]
    [Column]
    public string EntityInfo { get; set; }
}