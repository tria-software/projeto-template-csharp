namespace ProjetoTemplate.Domain.Helpers
{
    public class DateTimeBrazil
    {
        public static DateTime Now()
        {
            try
            {
                return TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time"));

            }
            catch (Exception)
            {
                return DateTime.Now;
            }
        }
    }
}
