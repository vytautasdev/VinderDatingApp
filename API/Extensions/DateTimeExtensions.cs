namespace API.Extensions
{
    public static class DateTimeExtensions
    {
        public static int CalculateUserAge(this DateTime dob)
        {
            var today = DateTime.Today;
            var currentUserAge = today.Year - dob.Year;

            if (dob.Date > today.AddYears(-currentUserAge)) currentUserAge--;
            return currentUserAge;
        }
    }
}