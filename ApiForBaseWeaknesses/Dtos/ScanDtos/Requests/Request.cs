using System.ComponentModel.DataAnnotations;

namespace ApiForBaseWeaknesses.Dtos.ScanDtos.Requests;

public class Request: IValidatableObject
{
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public int Page { get; set; }
    public int PageSize { get; set; }
    public string? SortBy { get; set; }
    public bool SortDescending { get; set; } = false;
    public SortMode SortMode { get; set; } = SortMode.Id;
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if ((EndDate!=null && StartDate!=null) && EndDate<=StartDate )
        {
            yield return new ValidationResult(
                "Некорректно введен период времени",
                new[] { nameof(Request) });
        }

        if (Page == 0 || PageSize == 0)
        {
            yield return new ValidationResult(
                "Некорректные данные ввода",
                new[] { nameof(Request) });
        }
    }
}