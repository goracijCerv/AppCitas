namespace Apcitas.WebService.Extensions;

public static class DateTimeExtensions
{
    public static int  CalculeteAge(this DateTime dob) 
    {
        var today = DateTime.Today;
        var age = today.Year - dob.Year;

        if (dob.Date > today.AddYears(-age)) age--;

        return age;
    }
}
