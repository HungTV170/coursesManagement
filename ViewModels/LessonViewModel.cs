using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
namespace CourseManagement.ViewModels;
public class LessonViewModel{
    public int Id {get;set;}
    public string? Title {get;set;}
    public IFormFile? Image {get;set;}
    public string? ImagePath {get;set;}
    public string? Introduction { get; set; }
    public string? Content {get;set;}
    [DataType(DataType.Date)]
    [DisplayName("Release Date")]
    public DateTime DateCreated {get;set;}
    [DisplayName("Course")]
    public int CourseId {get;set;}
}

public class LessonRequest{
    [Required]
    public int Id {get;set;}

    [StringLength(60)]
    [Required]
    public string? Title {get;set;}
    public IFormFile? Image { get; set; }
    [StringLength(60)]
    [Required]
    public string? Introduction { get; set; }
    [StringLength(60)]
    [Required]
    public string? Content {get;set;}

    [DisplayName("Release Date")]
    [DataType(DataType.Date)]
    [Required]
    public DateTime DateCreated {get;set;}

    [DisplayName("Course")]
    [Required]
    public int CourseId {get;set;}
}