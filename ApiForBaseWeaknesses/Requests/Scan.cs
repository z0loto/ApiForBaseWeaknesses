using System.ComponentModel.DataAnnotations;

namespace ApiForBaseWeaknesses.Requests;

public class Scan: IValidatableObject
{
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public int Page { get; set; }
    public int PageSize { get; set; }
    public bool SortDescending { get; set; } = false;
    public string SortMode { get; set; } = string.Empty;
    //public SortMode SortMode { get; set; } = SortMode.Id;
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if ((EndDate!=null && StartDate!=null) && EndDate<=StartDate )
        {
            yield return new ValidationResult(
                "Некорректно введен период времени",
                new[] { nameof(Scan) });
        }

        if (Page == 0 || PageSize == 0)
        {
            yield return new ValidationResult(
                "Некорректные данные ввода",
                new[] { nameof(Scan) });
        }
    }
}