namespace APIDatingApp.Extensions
{
    public static class DateTimeExtensions
    {
        public static int CalculateAge(this DateOnly dob)
        {
            // OJO: Es una cálculo simple de los años
            var today = DateOnly.FromDateTime(DateTime.UtcNow);

            var age = today.Year - dob.Year;

            if (dob > today.AddYears(-age)) age--;

            return age;
        }
    }
}